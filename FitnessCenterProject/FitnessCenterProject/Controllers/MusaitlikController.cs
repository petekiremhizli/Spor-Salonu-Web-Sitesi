using FitnessCenterProject.Data;
using FitnessCenterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class MusaitlikController : Controller
    {
        private readonly AppDbContext _context;

        public MusaitlikController(AppDbContext context)
        {
            _context = context;
        }

        // ===================== INDEX =====================
        public IActionResult Index()
        {
            var musaitlikler = _context.Musaitlikler
                .Include(m => m.Antrenor)
                    .ThenInclude(a => a.AntrenorHizmetler)
                        .ThenInclude(ah => ah.Hizmet)
                .OrderBy(m => m.Tarih)
                .ThenBy(m => m.BaslangicSaati)
                .ToList();

            return View(musaitlikler);
        }



        // ===================== CREATE - GET =====================
        public IActionResult Create()
        {
            ViewBag.Antrenorler = new SelectList(
                _context.Antrenorler,
                "Id",
                "AdSoyad"
            );

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Musaitlik musaitlik)

        {
            Console.WriteLine("AntrenorId: " + musaitlik.AntrenorId);

            ModelState.Remove("Antrenor"); 

            if (!ModelState.IsValid)
            {
                ViewBag.Antrenorler = new SelectList(_context.Antrenorler, "Id", "AdSoyad");
                return View(musaitlik);
            }

            _context.Musaitlikler.Add(musaitlik);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }




        // ===================== EDIT - GET =====================
        public IActionResult Edit(int id)
        {
            var musaitlik = _context.Musaitlikler.Find(id);
            if (musaitlik == null)
                return NotFound();

            ViewBag.Antrenorler =
                new SelectList(_context.Antrenorler, "Id", "AdSoyad", musaitlik.AntrenorId);

            return View(musaitlik);
        }

        // ===================== EDIT - POST =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Musaitlik model)
        {
            var musaitlik = _context.Musaitlikler.Find(id);
            if (musaitlik == null)
                return NotFound();

            bool cakismaVarMi = _context.Musaitlikler.Any(m =>
                m.Id != id &&
                m.AntrenorId == model.AntrenorId &&
                m.Tarih.Date == model.Tarih.Date &&
                model.BaslangicSaati < m.BitisSaati &&
                model.BitisSaati > m.BaslangicSaati
            );

            if (cakismaVarMi)
            {
                ModelState.AddModelError("",
                    "Bu saat aralığında başka bir müsaitlik bulunmaktadır.");
            }

            if (ModelState.IsValid)
            {
                musaitlik.AntrenorId = model.AntrenorId;
                musaitlik.Tarih = model.Tarih;
                musaitlik.BaslangicSaati = model.BaslangicSaati;
                musaitlik.BitisSaati = model.BitisSaati;

                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Antrenorler =
                new SelectList(_context.Antrenorler, "Id", "AdSoyad", model.AntrenorId);

            return View(model);
        }

        // ===================== DELETE - GET =====================
        public IActionResult Delete(int id)
        {
            var musaitlik = _context.Musaitlikler
                .Include(m => m.Antrenor)
                .FirstOrDefault(m => m.Id == id);

            if (musaitlik == null)
                return NotFound();

            return View(musaitlik);
        }

        // ===================== DELETE - POST =====================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var musaitlik = _context.Musaitlikler.Find(id);
            if (musaitlik == null)
                return NotFound();

            _context.Musaitlikler.Remove(musaitlik);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        // ===================== DETAILS =====================
        public IActionResult Details(int id)
        {
            var musaitlik = _context.Musaitlikler
                .Include(m => m.Antrenor)
                    .ThenInclude(a => a.AntrenorHizmetler)
                        .ThenInclude(ah => ah.Hizmet)
                .FirstOrDefault(m => m.Id == id);

            if (musaitlik == null)
                return NotFound();

            return View(musaitlik);
        }

    }
}
