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
        Task<List<Material>> GetAllMaterialsAsync();
        Task<StockDashboardDto> GetDashboardStatsAsync();
        Task<bool> MoveStockAsync(int materialId, decimal quantity, MovementType type);
    }
}
