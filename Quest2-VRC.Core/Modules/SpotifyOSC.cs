using System;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using static Quest2_VRC.PacketSender;

namespace Quest2_VRC
{
    public class SpotifyOSC
    {
        private static string? _accessToken;
        private static readonly HttpClient _httpClient = new HttpClient();
        private static string _lastTrackId = string.Empty;

        // Запуск модуля Spotify OSC
        public static void StartSpotifyOSC()
        {
            _accessToken = GetTokenFromConfig();
            if (string.IsNullOrEmpty(_accessToken))
            {
                // Можно добавить логирование ошибки
                return;
            }
            // Удалена подписка на Receiver.SpotifyControlCommandReceived
            Task.Factory.StartNew(Run, TaskCreationOptions.LongRunning);
        }


        // Получение токена Spotify из config.json
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

        // Основной цикл получения трека и отправки OSC
        private static async void Run()
        {
            while (true)
            {
                try
                {
                    var track = await GetCurrentTrack();
                    if (track != null && track.Id != _lastTrackId)
                    {
                        _lastTrackId = track.Id;
                        // Отправка данных в VRChat через OSC
                        SendPacket(
                            new VRChatMessage("SpotifyTrack", track.Name),
                            new VRChatMessage("SpotifyArtist", track.Artist),
                            new VRChatMessage("SpotifyAlbum", track.Album)
                        );
                    }
                }
                catch (Exception ex)
                {
                    // Можно добавить логирование
                }
                await Task.Delay(3000); // Проверять раз в 3 секунды
            }
        }

        // Получение текущего трека Spotify через Web API
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
