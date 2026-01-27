using ConstructionInventory.Domain.DTOs;
using ConstructionInventory.Domain.Entities;
using ConstructionInventory.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConstructionInventory.Domain.Services
{
    public interface IMaterialService
    {
        Task<List<Material>> GetAllActiveAsync();
        Task CreateAsync(Material material);
        Task<bool> UpdateAsync(int id, Material updatedMaterial);
        Task<bool> DeleteAsync(int id);
        Task<bool> RestoreAsync(int id);
        Task<List<Material>> GetCriticalStockAsync();
        Task<List<Material>> GetArchivedAsync();
        Task<StockDashboardDto> GetDashboardStatsAsync();
        Task<bool> MoveStockAsync(int materialId, int siteId, decimal quantity, MovementType type);
    }
}
