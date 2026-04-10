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
        private static readonly Dictionary<string, string> _allowedApps = new(StringComparer.OrdinalIgnoreCase)
{
    // Win32 players
    { "Spotify.exe",            "Spotify" },
    { "foobar2000.exe",         "foobar2000" },
    { "AIMP.exe",               "AIMP" },
    { "Winamp.exe",             "Winamp" },
    { "MusicBee.exe",           "MusicBee" },
    { "TIDAL.exe",              "TIDAL" },
    { "Deezer.exe",             "Deezer" },
    { "wmplayer.exe",            "Windows Media Player Legacy" },
    { "itunes.exe",             "iTunes" },
    // Video players (often used for music videos)
    { "vlc.exe",                "VLC" },
    { "mpc-hc64.exe",           "MPC-HC" },
    { "mpc-be64.exe",           "MPC-BE" },
    // Store
    { "SpotifyAB.SpotifyMusic", "Spotify" },
    { "AppleInc.AppleMusicWin", "Apple Music" },
    { "Microsoft.ZuneMusic",    "Windows Media Player" },
    { "TidalMusicAS.TIDAL",     "TIDAL" },

};
        private static string? GetAppFriendlyName(string appId) =>
     _allowedApps
         .FirstOrDefault(kv => appId.StartsWith(kv.Key, StringComparison.OrdinalIgnoreCase))
         .Value;

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
                var appName = GetAppFriendlyName(appId);
                if (appName == null)
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

                string message = $"Now Listening: {trackInfo} | {appName}";

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
            Console.WriteLine($"[{Name}] PlaybackStatus: {playback?.PlaybackStatus}");

            if (playback?.PlaybackStatus == GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused)
            {
                lock (_lockObject) { _lastTrackInfo = string.Empty; }
                return;
            }

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