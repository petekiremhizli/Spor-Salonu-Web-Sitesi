using FitnessCenterProject.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterProject.Controllers
{

    [Authorize(Roles = "Uye")]
    public class UyeController : Controller
    {
        private readonly AppDbContext _context;

        public UyeController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Hizmetler()
        {
            var hizmetler = _context.Hizmetler
                .Include(h => h.AntrenorHizmetler)
                .ThenInclude(ah => ah.Antrenor)
                .ToList();

            return View(hizmetler);
        }


    }
}
