using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionInventory.Domain.DTOs
{
    public class StockDashboardDto
    {
        public int TotalMaterialCount { get; set; }
        public int CriticalStockCount { get; set; }
        public int ArchivedMaterialCount { get; set; }
        public long TotalProductQuantity { get; set; }

    }
}
