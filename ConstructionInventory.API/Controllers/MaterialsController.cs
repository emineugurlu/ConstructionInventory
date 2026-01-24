using ConstructionInventory.Data;
using ConstructionInventory.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConstructionInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public MaterialsController(AppDbContext context)
        {
           _context = context;
        }
        //malzeme listeleme
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var materials = await _context.Materials.ToListAsync();
            return Ok(materials);
        }

        //malzeme ekleme
        [HttpPost]
        public async Task<IActionResult> Create(Material material)
        {
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
            return Ok(material);

        }

    }
}
