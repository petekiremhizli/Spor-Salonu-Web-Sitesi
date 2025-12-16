// ⭐️ 1. Gerekli Namespace'ler
using Microsoft.Extensions.Configuration; // IConfiguration için gerekli
using OpenAI; // Client sınıfı için gerekli (CS0246 hatasını çözer)
using OpenAI.Chat; // ChatCompletionRequest, ChatMessage, Role için gerekli (CS0246, CS0103 hatalarını çözer)
using System;
using System.Linq;
using System.Net.Http; // Eğer OpenAI kütüphanesi HttpClient gerektiriyorsa güvenlik için eklenebilir
using System.Threading.Tasks;

namespace FitnessCenterProject.Services
{
    public class AIService
    {
        // OpenAI kütüphanesinden gelen Client sınıfı
        private readonly OpenAIClient _openAIClient;

        public AIService(IConfiguration configuration)
        {
            string apiKey = configuration["OpenAI:ApiKey"]
                            ?? throw new InvalidOperationException("OpenAI API Anahtarı bulunamadı.");

            // Client sınıfını başlatıyoruz
            _openAIClient = new OpenAIClient(apiKey);
        }

        public async Task<string> GetFitnessPlanAsync(string bodyType, double height, double weight)
        {
            // ... (Prompt içeriği)

            var chatRequest = new ChatCompletionRequest(
                messages: new[]
                {
                    // ⭐️ Role.System ve Role.User kullanımı CS0103 ve CS1729 hatalarını çözer
                    new ChatMessage(Role.System, "Sen, kişiselleştirilmiş fitness ve beslenme planları oluşturan profesyonel bir yapay zekâ asistanısın."),
                    new ChatMessage(Role.User, prompt)
                },
                Model: "gpt-3.5-turbo"
            );

            try
            {
                var response = await _openAIClient.Chat.CreateCompletionAsync(chatRequest);

                return response.Choices.FirstOrDefault()?.Message.Content ?? "Üzgünüz, plan oluşturulamadı.";
            }
            catch (Exception ex)
            {
                return $"API Çağrısı Başarısız: {ex.Message}";
            }
        }
    }
}