using System.Diagnostics;
using FitnessCenterProject.Models; // FitnessInputViewModel için gerekli
using FitnessCenterProject.Services; // AIService için gerekli
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks; // Asenkron metodlar (async/await) için gerekli

namespace FitnessCenterProject.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AIService _aiService; // ⭐️ AIService değişkeni eklendi

        // Constructor: ILogger ve AIService bağımlılıkları alınıyor
        public HomeController(ILogger<HomeController> logger, AIService aiService)
        {
            _logger = logger;
            _aiService = aiService; // ⭐️ AIService tanımlandı
        }

        // Index aksiyonu güncellendi: ViewModel'i beklemelidir (Boş veya dolu)
        // Eğer View'ınızda @model FitnessInputViewModel tanımlıysa bu gereklidir.
        [HttpGet]
        public IActionResult Index()
        {
            // İlk açılışta veya Get isteğinde, boş bir model gönderiyoruz.
            return View(new YapayZekaEntegrasyonu());
        }

        // ⭐️ Yapay Zeka Planı Oluşturma Aksiyonu (Index.cshtml'deki form bu aksiyonu hedefler)
        [HttpPost]
        public async Task<IActionResult> GetPlan(YapayZekaEntegrasyonu model)
        {
            // Model validasyonunu kontrol et
            if (ModelState.IsValid)
            {
                _logger.LogInformation("AI plan isteği alındı: Hedef={BodyType}, Boy={Height}, Kilo={Weight}",
                 model.BodyType, model.Height, model.Weight);

                try
                {
                    // AI servisini çağır
                    string plan = await _aiService.GetFitnessPlanAsync(
                        model.BodyType,
                        model.Height,
                        model.Weight
                    );
                    model.AIResponse = plan;
                }
                catch (Exception ex)
                {
                    // API çağrısı sırasında oluşabilecek hataları yönet
                    _logger.LogError(ex, "OpenAI API çağrısı sırasında hata oluştu.");
                    model.AIResponse = "Üzgünüz, yapay zeka servisi şu an yanıt veremiyor. Lütfen daha sonra tekrar deneyin.";
                }
            }
            else
            {
                // Eğer form validasyon (required, range) hatası varsa logla
                _logger.LogWarning("AI plan isteği, geçersiz veya eksik model verileri nedeniyle reddedildi.");
            }

            // Sonuçla (AIResponse dolu veya boş) Index View'ını geri döndür
            return View("Index", model);
        }

        // Mevcut diğer aksiyonlarınız değişmeden kalır
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Services()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}