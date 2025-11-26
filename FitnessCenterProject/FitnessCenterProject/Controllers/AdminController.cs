using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterProject.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Antrenorler()
        {
            return View();
        }
        public IActionResult Hizmetler()
        {
            return View();
        }
        public IActionResult Salonlar()
        {
            return View();
        }
        public IActionResult Randevular()
        {
            return View();
        }
    }
}
