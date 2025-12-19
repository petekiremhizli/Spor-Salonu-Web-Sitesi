using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace FitnessCenterProject.Services
{
    public class GoogleAiService
    {
        private readonly HttpClient _httpClient;

        // 🔑 KENDİ API KEY’İN
        private const string ApiKey = "AIzaSyC3aNpjzQOTct0_vT37Mjh-ra4NEHUORw4";

        private const string Endpoint =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key=" + ApiKey;

        public GoogleAiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> DiyetOnerisiAl(
            int boy,
            int kilo,
            int yas,
            string cinsiyet,
            string hedef)
        {
            var prompt = $@"
Boy: {boy} cm
Kilo: {kilo} kg
Yaş: {yas}
Cinsiyet: {cinsiyet}
Hedef: {hedef}

Bu bilgilere göre 1 günlük sağlıklı diyet önerisi ver.
";

            var requestBody = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                }
            };

            var jsonRequest = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(jsonRequest, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Endpoint, content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            // DEBUG için (ilk çalıştırmada çok faydalı)
            Console.WriteLine(jsonResponse);

            using var doc = JsonDocument.Parse(jsonResponse);

            // 🛡️ GÜVENLİ JSON OKUMA (ÇÖKMEZ)
            if (doc.RootElement.TryGetProperty("candidates", out var candidates) &&
                candidates.GetArrayLength() > 0)
            {
                var candidate = candidates[0];

                if (candidate.TryGetProperty("content", out var contentObj) &&
                    contentObj.TryGetProperty("parts", out var parts) &&
                    parts.GetArrayLength() > 0 &&
                    parts[0].TryGetProperty("text", out var text))
                {
                    return text.GetString() ?? "AI yanıtı boş döndü.";
                }
            }

            return "Şu anda diyet önerisi alınamadı. Lütfen daha sonra tekrar deneyin.";
        }
    }
}
