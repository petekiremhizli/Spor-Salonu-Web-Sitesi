using FitnessCenterProject.Data;
using FitnessCenterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessCenterProject.Controllers
{
    [Authorize(Roles = "Uye")]
    public class RandevuController : Controller
    {
        private readonly AppDbContext _context;

        public RandevuController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX
        // =========================
        public IActionResult Index()
        {
            var uyeId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var randevular = _context.Randevular
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .Where(r => r.UyeId == uyeId)
                .OrderByDescending(r => r.BaslangicZamani)
                .ToList();

            return View(randevular);
        }

        // =========================
        // DETAILS
        // =========================
        public IActionResult Details(int id)
        {
            var randevu = _context.Randevular
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .FirstOrDefault(r => r.Id == id);

            if (randevu == null) return NotFound();
            return View(randevu);
        }

        // =========================
        // CREATE - GET
        // =========================
        public IActionResult Create()
        {
            ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "Id", "Ad");
            ViewBag.Antrenorler = new SelectList(new List<Antrenor>(), "Id", "AdSoyad");
            return View();
        }

        // =========================
        // CREATE - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Randevu model)
        {
            model.UyeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            model.Durum = "Beklemede";

            var hizmet = _context.Hizmetler.FirstOrDefault(h => h.Id == model.HizmetId);
            if (hizmet != null)
                model.BitisZamani = model.BaslangicZamani.AddMinutes(hizmet.Sure);

            if (!ModelState.IsValid)
            {
                ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "Id", "Ad");
                ViewBag.Antrenorler = new SelectList(new List<Antrenor>(), "Id", "AdSoyad");
                return View(model);
            }

            _context.Randevular.Add(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // EDIT - GET
        // =========================
        public IActionResult Edit(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu == null) return NotFound();

            // Tüm hizmetleri gönder
            ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "Id", "Ad", randevu.HizmetId);

            // Mevcut hizmete uygun antrenörleri gönder
            ViewBag.Antrenorler = new SelectList(
                _context.Antrenorler.Where(a => a.AntrenorHizmetler.Any(h => h.HizmetId == randevu.HizmetId)).ToList(),
                "Id", "AdSoyad", randevu.AntrenorId);

            return View(randevu);
        }


        // =========================
        // EDIT - POST
        // =========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Randevu model)
        {
            var randevu = _context.Randevular.Find(model.Id);
            if (randevu == null) return NotFound();

            randevu.HizmetId = model.HizmetId;
            randevu.AntrenorId = model.AntrenorId;
            randevu.BaslangicZamani = model.BaslangicZamani;

            var hizmet = _context.Hizmetler.FirstOrDefault(h => h.Id == model.HizmetId);
            if (hizmet != null)
                randevu.BitisZamani = model.BaslangicZamani.AddMinutes(hizmet.Sure);

            if (!ModelState.IsValid)
            {
                ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "Id", "Ad", randevu.HizmetId);
                ViewBag.Antrenorler = new SelectList(
                    _context.Antrenorler.Where(a => a.AntrenorHizmetler.Any(h => h.HizmetId == randevu.HizmetId)).ToList(),
                    "Id", "AdSoyad", randevu.AntrenorId);
                return View(model);
            }

            _context.Randevular.Update(randevu);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // DELETE - GET
        // =========================
        public IActionResult Delete(int id)
        {
            var randevu = _context.Randevular
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet)
                .FirstOrDefault(r => r.Id == id);

            if (randevu == null) return NotFound();
            return View(randevu);
        }

        // =========================
        // DELETE - POST
        // =========================
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu == null) return NotFound();

            _context.Randevular.Remove(randevu);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // =========================
        // AJAX: Antrenörleri Getir
        // =========================
        [HttpGet]
        public IActionResult GetAntrenorlerByHizmet(int hizmetId)
        {
            var antrenorler = _context.Antrenorler
                .Where(a => a.AntrenorHizmetler.Any(h => h.HizmetId == hizmetId))
                .Select(a => new { id = a.Id, adSoyad = a.AdSoyad })
                .ToList();

            return Json(antrenorler);
        }

        // =========================
        // AJAX: Müsait Günler
        // =========================
        [HttpGet]
        public IActionResult GetMusaitGunler(int antrenorId)
        {
            var gunler = _context.Musaitlikler
                .Where(m => m.AntrenorId == antrenorId)
                .Select(m => m.Tarih)
                .Distinct()
                .ToList();

            return Json(gunler);
        }

        // =========================
        // AJAX: Müsait Saatler
        // =========================
        [HttpGet]
        public IActionResult GetMusaitSaatler(int antrenorId, DateTime tarih, int hizmetId)
        {
            var hizmet = _context.Hizmetler.FirstOrDefault(h => h.Id == hizmetId);
            if (hizmet == null) return Json(new List<string>());

            var musaitlikler = _context.Musaitlikler
                .Where(m => m.AntrenorId == antrenorId && m.Tarih == tarih)
                .ToList();

            var doluRandevular = _context.Randevular
                .Where(r => r.AntrenorId == antrenorId &&
                            r.Durum != "Iptal" &&
                            r.BaslangicZamani.Date == tarih)
                .ToList();

            var saatler = new List<string>();
            foreach (var m in musaitlikler)
            {
                DateTime zaman = tarih.Date + m.BaslangicSaati;
                while (zaman.TimeOfDay.Add(TimeSpan.FromMinutes(hizmet.Sure)) <= m.BitisSaati)
                {
                    bool dolu = doluRandevular.Any(r =>
                        zaman < r.BitisZamani &&
                        zaman.AddMinutes(hizmet.Sure) > r.BaslangicZamani
                    );

                    if (!dolu)
                        saatler.Add(zaman.ToString("HH:mm"));

                    zaman = zaman.AddMinutes(15);
                }
            }

            return Json(saatler);
        }
    }
}
