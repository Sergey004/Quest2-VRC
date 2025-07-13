using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using Quest2_VRC;

namespace Plugin.SpotifyOSC
{
    public class SpotifyOSC : IPlugin
    {
        private static string? _accessToken;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string _lastTrackId = string.Empty;
        private Task? _worker;
        private bool _running = false;

        public string Name => "SpotifyOSC";
        public string Description => "Sends current Spotify track info to VRChat via OSC.";

        public void Init() { }

        public void Start()
        {
            _accessToken = GetTokenFromConfig();
            if (string.IsNullOrEmpty(_accessToken))
                return;
            _running = true;
            _worker = Task.Factory.StartNew(Run, TaskCreationOptions.LongRunning);
        }

        public void Stop()
        {
            _running = false;
        }

        private static string? GetTokenFromConfig()
        {
            try
            {
                if (!File.Exists("config.json"))
                    return null;
                var json = File.ReadAllText("config.json");
                var jObj = JObject.Parse(json);
                return jObj["SpotifyToken"]?.ToString();
            }
            catch
            {
                return null;
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
                        PacketSender.SendPacket(
                            new VRChatMessage("SpotifyTrack", track.Name),
                            new VRChatMessage("SpotifyArtist", track.Artist),
                            new VRChatMessage("SpotifyAlbum", track.Album)
                        );
                    }
                }
                catch { }
                await Task.Delay(3000);
            }
        }

        private static async Task<SpotifyTrack?> GetCurrentTrack()
        {
            if (string.IsNullOrEmpty(_accessToken))
                return null;
            var request = new HttpRequestMessage(HttpMethod.Get, "https://api.spotify.com/v1/me/player/currently-playing");
            request.Headers.Add("Authorization", $"Bearer {_accessToken}");
            var response = await _httpClient.SendAsync(request);
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
            return new SpotifyTrack(id, name, artist, album);
        }

        private record SpotifyTrack(string Id, string Name, string Artist, string Album);
    }
}
