
using ConstructionInventory.Domain.Services;
using ConstructionInventory.Domain.Entities;
using ConstructionInventory.Domain.DTOs;
using ConstructionInventory.Domain.Enums;
using ConstructionInventory.Data;
using Microsoft.EntityFrameworkCore;

namespace ConstructionInventory.API.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly AppDbContext _context;
        public MaterialService(AppDbContext context) { _context = context; }

        public async Task<List<Material>> GetAllActiveAsync() => await _context.Materials.Where(x => !x.IsDeleted).ToListAsync();

        public async Task CreateAsync(Material material)
        {
            _context.Materials.Add(material);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, Material updatedMaterial)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null) return false;
            material.Name = updatedMaterial.Name;
            material.MinStockLimit = updatedMaterial.MinStockLimit;
            material.Unit = updatedMaterial.Unit;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null) return false;
            material.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RestoreAsync(int id)
        {
            var material = await _context.Materials.FindAsync(id);
            if (material == null) return false;
            material.IsDeleted = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Material>> GetCriticalStockAsync() => await _context.Materials.Where(x => !x.IsDeleted && x.StockCount < x.MinStockLimit).ToListAsync();

        public async Task<List<Material>> GetArchivedAsync() => await _context.Materials.Where(x => x.IsDeleted).ToListAsync();

        public async Task<StockDashboardDto> GetDashboardStatsAsync()
        {
            var all = await _context.Materials.ToListAsync();
            return new StockDashboardDto
            {
                TotalMaterialCount = all.Count(x => !x.IsDeleted),
                CriticalStockCount = all.Count(x => !x.IsDeleted && x.StockCount < x.MinStockLimit),
                ArchivedMaterialCount = all.Count(x => x.IsDeleted),
                TotalProductQuantity = all.Where(x => !x.IsDeleted).Sum(x => (long)x.StockCount)
            };
        }

        public async Task<bool> MoveStockAsync(int materialId, int siteId, decimal quantity, MovementType type )
        {
            var m = await _context.Materials.FindAsync(materialId);
            if (m == null || m.IsDeleted) return false; 

            
            if (type == MovementType.Exit || type == MovementType.Waste)
            {
                if (m.StockCount < quantity) return false;
                m.StockCount -= (int)quantity;
            }
            else
            {
                m.StockCount += (int)quantity;
            }

            var movement = new StockMovement
            {
                MaterialId = materialId,
                ConstructionSiteId = siteId,
                Quantity = quantity,
                MovementType = type,
                MovementDate = DateTime.Now,
                ProcessedBy = "Sistem" 
            };

           

            _context.StockMovements.Add(movement);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<StockMovement>> GetMaterialHistoryAsync(int materialId) => await _context.StockMovements.Where(x => x.MaterialId == materialId).OrderByDescending(x => x.MovementDate).ToListAsync();
    }
}