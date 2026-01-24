using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace ConstructionInventory.Domain.Entities
{
    public class Material
    {
        public int Id { get; set; } // Her ürün için id

        [Required(ErrorMessage = "Malzeme adı boş bırakılmaz.")]
        [StringLength(100, MinimumLength =2, ErrorMessage ="Malzeme adı 2 ile 100 karakter araasında olmaalıdır.")]
        public string Name { get; set; }   // Her ürünün adı
        [Range(0,1000000, ErrorMessage ="Stok miktarı 0'dan küçük olamaz")]
        public decimal StockCount { get; set; } // Toplam stok miktarı
        [Range(0, 10000, ErrorMessage = "Minimum stok limiti negatif olamaz.")]
        public decimal MinStockLimit { get; set; } // Kritik stok limiti örn 5 kalırsa uyarı ver
        [Required(ErrorMessage = "Malzeme ölçü birimi boş bırakılamaz(Adet,Ton, vb.).")]
        public string Unit { get; set; } // Ölçü birimi 
    }
}
