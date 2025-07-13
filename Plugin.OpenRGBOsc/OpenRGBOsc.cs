using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using OpenRGB.NET;
using System.Text;
using Quest2_VRC;

namespace Plugin.OpenRGBOsc
{
    public class OpenRGBOsc : IPlugin
    {
        private static readonly string[] RGBAddresses = {
            "/avatar/parameters/R",
            "/avatar/parameters/G",
            "/avatar/parameters/B"
        };
        private readonly ConcurrentDictionary<string, int> _rgbBuffer = new();
        private Timer _rgbTimer;
        private bool _started = false;
        private UdpClient _oscListener;
        private CancellationTokenSource _cts;
        private int _oscPort = 9000; // Можно вынести в конфиг

        public string Name => "OpenRGBOsc";
        public string Description => "Controls OpenRGB via OSC commands from VRChat (standalone).";

        public void Init()
        {
            // Можно добавить чтение порта из конфига
        }

        public void Start()
        {
            if (_started) return;
            _cts = new CancellationTokenSource();
            StartOscListener(_cts.Token);
            _rgbTimer = new Timer(ProcessRGB, null, 0, 250);
            _started = true;
            Console.WriteLine("[OpenRGBOsc] Started (standalone OSC receiver)");
        }

        public void Stop()
        {
            if (!_started) return;
            _cts.Cancel();
            _oscListener?.Close();
            _rgbTimer?.Dispose();
            _started = false;
            Console.WriteLine("[OpenRGBOsc] Stopped");
        }

        private void StartOscListener(CancellationToken token)
        {
            Task.Run(async () =>
            {
                try
                {
                    _oscListener = new UdpClient(_oscPort);
                    Console.WriteLine($"[OpenRGBOsc] Listening OSC on UDP port {_oscPort}");
                    while (!token.IsCancellationRequested)
                    {
                        var result = await _oscListener.ReceiveAsync();
                        ParseOscPacket(result.Buffer);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[OpenRGBOsc] OSC listener error: {ex.Message}");
                }
            }, token);
        }

        private void ParseOscPacket(byte[] data)
        {
            // Минимальный парсер OSC-пакета для float/int по нужным адресам
            try
            {
                int idx = 0;
                string address = ReadOscString(data, ref idx);
                if (!RGBAddresses.Contains(address)) return;
                string typeTag = ReadOscString(data, ref idx);
                if (typeTag.Length < 2 || typeTag[0] != ',') return;
                if (typeTag[1] == 'i' && data.Length >= idx + 4)
                {
                    int value = ReadInt32(data, idx);
                    _rgbBuffer[address] = value;
                    Console.WriteLine($"[OpenRGBOsc] OSC {address}: {value}");
                }
                else if (typeTag[1] == 'f' && data.Length >= idx + 4)
                {
                    float fval = ReadFloat32(data, idx);
                    int value = (int)fval;
                    _rgbBuffer[address] = value;
                    Console.WriteLine($"[OpenRGBOsc] OSC {address}: {value}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OpenRGBOsc] OSC parse error: {ex.Message}");
            }
        }

        private string ReadOscString(byte[] data, ref int idx)
        {
            int start = idx;
            while (idx < data.Length && data[idx] != 0) idx++;
            string s = Encoding.ASCII.GetString(data, start, idx - start);
            idx++;
            while (idx % 4 != 0) idx++;
            return s;
        }
        private int ReadInt32(byte[] data, int idx)
        {
            if (BitConverter.IsLittleEndian)
            {
                byte[] b = new byte[4];
                Array.Copy(data, idx, b, 0, 4);
                Array.Reverse(b);
                return BitConverter.ToInt32(b, 0);
            }
            return BitConverter.ToInt32(data, idx);
        }
        private float ReadFloat32(byte[] data, int idx)
        {
            if (BitConverter.IsLittleEndian)
            {
                byte[] b = new byte[4];
                Array.Copy(data, idx, b, 0, 4);
                Array.Reverse(b);
                return BitConverter.ToSingle(b, 0);
            }
            return BitConverter.ToSingle(data, idx);
        }

        private void ProcessRGB(object state)
        {
            int r = _rgbBuffer.TryGetValue("/avatar/parameters/R", out var rv) ? rv : 0;
            int g = _rgbBuffer.TryGetValue("/avatar/parameters/G", out var gv) ? gv : 0;
            int b = _rgbBuffer.TryGetValue("/avatar/parameters/B", out var bv) ? bv : 0;
            Console.WriteLine($"[OpenRGBOsc] Processing RGB: R={r}, G={g}, B={b}");
            SendRGBRawData(r, g, b);
            _rgbBuffer.Clear();
        }

        private void SendRGBRawData(int R, int G, int B)
        {
            try
            {
                using var client = new OpenRgbClient(name: "OpenRGBOsc", autoConnect: true, timeoutMs: 1000);
                var devices = client.GetAllControllerData();
                var R_Byte = (byte)R;
                var G_Byte = (byte)G;
                var B_Byte = (byte)B;
                for (int i = 0; i < devices.Length; i++)
                {
                    var leds = System.Linq.Enumerable.Range(0, devices[i].Colors.Length)
                        .Select(_ => new OpenRGB.NET.Color(R_Byte, G_Byte, B_Byte))
                        .ToArray();
                    client.UpdateLeds(i, leds);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[OpenRGBOsc] OpenRGB error: {ex.Message}");
            }
        }
    }
}
