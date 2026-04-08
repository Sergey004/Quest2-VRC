using Quest2_VRC;
using System.Collections.Concurrent;
using System.Timers;
using Timer = System.Timers.Timer;


namespace Plugin.OpenRGBOSC
{
    public class OpenRGBConfig
    {
        public bool EnableLogging { get; set; } = true;
        public int BufferTimeMs { get; set; } = 230;
        public bool UseCustomAddresses { get; set; } = false;
        public string[] CustomAddresses { get; set; } = { "/avatar/parameters/R", "/avatar/parameters/G", "/avatar/parameters/B" };
    }

    public class OpenRGBOSC : IPlugin
    {
        private static readonly string[] DefaultRGBAddresses = {
            "/avatar/parameters/R",
            "/avatar/parameters/G",
            "/avatar/parameters/B"
        };

        private OpenRGBConfig _config;
        private bool _started = false;
        private string[] _activeAddresses;
        private readonly ConcurrentDictionary<string, int> _rgbBuffer = new();
        private readonly Timer _processTimer;

        public string Name => "OpenRGBOSC";
        public string Description => "Controls OpenRGB devices via OSC RGB parameters from VRChat.";

        public OpenRGBOSC()
        {
            _processTimer = new Timer();
            _processTimer.Elapsed += ProcessBufferedData;
            _processTimer.AutoReset = true;
        }

        public void Init()
        {
            _config = this.LoadConfiguration<OpenRGBConfig>();
            _processTimer.Interval = _config.BufferTimeMs;

            if (_config.EnableLogging)
                Console.WriteLine($"[OpenRGBOSC] Initialized with buffer time={_config.BufferTimeMs}ms");

            _activeAddresses = _config.UseCustomAddresses && _config.CustomAddresses.Length > 0
                ? _config.CustomAddresses
                : DefaultRGBAddresses;

            RegisterAddresses();
        }

        private void RegisterAddresses()
        {
            foreach (var address in _activeAddresses)
            {
                if (_config.EnableLogging)
                    Console.WriteLine($"[OpenRGBOSC] Registering address: {address}");
                Receiver.RegisterAddress(address);
            }
        }

        public void Start()
        {
            if (_started) return;

            RegisterAddresses(); // Повторная регистрация при старте
            Receiver.PluginCommandReceived += OnRGBCommandReceived;
            _processTimer.Start();

            // Инициализация OpenRGB
            RGBController.SendRGBRawData(255, 255, 255);  // Init OpenRGB
            System.Threading.Tasks.Task.Delay(20).Wait();
            RGBController.SendRGBRawData(0, 0, 0);        // Set to Black

            _started = true;
            if (_config.EnableLogging)
                Console.WriteLine("[OpenRGBOSC] Started");
        }

        public void Stop()
        {
            if (!_started) return;

            Receiver.PluginCommandReceived -= OnRGBCommandReceived;
            _processTimer.Stop();
            _started = false;

            if (_config.EnableLogging)
                Console.WriteLine("[OpenRGBOSC] Stopped");
        }

        private void OnRGBCommandReceived(string address, object value)
        {
            // Обработка только RGB адресов
            if (!IsRGBAddress(address)) return;

            int rgbValue;
            if (value is float floatValue)
            {
                rgbValue = (int)(floatValue * 255f);
                if (_config.EnableLogging)
                    Console.WriteLine($"[OpenRGBOSC] Received {address}: {floatValue} (converted to {rgbValue})");
            }
            else if (value is int intValue)
            {
                rgbValue = intValue;
                if (_config.EnableLogging)
                    Console.WriteLine($"[OpenRGBOSC] Received {address}: {intValue}");
            }
            else
            {
                if (_config.EnableLogging)
                    Console.WriteLine($"[OpenRGBOSC] Invalid value type for {address}: {value}");
                return;
            }

            _rgbBuffer[address] = rgbValue;
        }

        private bool IsRGBAddress(string address)
        {
            return Array.Exists(_activeAddresses, addr => addr.Equals(address, StringComparison.OrdinalIgnoreCase));
        }

        private void ProcessBufferedData(object sender, ElapsedEventArgs e)
        {
            if (_rgbBuffer.Count == 0) return;

            int r = GetRGBValue("R");
            int g = GetRGBValue("G");
            int b = GetRGBValue("B");

            if (_config.EnableLogging)
                Console.WriteLine($"[OpenRGBOSC] Processing RGB: R={r}, G={g}, B={b}");

            ProcessRGB(r, g, b);
            _rgbBuffer.Clear();
        }

        private int GetRGBValue(string component)
        {
            foreach (var kvp in _rgbBuffer)
            {
                if (kvp.Key.EndsWith(component, StringComparison.OrdinalIgnoreCase))
                    return kvp.Value;
            }
            return 0;
        }
        private void ProcessRGB(int r, int g, int b)
        {
            try
            {
                RGBController.SendRGBRawData(r, g, b);
            }
            catch (Exception ex)
            {
                if (_config.EnableLogging)
                    Console.WriteLine($"[OpenRGBOSC] Error sending RGB data: {ex.Message}");
            }
        }
    }
}