using ConstructionInventory.Data;
using ConstructionInventory.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ConstructionInventory.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockMovementsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public StockMovementsController(AppDbContext context)
        {
            _context = context;
        }

        //tüm hareketleri listeleme
        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _context.StockMovements.ToListAsync());

        //hareket ekleme

        [HttpPost]
        public async Task<IActionResult> Create(StockMovement movement)
        {
           //malzemeyi veri tabanında bul
           var material = await _context.Materials.FindAsync(movement.MaterialId);
              if (material == null)
              {
                return NotFound("Malzeme bulunamadı.");
              }
            //hareket tipine göre stoğu güncelle
            if (movement.MovementType == 0) // giriş enuma göre takip et
            {
                material.StockCount += movement.Quantity;
            }
            else
            { 
                material.StockCount -= movement.Quantity;
            }

            //hareketi ve güncellenen stok miktarını kaydet
            movement.MovementDate = DateTime.Now;
            _context.StockMovements.Add(movement);

            await _context.SaveChangesAsync();

            return Ok(new { Message = "Stok güncellendi ve hareket kaydedildi", CurrentStock = material.StockCount });
        }
        //sadece bir şantiyeyi getirir bu üsteki get ise tüm listeyi getirir
        [HttpGet("site/{siteId}")] // burda arayacak bulacak ondan böyle yazdık
        public async Task<IActionResult> GetBySite(int siteId)
        {
            var movements = await _context.StockMovements
                .Where(sm => sm.ConstructionSiteId == siteId)
                .ToListAsync();
            if (movements.Count == 0) return NotFound("Bu şantiyeye ait henüz bir hareket yok.");

            return Ok(movements);
        }

    }
}
