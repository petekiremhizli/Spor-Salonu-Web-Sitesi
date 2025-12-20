using FitnessCenterProject.Data;
using FitnessCenterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        // LISTELEME
        public IActionResult Index()
        {
            var hizmetler = _context.Hizmetler.ToList();
            return View(hizmetler);
        }

        // CREATE - GET
        public IActionResult Create()
        {

            return View();
        }

        // CREATE - POST
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

        // DETAILS
        public IActionResult Details(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
                return NotFound();

            return View(hizmet);
        }

        // UPDATE - GET
        public IActionResult Update(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
                return NotFound();

            return View(hizmet);
        }

        // UPDATE - POST
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
        // GET: Hizmet/Delete/5
        public IActionResult Delete(int id)
        {
            var hizmet = _context.Hizmetler
                .Include(h => h.AntrenorHizmetler)
                .FirstOrDefault(h => h.Id == id);

            if (hizmet == null)
                return NotFound();

            //  Bu hizmet kullanılıyor mu?
            if (hizmet.AntrenorHizmetler.Any())
            {
                TempData["Error"] =
                    "Bu hizmete sahip antrenör bulunmaktadır. Önce antrenörü siliniz.";
                return RedirectToAction(nameof(Index));
            }

            // Kullanılmıyorsa onay sayfası
            return View(hizmet);
        }


        //POST
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet == null)
                return NotFound();

            _context.Hizmetler.Remove(hizmet);
            _context.SaveChanges();

            TempData["Success"] = "Hizmet başarıyla silindi.";
            return RedirectToAction("Index", "Hizmet");

        }




    }
}
