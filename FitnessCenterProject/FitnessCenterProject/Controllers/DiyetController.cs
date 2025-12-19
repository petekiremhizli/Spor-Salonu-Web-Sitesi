using FitnessCenterProject.Services;
using Microsoft.AspNetCore.Mvc;

public class DiyetController : Controller
{
    private readonly GoogleAiService _aiService;

    public DiyetController(GoogleAiService aiService)
    {
        _aiService = aiService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Oneri(
        int boy,
        int kilo,
        int yas,
        string cinsiyet,
        string hedef)
    {
        var sonuc = await _aiService.DiyetOnerisiAl(
            boy, kilo, yas, cinsiyet, hedef);

        ViewBag.AIResult = sonuc;
        return View("Index");
    }
}
