using ConstructionInventory.Domain.Entities;
using ConstructionInventory.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConstructionInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ConstructionSitesController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ConstructionSitesController(AppDbContext context) { _context = context; }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.ConstructionSites.ToListAsync());

        [HttpPost]
        public async Task<IActionResult> Create(ConstructionSite site)
        {
            _context.ConstructionSites.Add(site);
            await _context.SaveChangesAsync();
            return Ok(site);
        }
    }
}