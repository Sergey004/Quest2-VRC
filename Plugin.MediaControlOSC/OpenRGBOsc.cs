using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Threading;
using Quest2_VRC;

namespace Plugin.MediaControlOSC
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

        public string Name => "OpenRGBOsc";
        public string Description => "Controls OpenRGB via OSC commands from VRChat.";

        public void Init()
        {
            foreach (var address in RGBAddresses)
            {
                Quest2_VRC.Receiver.RegisterOSCAddress(address);
            }
        }

        public void Start()
        {
            if (_started) return;
            Quest2_VRC.Receiver.MediaControlCommandReceived += OnRGBOscMessage;
            _rgbTimer = new Timer(ProcessRGB, null, 0, 250);
            _started = true;
            Console.WriteLine("[OpenRGBOsc] Started");
        }

        public void Stop()
        {
            if (!_started) return;
            Quest2_VRC.Receiver.MediaControlCommandReceived -= OnRGBOscMessage;
            _rgbTimer?.Dispose();
            _started = false;
            Console.WriteLine("[OpenRGBOsc] Stopped");
        }

        private void OnRGBOscMessage(string address)
        {
            // address: "R", "G", "B" (string)
            if (RGBAddresses.Contains($"/avatar/parameters/{address}") && TryGetOscIntValue(address, out int value))
            {
                _rgbBuffer[$"/avatar/parameters/{address}"] = value;
                Console.WriteLine($"[OpenRGBOsc] Buffered {address}: {value}");
            }
        }

        private bool TryGetOscIntValue(string address, out int value)
        {
            // TODO: Реализовать получение значения из OSC сообщения (требуется интеграция с Receiver)
            value = 0;
            return false;
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
                using var client = new OpenRGB.NET.OpenRgbClient(name: "OpenRGBOsc", autoConnect: true, timeoutMs: 1000);
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
