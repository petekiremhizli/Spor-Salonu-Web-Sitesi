using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterProject.Controllers
{

    [Authorize(Roles="Admin")]
    public class AdminnController : Controller
    {


        public IActionResult Index()
        {
            return View();
        }
        
    }
}
