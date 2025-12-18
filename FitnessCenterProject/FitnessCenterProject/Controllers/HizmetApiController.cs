using FitnessCenterProject.Data;
using FitnessCenterProject.Models.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessCenterProject.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class HizmetApiController : ControllerBase
    {
        private readonly AppDbContext _context;

        public HizmetApiController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<HizmetApiModel>>> GetHizmetler()
        {
            var hizmetler = await _context.Hizmetler
                .Select(h => new HizmetApiModel
                {
                    Id = h.Id,
                    Ad = h.Ad,
                    Sure = h.Sure,
                    Ucret = h.Ucret
                })
                .ToListAsync();

            return Ok(hizmetler);
        }
    }
}
