using FitnessCenterProject.Data;
using Microsoft.AspNetCore.Mvc;

public class MusaitlikController : Controller
{
    private readonly AppDbContext _context;

    public MusaitlikController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Create()
    {
        ViewBag.Antrenorlar = _context.Antrenorler.ToList();
        return View();
    }
}
