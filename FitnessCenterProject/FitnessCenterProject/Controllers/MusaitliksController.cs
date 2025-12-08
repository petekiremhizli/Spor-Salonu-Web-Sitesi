using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FitnessCenterProject.Data;
using FitnessCenterProject.Models;

namespace FitnessCenterProject.Controllers
{
    public class MusaitliksController : Controller
    {
        private readonly AppDbContext _context;

        public MusaitliksController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Musaitliks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Musaitlikler.Include(m => m.Antrenor);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Musaitliks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musaitlik = await _context.Musaitlikler
                .Include(m => m.Antrenor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (musaitlik == null)
            {
                return NotFound();
            }

            return View(musaitlik);
        }

        // GET: Musaitliks/Create
        public IActionResult Create()
        {
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "Id", "AdSoyad");
            return View();
        }

        // POST: Musaitliks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Gun,BaslangicSaati,BitisSaati,AntrenorId")] Musaitlik musaitlik)
        {
            if (ModelState.IsValid)
            {
                _context.Add(musaitlik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "Id", "AdSoyad", musaitlik.AntrenorId);
            return View(musaitlik);
        }

        // GET: Musaitliks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musaitlik = await _context.Musaitlikler.FindAsync(id);
            if (musaitlik == null)
            {
                return NotFound();
            }
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "Id", "AdSoyad", musaitlik.AntrenorId);
            return View(musaitlik);
        }

        // POST: Musaitliks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Gun,BaslangicSaati,BitisSaati,AntrenorId")] Musaitlik musaitlik)
        {
            if (id != musaitlik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(musaitlik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MusaitlikExists(musaitlik.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AntrenorId"] = new SelectList(_context.Antrenorler, "Id", "AdSoyad", musaitlik.AntrenorId);
            return View(musaitlik);
        }

        // GET: Musaitliks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var musaitlik = await _context.Musaitlikler
                .Include(m => m.Antrenor)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (musaitlik == null)
            {
                return NotFound();
            }

            return View(musaitlik);
        }

        // POST: Musaitliks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var musaitlik = await _context.Musaitlikler.FindAsync(id);
            if (musaitlik != null)
            {
                _context.Musaitlikler.Remove(musaitlik);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MusaitlikExists(int id)
        {
            return _context.Musaitlikler.Any(e => e.Id == id);
        }
    }
}
