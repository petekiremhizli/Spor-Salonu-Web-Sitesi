using System.Text;
using System.Text.Json;

namespace FitnessCenterProject.Services
{
    public class GeminiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public GeminiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["GeminiSettings:ApiKey"];
        }

        public async Task<string> DiyetOnerisiAl(int yas, int boy, int kilo, string hedef)
        {
            string modelName = "gemini-flash-latest"; // Model adı güncel

            string prompt = $@"
            Sen profesyonel bir diyetisyensin.
            Kullanıcı bilgileri:
            Yaş: {yas}
            Boy: {boy} cm
            Kilo: {kilo} kg
            Hedef: {hedef}

            Bu bilgilere göre günlük kalori öner, 1 günlük örnek diyet listesi ve kısa tavsiyeler ver.
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

            string jsonBody = JsonSerializer.Serialize(requestBody);

            var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://generativelanguage.googleapis.com/v1beta/models/{modelName}:generateContent?key={_apiKey}")
            {
                Content = new StringContent(jsonBody, Encoding.UTF8, "application/json")
            };

            // Google API anahtarı header olarak da gönderilebilir
            request.Headers.Add("x-goog-api-key", _apiKey);

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            string responseJson = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(responseJson);
            var result = doc.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return result ?? "Diyet önerisi alınamadı.";
        }
    }
}
