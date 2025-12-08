using FitnessCenterProject.Data;
using FitnessCenterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HizmetController : Controller
    {
        private readonly AppDbContext _context;

        public HizmetController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ LISTELEME
        public IActionResult Index()
        {
            var hizmetler = _context.Hizmetler.ToList();
            return View(hizmetler);
        }

        // ✅ CREATE - GET
        public IActionResult Create()
        {

            return View();
        }

        // ✅ CREATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Hizmetler.Add(hizmet);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(hizmet);
        }

        // ✅ DETAILS
        public IActionResult Details(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
                return NotFound();

            return View(hizmet);
        }

        // ✅ UPDATE - GET
        public IActionResult Update(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
                return NotFound();

            return View(hizmet);
        }

        // ✅ UPDATE - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Hizmetler.Update(hizmet);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(hizmet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
                return NotFound();

            _context.Hizmetler.Remove(hizmet);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

    }
}
