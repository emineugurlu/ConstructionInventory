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
            movement.MovementDate = DateTime.Now;
            _context.StockMovements.Add(movement);
            await _context.SaveChangesAsync();
            return Ok(movement);
        }

    }
}
