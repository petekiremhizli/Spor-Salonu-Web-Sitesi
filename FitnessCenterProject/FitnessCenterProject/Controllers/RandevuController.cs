using FitnessCenterProject.Data;
using FitnessCenterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterProject.Controllers
{
    [Authorize(Roles = "Uye")]
    public class RandevuController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Uye> _userManager;

        public RandevuController(AppDbContext context, UserManager<Uye> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // ================== INDEX ==================
        // Üyenin kendi randevuları
        public IActionResult Index()
        {
            var uyeId = _userManager.GetUserId(User);

            var randevular = _context.Randevular
                .Include(r => r.Hizmet)
                .Include(r => r.Antrenor)
                .Where(r => r.UyeId == uyeId)
                .OrderByDescending(r => r.BaslangicZamani)
                .ToList();

            return View(randevular);
        }

        // ================== CREATE (GET) ==================
        public IActionResult Create()
        {
            ViewBag.Hizmetler = _context.Hizmetler.ToList();
            ViewBag.Antrenorler = new List<Antrenor>(); // boş başlasın

            return View();
        }
        [HttpGet]
        public IActionResult GetAntrenorlerByHizmet(int hizmetId)
        {
            var antrenorler = _context.AntrenorHizmetler
                .Where(x => x.HizmetId == hizmetId)
                .Select(x => new
                {
                    x.Antrenor.Id,
                    x.Antrenor.AdSoyad
                })
                .Distinct()
                .ToList();

            return Json(antrenorler);
        }


        // ================== CREATE (POST) ==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Randevu randevu)
        {
            // 🔥 FORMDA OLMAYAN ALANLARI VALIDATION'DAN ÇIKAR
            ModelState.Remove("UyeId");
            ModelState.Remove("Durum");
            ModelState.Remove("Uye");
            ModelState.Remove("Antrenor");
            ModelState.Remove("Hizmet");

            if (!ModelState.IsValid)
            {
                ViewBag.Hizmetler = _context.Hizmetler.ToList();
                return View(randevu);
            }

            // ✅ BACKEND SET
            randevu.UyeId = _userManager.GetUserId(User);
            randevu.Durum = "Beklemede";

            var hizmet = _context.Hizmetler.Find(randevu.HizmetId);
            randevu.BitisZamani =
                randevu.BaslangicZamani.AddMinutes(hizmet.Sure);

            _context.Randevular.Add(randevu);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }



        // ================== EDIT (GET) ==================
        public IActionResult Edit(int id)
        {
            var uyeId = _userManager.GetUserId(User);

            var randevu = _context.Randevular
                .FirstOrDefault(r => r.Id == id && r.UyeId == uyeId);

            if (randevu == null)
                return NotFound();

            ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "Id", "Ad", randevu.HizmetId);
            ViewBag.Antrenorler = new SelectList(_context.Antrenorler, "Id", "AdSoyad", randevu.AntrenorId);

            return View(randevu);
        }

        // ================== EDIT (POST) ==================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Randevu randevu)
        {
            var uyeId = _userManager.GetUserId(User);

            if (randevu.UyeId != uyeId)
                return Unauthorized();

            if (!ModelState.IsValid)
            {
                ViewBag.Hizmetler = new SelectList(_context.Hizmetler, "Id", "Ad", randevu.HizmetId);
                ViewBag.Antrenorler = new SelectList(_context.Antrenorler, "Id", "AdSoyad", randevu.AntrenorId);
                return View(randevu);
            }

            var hizmet = _context.Hizmetler.Find(randevu.HizmetId);
            if (hizmet != null)
            {
                randevu.BitisZamani = randevu.BaslangicZamani.AddMinutes(hizmet.Sure);
            }

            _context.Randevular.Update(randevu);
            _context.SaveChanges();

            TempData["Success"] = "Randevu güncellendi.";

            return RedirectToAction(nameof(Index));
        }

        // ================== DELETE ==================
        public IActionResult Delete(int id)
        {
            var uyeId = _userManager.GetUserId(User);

            var randevu = _context.Randevular
                .Include(r => r.Hizmet)
                .Include(r => r.Antrenor)
                .FirstOrDefault(r => r.Id == id && r.UyeId == uyeId);

            if (randevu == null)
                return NotFound();

            return View(randevu);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var uyeId = _userManager.GetUserId(User);

            var randevu = _context.Randevular
                .FirstOrDefault(r => r.Id == id && r.UyeId == uyeId);

            if (randevu == null)
                return NotFound();

            _context.Randevular.Remove(randevu);
            _context.SaveChanges();

            TempData["Success"] = "Randevu silindi.";

            return RedirectToAction(nameof(Index));
        }
    }
}
