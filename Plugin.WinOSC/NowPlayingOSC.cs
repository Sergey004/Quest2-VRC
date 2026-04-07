using System;
using System.Threading.Tasks;
using Quest2_VRC;
using Windows.Media.Control;

namespace Plugin.WinOSC
{
    public class WinOSC : IPlugin
    {
        private GlobalSystemMediaTransportControlsSessionManager? _sessionManager;
        private GlobalSystemMediaTransportControlsSession? _currentSession;


        private string _lastTrackInfo = string.Empty;
        private static readonly string[] _allowedApps = {
    "Spotify.exe", "foobar2000.exe", "AIMP.exe", "Winamp.exe",
    "MusicBee.exe", "TIDAL.exe", "Deezer.exe", "vlc.exe",
    "mpc-hc64.exe", "mpc-be64.exe",
    "SpotifyAB.SpotifyMusic", "AppleInc.AppleMusicWin",
    "Microsoft.ZuneMusic", "TidalMusicAS.TIDAL",
};
        private static bool IsAllowedApp(string appId) =>
    _allowedApps.Any(a => appId.StartsWith(a, StringComparison.OrdinalIgnoreCase));

        private CancellationTokenSource? _cts;
        private readonly object _lockObject = new object();
                                                            
        public string Name => "WindowsMediaOSC";
        public string Description => "Sends current Windows media track info to VRChat via OSC.";

        public void Init()
        {
            Console.WriteLine($"[{Name}] Initializing SMTC media handler...");
        }

        public async void Start()
        {
            try
            {
                _sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();

                if (_sessionManager != null)
                {
                    _sessionManager.CurrentSessionChanged += OnCurrentSessionChanged;
                    UpdateSession(_sessionManager.GetCurrentSession());
                    _cts = new CancellationTokenSource();        
                    _ = PollLoop(_cts.Token);

                    Console.WriteLine($"[{Name}] Successfully connected to Windows SMTC.");
                }
                else
                {
                    Console.WriteLine($"[{Name}] Failed to initialize Session Manager.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{Name}] Startup error: {ex.Message}");
            }
        }

        public void Stop()
        {
            _cts?.Cancel();
            lock (_lockObject)
            {
                if (_currentSession != null)
                {
                    _currentSession.MediaPropertiesChanged -= OnMediaPropertiesChanged;
                    _currentSession.PlaybackInfoChanged -= OnPlaybackInfoChanged;
                    _currentSession = null;
                }

                if (_sessionManager != null)
                {
                    _sessionManager.CurrentSessionChanged -= OnCurrentSessionChanged;
                    _sessionManager = null;
                }
            }

            Console.WriteLine($"[{Name}] Stopped.");
        }

        private void OnCurrentSessionChanged(GlobalSystemMediaTransportControlsSessionManager sender, CurrentSessionChangedEventArgs args)
        {
            UpdateSession(sender.GetCurrentSession());
        }

        private void UpdateSession(GlobalSystemMediaTransportControlsSession? session)
        {
            lock (_lockObject)
            {
                // Unsubscribe from the old session before switching
                if (_currentSession != null)
                {
                    _currentSession.MediaPropertiesChanged -= OnMediaPropertiesChanged;
                }

                _currentSession = session;

                if (_currentSession != null)
                {
                    _currentSession.MediaPropertiesChanged += OnMediaPropertiesChanged;
                    _currentSession.PlaybackInfoChanged += OnPlaybackInfoChanged;  

                    // Fire-and-forget initial update
                    _ = UpdateTrackInfoAsync(_currentSession);
                }
            }
        }

        private async void OnMediaPropertiesChanged(GlobalSystemMediaTransportControlsSession sender, MediaPropertiesChangedEventArgs args)
        {
            await UpdateTrackInfoAsync(sender);
        }

        private async Task UpdateTrackInfoAsync(GlobalSystemMediaTransportControlsSession session)
        {
            try
            {
                var appId = session.SourceAppUserModelId ?? "";
                if (!IsAllowedApp(appId))
                {
                    //Console.WriteLine($"[{Name}] Skipped: {appId}");
                    return;
                }

                var mediaProperties = await session.TryGetMediaPropertiesAsync();
                if (mediaProperties == null) return;

                string title = mediaProperties.Title;
                string artist = mediaProperties.Artist;
                string album = mediaProperties.AlbumTitle;

                if (string.IsNullOrWhiteSpace(title)) return;

                string trackInfo = string.IsNullOrWhiteSpace(artist) ? title : $"{artist} - {title}";
                
                if (!string.IsNullOrWhiteSpace(album))
                    trackInfo += $" ({album})";
                trackInfo = trackInfo.Replace('–', '-').Replace('—', '-').Replace('―', '-');

                // Prevent spamming OSC if the track hasn't actually changed
                lock (_lockObject)
                {
                    if (trackInfo == _lastTrackInfo) return;
                    _lastTrackInfo = trackInfo;

                }

                string message = $"Now Listening: {trackInfo}";

                PacketSender.SendPacket(new VRChatMessage("input", message));
                Console.WriteLine($"[{Name}] Sent: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[{Name}] Error fetching track info: {ex.Message}");
            }
        }

        private async void OnPlaybackInfoChanged(
    GlobalSystemMediaTransportControlsSession sender,
    PlaybackInfoChangedEventArgs args)
        {
            var playback = sender.GetPlaybackInfo();
            if (playback?.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing)
                await UpdateTrackInfoAsync(sender);
        }

        private async Task PollLoop(CancellationToken ct)
        {
            while (!ct.IsCancellationRequested)
            {
                await Task.Delay(5000, ct).ContinueWith(_ => { });
                GlobalSystemMediaTransportControlsSession? session;
                lock (_lockObject) { session = _currentSession; }
                if (session != null)
                    await UpdateTrackInfoAsync(session);
            }
        }
        #region Legacy Interface Compatibility
        // Stubs left to prevent breaking changes if the main app calls them via Reflection
        public void RequestAccessToken() { }
        public void StartOAuthWebServer() { }
        public string? GetAccessToken() => null;
        #endregion
    }
}