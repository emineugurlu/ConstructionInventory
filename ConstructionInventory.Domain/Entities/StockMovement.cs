using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConstructionInventory.Domain.Enums;

namespace ConstructionInventory.Domain.Entities
{
    public class StockMovement
    {
        public int Id { get; set; }
        public  int MaterialId { get; set; } // Hangi malzeme nerde 
        public int ConstructionSiteId { get; set; } // hangi şantiyeden geldi yada gitti
        public decimal Quantity { get; set; } // miktar 
        public MovementType MovementType { get; set; } // hareket tipi
       
        public DateTime MovementDate { get; set; } // ne zaman yapıldı
        public string? ProcessedBy { get; set; } = null; // işlemi yapan kişi 
       
    }
}

