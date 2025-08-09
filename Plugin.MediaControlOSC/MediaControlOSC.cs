using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Quest2_VRC;
using System.Linq;

namespace Plugin.MediaControlOSC
{
    public class MediaControlConfig
    {
        public bool EnableLogging { get; set; } = false;
        public bool UseCustomAddresses { get; set; } = false;
        public string[] CustomAddresses { get; set; } = Array.Empty<string>();
    }

    public class MediaControlOSC : IPlugin
    {
        private static readonly string[] DefaultMediaAddresses = {
            "/avatar/parameters/MediaPlayPause",
            "/avatar/parameters/MediaPlay",
            "/avatar/parameters/MediaPause",
            "/avatar/parameters/MediaNext",
            "/avatar/parameters/MediaPrevious"
        };

        private MediaControlConfig _config;
        private bool _started = false;
        private string[] _activeAddresses;

        public string Name => "MediaControlOSC";
        public string Description => "Controls media playback via OSC commands from VRChat.";

        public void Init()
        {
            // Загрузка конфига
            _config = this.LoadConfiguration<MediaControlConfig>();
            Console.WriteLine($"[MediaControlOSC] Initialized with logging={_config.EnableLogging}");

            _activeAddresses = _config.UseCustomAddresses && _config.CustomAddresses.Length > 0 
                ? _config.CustomAddresses 
                : DefaultMediaAddresses;

            RegisterAddresses();
        }

        private void RegisterAddresses()
        {
            foreach (var address in _activeAddresses)
            {
                if (_config.EnableLogging)
                    Console.WriteLine($"[MediaControlOSC] Registering address: {address}");
                Quest2_VRC.PluginReceiver.RegisterPluginAddress(address);
            }
        }

        public void Start()
        {
            if (_started) return;
            RegisterAddresses(); // Повторная регистрация при старте
            Quest2_VRC.PluginReceiver.PluginCommandReceived += OnMediaControlCommandReceived;
            _started = true;
            if (_config.EnableLogging)
                Console.WriteLine("[MediaControlOSC] Started");
        }

        public void Stop()
        {
            if (!_started) return;
            Quest2_VRC.PluginReceiver.PluginCommandReceived -= OnMediaControlCommandReceived;
            _started = false;
            if (_config.EnableLogging)
                Console.WriteLine("[MediaControlOSC] Stopped");
        }

        private void OnMediaControlCommandReceived(string address, object value)
        {
            if (_config.EnableLogging)
                Console.WriteLine($"[MediaControlOSC] Received {address}: {value}");

            string command = address.Split('/').LastOrDefault()?.ToLower() ?? "";
            
            switch (command)
            {
                case "mediaplaypause":
                    HandlePlayPause(value);
                    break;
                case "mediaplay":
                    MediaPlay();
                    break;
                case "mediapause":
                    MediaPause();
                    break;
                case "medianext":
                    MediaNext();
                    break;
                case "mediaprevious":
                    MediaPrevious();
                    break;
            }
        }

        private void HandlePlayPause(object value)
        {
            bool isPlay = value switch
            {
                bool b => b,
                int i => i != 0,
                float f => Math.Abs(f) > 0.5f,
                double d => Math.Abs(d) > 0.5,
                _ => false
            };

            if (isPlay)
            {
                if (_config.EnableLogging)
                    Console.WriteLine("[MediaControlOSC] Play command");
                MediaPlay();
            }
            else
            {
                if (_config.EnableLogging)
                    Console.WriteLine("[MediaControlOSC] Pause command");
                MediaPause();
            }
        }

        private void MediaPlay() => SendMediaKey(MediaKeyAction.Play);
        private void MediaPause() => SendMediaKey(MediaKeyAction.Pause);
        private void MediaNext() => SendMediaKey(MediaKeyAction.Next);
        private void MediaPrevious() => SendMediaKey(MediaKeyAction.Previous);

        private enum MediaKeyAction { Play, Pause, Next, Previous }

        private void SendMediaKey(MediaKeyAction action)
        {
            if (_config.EnableLogging)
                Console.WriteLine($"[MediaControlOSC] Sending media key: {action}");

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                switch (action)
                {
                    case MediaKeyAction.Play:
                    case MediaKeyAction.Pause:
                        keybd_event(0xB3, 0, 0, 0); // Play/Pause
                        keybd_event(0xB3, 0, 2, 0);
                        break;
                    case MediaKeyAction.Next:
                        keybd_event(0xB0, 0, 0, 0); // Next Track
                        keybd_event(0xB0, 0, 2, 0);
                        break;
                    case MediaKeyAction.Previous:
                        keybd_event(0xB1, 0, 0, 0); // Previous Track
                        keybd_event(0xB1, 0, 2, 0);
                        break;
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                string cmd = action switch
                {
                    MediaKeyAction.Play => "play-pause",
                    MediaKeyAction.Pause => "play-pause",
                    MediaKeyAction.Next => "next",
                    MediaKeyAction.Previous => "previous",
                    _ => ""
                };
                if (!string.IsNullOrEmpty(cmd))
                {
                    try { Process.Start("playerctl", cmd); } catch { }
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                string script = action switch
                {
                    MediaKeyAction.Play => "tell application \"Music\" to playpause",
                    MediaKeyAction.Pause => "tell application \"Music\" to playpause",
                    MediaKeyAction.Next => "tell application \"Music\" to next track",
                    MediaKeyAction.Previous => "tell application \"Music\" to previous track",
                    _ => ""
                };
                if (!string.IsNullOrEmpty(script))
                {
                    try { Process.Start("osascript", $"-e \"{script}\"" ); } catch { }
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
    }
}
