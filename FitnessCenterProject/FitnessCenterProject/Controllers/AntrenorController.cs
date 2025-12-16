using FitnessCenterProject.Data;
using FitnessCenterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AntrenorController : Controller
    {
        private readonly AppDbContext _context;

        public AntrenorController(AppDbContext context)
        {
            _context = context;
        }

        // ===================== INDEX =====================
        public IActionResult Index()
        {
            var antrenorler = _context.Antrenorler
                .Include(a => a.AntrenorHizmetler)
                .ThenInclude(ah => ah.Hizmet)
                .ToList();

            return View(antrenorler);
        }

        // ===================== CREATE (GET) =====================
        public IActionResult Create()
        {
            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            return View();
        }

        // ===================== CREATE (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Antrenor antrenor, List<int> seciliHizmetler)
        {
            if (seciliHizmetler == null || !seciliHizmetler.Any())
            {
                ModelState.AddModelError("", "En az bir uzmanlık alanı seçmelisiniz!");
            }

            if (ModelState.IsValid)
            {
                _context.Antrenorler.Add(antrenor);
                _context.SaveChanges(); // AntrenorId oluşur

                foreach (var hizmetId in seciliHizmetler)
                {
                    _context.AntrenorHizmetler.Add(new AntrenorHizmet
                    {
                        AntrenorId = antrenor.Id,
                        HizmetId = hizmetId
                    });
                }

                _context.SaveChanges();

                TempData["Success"] = "Antrenör başarıyla eklendi.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            return View(antrenor);
        }

        // ===================== EDIT (GET) =====================
        public IActionResult Edit(int id)
        {
            var antrenor = _context.Antrenorler
                .Include(a => a.AntrenorHizmetler)
                .FirstOrDefault(a => a.Id == id);

            if (antrenor == null)
                return NotFound();

            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            ViewBag.SeciliHizmetler = antrenor.AntrenorHizmetler
                .Select(ah => ah.HizmetId)
                .ToList();

            return View(antrenor);
        }

        // ===================== EDIT (POST) =====================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Antrenor antrenor, List<int> seciliHizmetler)
        {
            if (seciliHizmetler == null || !seciliHizmetler.Any())
            {
                ModelState.AddModelError("", "En az bir uzmanlık alanı seçmelisiniz!");
            }

            if (ModelState.IsValid)
            {
                _context.Antrenorler.Update(antrenor);

                // Eski uzmanlıkları sil
                var eskiKayitlar = _context.AntrenorHizmetler
                    .Where(x => x.AntrenorId == antrenor.Id);
                _context.AntrenorHizmetler.RemoveRange(eskiKayitlar);

                // Yeni uzmanlıkları ekle
                foreach (var hizmetId in seciliHizmetler)
                {
                    _context.AntrenorHizmetler.Add(new AntrenorHizmet
                    {
                        AntrenorId = antrenor.Id,
                        HizmetId = hizmetId
                    });
                }

                _context.SaveChanges();

                TempData["Success"] = "Antrenör başarıyla güncellendi.";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            ViewBag.SeciliHizmetler = seciliHizmetler;
            return View(antrenor);
        }

        // ===================== DELETE =====================
        public IActionResult Delete(int id)
        {
            var antrenor = _context.Antrenorler
                .Include(a => a.AntrenorHizmetler)
                .FirstOrDefault(a => a.Id == id);

            if (antrenor == null)
                return NotFound();

            // Önce ara tablo temizlenir
            _context.AntrenorHizmetler.RemoveRange(antrenor.AntrenorHizmetler);
            _context.Antrenorler.Remove(antrenor);
            _context.SaveChanges();

            TempData["Success"] = "Antrenör başarıyla silindi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
