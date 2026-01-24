using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ConstructionInventory.Domain.Entities
{
    public class Material
    {
        public int Id { get; set; } // Her ürün için id
        public string Name { get; set; }   // Her ürünün adı
        public decimal StockCount { get; set; } // Toplam stok miktarı
        public decimal MinStockLimit { get; set; } // Kritik stok limiti örn 5 kalırsa uyarı ver
        public string Unit { get; set; } // Ölçü birimi 
    }
}
