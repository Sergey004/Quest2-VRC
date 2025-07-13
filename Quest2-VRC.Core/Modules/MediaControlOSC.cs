using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using static Quest2_VRC.PacketSender;

namespace Quest2_VRC
{
    public static class MediaControlOSC
    {
        // Запуск OSC-приемника для управления мультимедиа
        public static void StartMediaControlOSC()
        {
            // Можно вынести в отдельный поток, если потребуется
            Receiver.MediaControlCommandReceived += OnMediaControlCommandReceived;
        }

        // Обработчик команд OSC для управления мультимедиа
        private static void OnMediaControlCommandReceived(string command)
        {
            switch (command.ToLower())
            {
                case "play":
                    MediaPlay();
                    break;
                case "pause":
                    MediaPause();
                    break;
                case "next":
                    MediaNext();
                    break;
                case "previous":
                    MediaPrevious();
                    break;
            }
        }

        // Методы управления мультимедиа через Windows API
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
