using FitnessCenterProject.Data;
using FitnessCenterProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterProject.Controllers.Admin
{
    [Authorize(Roles = "Admin")]
    public class RandevuOnayController : Controller
    {
        private readonly AppDbContext _context;

        public RandevuOnayController(AppDbContext context)
        {
            _context = context;
        }

        // =========================
        // INDEX: Beklemedeki randevular
        // =========================
        public IActionResult Index(string filter = "Beklemede")
        {
            // filter parametresi: Beklemede, Onaylandi, Iptal veya Hepsi
            IQueryable<Randevu> randevular = _context.Randevular
                .Include(r => r.Uye)
                .Include(r => r.Antrenor)
                .Include(r => r.Hizmet);

            if (filter != "Hepsi")
            {
                randevular = randevular.Where(r => r.Durum == filter);
            }

            var model = randevular
                .OrderBy(r => r.BaslangicZamani)
                .ToList();

            ViewBag.Filter = filter;

            return View(model);
        }


        // =========================
        // Randevu Onayla
        // =========================
        public IActionResult Onayla(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu == null) return NotFound();

            randevu.Durum = "Onaylandi";
            _context.SaveChanges();

            TempData["Success"] = "Randevu başarıyla onaylandı.";
            return RedirectToAction(nameof(Index));
        }

        // =========================
        // Randevu İptal
        // =========================
        public IActionResult IptalEt(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu == null) return NotFound();

            randevu.Durum = "Iptal";
            _context.SaveChanges();

            TempData["Success"] = "Randevu iptal edildi.";
            return RedirectToAction(nameof(Index));
        }
    }
}
