using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Quest2_VRC;

namespace Plugin.MediaControlOSC
{
    public class MediaControlOSC : IPlugin
    {
        private static readonly string[] MediaAddresses = {
            "/avatar/parameters/MediaPlay",
            "/avatar/parameters/MediaPause",
            "/avatar/parameters/MediaNext",
            "/avatar/parameters/MediaPrevious"
        };
        private bool _started = false;

        public string Name => "MediaControlOSC";
        public string Description => "Controls media playback via OSC commands from VRChat.";

        public void Init()
        {
            foreach (var address in MediaAddresses)
            {
                Quest2_VRC.Receiver.RegisterOSCAddress(address);
            }
        }

        public void Start()
        {
            if (_started) return;
            Quest2_VRC.Receiver.MediaControlCommandReceived += OnMediaControlCommandReceived;
            _started = true;
        }

        public void Stop()
        {
            if (!_started) return;
            Quest2_VRC.Receiver.MediaControlCommandReceived -= OnMediaControlCommandReceived;
            _started = false;
        }

        private static void OnMediaControlCommandReceived(string command)
        {
            switch (command.ToLower())
            {
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

        private static void MediaPlay() => SendMediaKey(MediaKeyAction.Play);
        private static void MediaPause() => SendMediaKey(MediaKeyAction.Pause);
        private static void MediaNext() => SendMediaKey(MediaKeyAction.Next);
        private static void MediaPrevious() => SendMediaKey(MediaKeyAction.Previous);

        private enum MediaKeyAction { Play, Pause, Next, Previous }

        private static void SendMediaKey(MediaKeyAction action)
        {
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
                    try { Process.Start("osascript", $"-e \"{script}\""); } catch { }
                }
            }
        }

        [DllImport("user32.dll")]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
    }
}
