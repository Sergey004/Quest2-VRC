using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Quest2_VRC;
using System.Net;
using System.Web;

namespace Plugin.SpotifyOSC
{
    public class SpotifyConfig
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public string? ClientId { get; set; }
        public string? ClientSecret { get; set; }

        public SpotifyConfig()
        {
            AccessToken = null;
            RefreshToken = null;
            ClientId = null;
            ClientSecret = null;
        }
    }

    public class SpotifyOSC : IPlugin
    {
        private SpotifyConfig _config;
        private static HttpListener? _httpListener;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string _lastTrackId = string.Empty;
        private Task? _worker;
        private bool _running = false;

        public string Name => "SpotifyOSC";
        public string Description => "Sends current Spotify track info to VRChat via OSC.";

        public void Init()
        {
            // ��������� ������ ��������� ������� ������������ ��������
            _config = this.LoadConfiguration<SpotifyConfig>();
            Console.WriteLine($"[SpotifyOSC] Initialized with config: ClientId={!string.IsNullOrEmpty(_config.ClientId)}, AccessToken={!string.IsNullOrEmpty(_config.AccessToken)}");
            // ���� ���� ������ ������ � vars.json, ��������� ���
            MigrateOldConfig();
        }

        // ����� ��������� ����� ��� ������� ������� ������
        public void RequestAccessToken()
        {
            StartOAuthFlow();
        }

        private void MigrateOldConfig()
        {
            if (!File.Exists("vars.json")) return;
            try
            {
                var json = File.ReadAllText("vars.json");
                var jObj = JObject.Parse(json);
                bool configChanged = false;
                if (string.IsNullOrEmpty(_config.AccessToken) && jObj["SpotifyToken"] != null)
                {
                    _config.AccessToken = jObj["SpotifyToken"]?.ToString();
                    configChanged = true;
                }
                if (string.IsNullOrEmpty(_config.RefreshToken) && jObj["SpotifyRefreshToken"] != null)
                {
                    _config.RefreshToken = jObj["SpotifyRefreshToken"]?.ToString();
                    configChanged = true;
                }
                if (string.IsNullOrEmpty(_config.ClientId) && jObj["SpotifyClientId"] != null)
                {
                    _config.ClientId = jObj["SpotifyClientId"]?.ToString();
                    configChanged = true;
                }
                if (string.IsNullOrEmpty(_config.ClientSecret) && jObj["SpotifyClientSecret"] != null)
                {
                    _config.ClientSecret = jObj["SpotifyClientSecret"]?.ToString();
                    configChanged = true;
                }
                if (configChanged)
                {
                    Console.WriteLine("[SpotifyOSC] Migrated configuration from vars.json");
                    this.SaveConfiguration(_config);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SpotifyOSC] Failed to migrate old config: {ex.Message}");
            }
        }

        public void Start()
        {
            // Всегда запускаем OAuth сервер для доступа к /login
            StartOAuthFlow();
            
            if (!string.IsNullOrEmpty(_config.AccessToken))
            {
                _running = true;
                _worker = Task.Factory.StartNew(Run, TaskCreationOptions.LongRunning);
            }
            else
            {
                Console.WriteLine("[SpotifyOSC] No access token found. Please visit http://localhost:8888/login to authorize.");
            }
        }

        public void Stop()
        {
            _running = false;
            
            // Останавливаем HTTP listener если он запущен
            if (_httpListener != null && _httpListener.IsListening)
            {
                try
                {
                    _httpListener.Stop();
                    _httpListener.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SpotifyOSC] Error stopping HTTP listener: {ex.Message}");
                }
            }
        }

        private async void Run()
        {
            while (_running)
            {
                try
                {
                    var track = await GetCurrentTrack();
                    if (track != null && track.Id != _lastTrackId)
                    {
                        _lastTrackId = track.Id;
                        
                        // Формируем строку с информацией о треке
                        var trackInfo = $"Now Playing: {track.Name} - {track.Artist} ({track.Album})";
                        
                        PacketSender.SendPacket(
                            // Отправляем в inputbox
                            new VRChatMessage("input", trackInfo)
                        );
                        
                        Console.WriteLine($"[SpotifyOSC] Sent track info: {trackInfo}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[SpotifyOSC] Error in Run: {ex.Message}");
                }
                await Task.Delay(3000);
            }
        }

        // --- OAuth2 PKCE Implementation (vrc-osc-spotify style) ---
        private string _codeVerifier = string.Empty;
        private string _codeChallenge = string.Empty;

        private void StartOAuthFlow()
        {
            if (string.IsNullOrEmpty(_config.ClientId))
            {
                Logger.LogToConsole("Spotify Client ID missing in config");
                return;
            }
            
            // Проверяем, не запущен ли уже HTTP listener
            if (_httpListener != null && _httpListener.IsListening)
            {
                Console.WriteLine("[SpotifyOSC] OAuth server already running on http://localhost:8888/login");
                return;
            }
            if (string.IsNullOrEmpty(_config.AccessToken)==true)

            {
                GeneratePkceCodes();
                Console.WriteLine("[SpotifyOSC] Starting OAuth PKCE flow...");
                Console.WriteLine("[SpotifyOSC] Visit http://localhost:8888/login to authorize Spotify access");

                _httpListener = new HttpListener();
                _httpListener.Prefixes.Add("http://localhost:8888/login/");
                _httpListener.Prefixes.Add("http://localhost:8888/callback/");
                _httpListener.Start();
                Task.Run(async () => await HandleOAuthRequests());
            }
            
        }

        private async Task HandleOAuthRequests()
        {
            while (_httpListener != null && _httpListener.IsListening)
            {
                var context = await _httpListener.GetContextAsync();
                var url = context.Request.Url?.AbsolutePath;
                if (url == "/login")
                {
                    var authUrl = $"https://accounts.spotify.com/authorize?client_id={_config.ClientId}&response_type=code&redirect_uri=http://localhost:8888/callback/&scope=user-read-currently-playing&code_challenge_method=S256&code_challenge={_codeChallenge}";
                    context.Response.Redirect(authUrl);
                    context.Response.Close();
                }
                else if (url == "/callback/")
                {
                    var code = System.Web.HttpUtility.ParseQueryString(context.Request.Url?.Query ?? "").Get("code");
                    var responseString = "<html><body>Spotify authorization complete. You may close this window.</body></html>";
                    var buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
                    context.Response.ContentLength64 = buffer.Length;
                    await context.Response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
                    context.Response.OutputStream.Close();
                    _httpListener.Stop();
                    await ExchangeCodeForToken(code ?? "");
                    break;
                }
                else
                {
                    context.Response.StatusCode = 404;
                    context.Response.Close();
                }
            }
        }

        private void GeneratePkceCodes()
        {
            // Generate a high-entropy code verifier
            var rng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var bytes = new byte[32];
            rng.GetBytes(bytes);
            _codeVerifier = Convert.ToBase64String(bytes).TrimEnd('=');
            // Create code challenge
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var challengeBytes = sha256.ComputeHash(System.Text.Encoding.ASCII.GetBytes(_codeVerifier));
            _codeChallenge = Base64UrlEncode(challengeBytes);
        }

        private static string Base64UrlEncode(byte[] input)
        {
            return Convert.ToBase64String(input).TrimEnd('=')
                .Replace('+', '-')
                .Replace('/', '_');
        }

        private async Task ExchangeCodeForToken(string code)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
                var body = $"grant_type=authorization_code&code={code}&redirect_uri=http://localhost:8888/callback/&client_id={_config.ClientId}&code_verifier={_codeVerifier}";
                request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                
                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[SpotifyOSC] Token response: {json}"); // Debug log
                
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[SpotifyOSC] Failed to exchange code for token: {response.StatusCode}");
                    return;
                }

                var obj = JObject.Parse(json);
                var accessToken = obj["access_token"]?.ToString();
                var refreshToken = obj["refresh_token"]?.ToString();

                if (string.IsNullOrEmpty(accessToken))
                {
                    Console.WriteLine("[SpotifyOSC] No access_token in response");
                    return;
                }

                _config.AccessToken = accessToken;
                _config.RefreshToken = refreshToken;
                
                Console.WriteLine("[SpotifyOSC] Saving new tokens to config...");
                this.SaveConfiguration(_config);
                Console.WriteLine("[SpotifyOSC] AccessToken received and saved");

                // После получения токена запускаем основной функционал
                if (!_running)
                {
                    _running = true;
                    _worker = Task.Factory.StartNew(Run, TaskCreationOptions.LongRunning);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SpotifyOSC] Error exchanging code for token: {ex.Message}");
            }
        }

        private async Task RefreshAccessToken()
        {
            if (string.IsNullOrEmpty(_config.RefreshToken))
            {
                Console.WriteLine("[SpotifyOSC] No refresh token available");
                return;
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, "https://accounts.spotify.com/api/token");
                var body = $"grant_type=refresh_token&refresh_token={_config.RefreshToken}&client_id={_config.ClientId}";
                
                // Добавляем client_secret только если он есть
                if (!string.IsNullOrEmpty(_config.ClientSecret))
                {
                    body += $"&client_secret={_config.ClientSecret}";
                }
                
                request.Content = new StringContent(body, System.Text.Encoding.UTF8, "application/x-www-form-urlencoded");
                
                var response = await _httpClient.SendAsync(request);
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"[SpotifyOSC] Refresh response: {json}"); // Debug log

                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"[SpotifyOSC] Failed to refresh token: {response.StatusCode}");
                    return;
                }

                var obj = JObject.Parse(json);
                var accessToken = obj["access_token"]?.ToString();

                if (string.IsNullOrEmpty(accessToken))
                {
                    Console.WriteLine("[SpotifyOSC] No access_token in refresh response");
                    return;
                }

                _config.AccessToken = accessToken;
                
                // Обновляем refresh_token если он пришёл в ответе
                var newRefreshToken = obj["refresh_token"]?.ToString();
                if (!string.IsNullOrEmpty(newRefreshToken))
                {
                    _config.RefreshToken = newRefreshToken;
                }
                
                Console.WriteLine("[SpotifyOSC] Saving refreshed tokens to config...");
                this.SaveConfiguration(_config);
                Console.WriteLine("[SpotifyOSC] AccessToken refreshed and saved");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SpotifyOSC] Error refreshing token: {ex.Message}");
            }
        }

        private async Task<SpotifyTrack?> GetCurrentTrack()
        {
            if (string.IsNullOrEmpty(_config.AccessToken))
                return null;
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/me/player/currently-playing");
            request.Headers.Add("Authorization", $"Bearer {_config.AccessToken}");
            var response = await _httpClient.SendAsync(request);
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                await RefreshAccessToken();
                return await GetCurrentTrack();
            }
            if (!response.IsSuccessStatusCode)
                return null;
            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            if (!root.TryGetProperty("item", out var item))
                return null;
            var id = item.GetProperty("id").GetString();
            var name = item.GetProperty("name").GetString();
            var album = item.GetProperty("album").GetProperty("name").GetString();
            var artist = item.GetProperty("artists")[0].GetProperty("name").GetString();
            return new SpotifyTrack(id ?? "", name ?? "", artist ?? "", album ?? "");
        }

        private record SpotifyTrack(string Id, string Name, string Artist, string Album);
        // Получить AccessToken (публичный метод)
        public string? GetAccessToken()
        {
            return _config?.AccessToken;
        }
        // Публичный метод для запуска OAuth веб-сервера вручную
        public void StartOAuthWebServer()
        {
            StartOAuthFlow();
        }
    }
}
