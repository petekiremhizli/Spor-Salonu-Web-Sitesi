using FitnessCenterProject.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FitnessCenterProject.Controllers
{
    public class DiyetController : Controller
    {
        private readonly GeminiService _geminiService;

        public DiyetController(GeminiService geminiService)
        {
            _geminiService = geminiService;
        }

        // GET: Form sayfası
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // POST: Diyet önerisi al
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(int yas, int boy, int kilo, string hedef)
        {
            if (boy <= 0 || kilo <= 0 || yas <= 0 || string.IsNullOrEmpty(hedef))
            {
                ViewBag.Sonuc = "Lütfen tüm alanları doğru şekilde doldurun.";
                return View();
            }

            try
            {
                var sonuc = await _geminiService.DiyetOnerisiAl(yas, boy, kilo, hedef);
                ViewBag.Sonuc = sonuc;
            }
            catch (HttpRequestException ex)
            {
                ViewBag.Sonuc = $"API hatası: {ex.Message}";
            }

            return View();
        }
    }
}
