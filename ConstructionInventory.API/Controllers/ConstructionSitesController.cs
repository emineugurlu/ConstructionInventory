using ConstructionInventory.Data;
using ConstructionInventory.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace ConstructionInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConstructionSitesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ConstructionSitesController(AppDbContext context)
        {
            _context = context;
        }

        //şantiye listeleme
        [HttpGet]

        public async Task<IActionResult> GetAll() => Ok(await _context.ConstructionSites.ToListAsync());


        //şantiye ekleme

        [HttpPost]

        public async Task<IActionResult> Create(ConstructionSite site)
        {
            _context.ConstructionSites.Add(site);
            await _context.SaveChangesAsync();
            return Ok(site);

        }
    }
}
