using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FitnessCenterProject.Controllers
{

    [Authorize(Roles = "Uye")]
    public class UyeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
