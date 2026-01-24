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
            var materials = await _context.Materials
            .Where(x => !x.IsDeleted)
            .ToListAsync();
            return Ok(materials);
        }

        //malzeme ekleme
        [HttpPost]
        public async Task<IActionResult> Create(Material material)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState); //kurallara uyulmazsa hata atıcak kullanıcıya
            }

            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
            return Ok(material);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Material updatedMaterial)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound("Malzeme bulunamadı.");
            }
            material.Name = updatedMaterial.Name;
            material.MinStockLimit = updatedMaterial.MinStockLimit;
            material.Unit = updatedMaterial.Unit;
            await _context.SaveChangesAsync();
            return Ok("Malzeme başarıyla güncellendi.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null)
            {
                return NotFound("Malzeme bulunamadı.");
            }
            material.IsDeleted = true;
            await _context.SaveChangesAsync();
            return Ok($"{material.Name} başarıyla silindi.");
        }

        //az kalanları uyarıcak stok takibi gibi yani
        [HttpGet("critical-stock")]
        public async Task<IActionResult> GetCriticalStock()
        {
            var criticalMaterials = await _context.Materials
                .Where(x => x.StockCount < x.MinStockLimit)
                .ToListAsync();

            return Ok(criticalMaterials);
        }
        //silinen olursa kurtarmak için yazdım bunu
        [HttpPost("restore/{id}")]
        public async Task<IActionResult> Restore(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null) return NotFound();

            material.IsDeleted = false; //silindi diye o damgayı kaldırır

            await _context.SaveChangesAsync();
            return Ok($"{material.Name} malzemesi başarıyla geri yüklendi.");


        }
    }
}
