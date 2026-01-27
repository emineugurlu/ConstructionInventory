using ConstructionInventory.Data;
using ConstructionInventory.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ConstructionInventory.Domain.DTOs;
using ConstructionInventory.Domain.Enums;
using ClosedXML.Excel;

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
            if (!ModelState.IsValid)
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
        //silinenleri çöp kutusuna taşıyor bu
        [HttpGet("archive")]
        public async Task<IActionResult> GetArchive()
        {
            var archived = await _context.Materials
                .Where(x => x.IsDeleted) //true olanları seçicek burdan
                .ToListAsync();
            return Ok(archived);
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

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            // Veritabanındaki tüm malzemeleri bir listeye alıyoruz
            var allMaterials = await _context.Materials.ToListAsync();

            var stats = new StockDashboardDto
            {
                // Silinmemiş olanların toplam sayısı
                TotalMaterialCount = allMaterials.Count(x => !x.IsDeleted),

                // Hem silinmemiş hem de stok miktarı limitin altına düşmüş olanlar
                CriticalStockCount = allMaterials.Count(x => !x.IsDeleted && x.StockCount < x.MinStockLimit),

                // Veritabanında duran ama 'silindi' işaretlenmiş olanlar
                ArchivedMaterialCount = allMaterials.Count(x => x.IsDeleted),

                // Depodaki tüm fiziksel ürünlerin toplamı (Örn: 10 torba çimento + 20 adet demir = 30)
                TotalProductQuantity = allMaterials.Where(x => !x.IsDeleted).Sum(x => (long)x.StockCount)
            };

            return Ok(stats);
        }

        [HttpPost("move-stock")]
        public async Task<IActionResult> MoveStock(int materialId, int siteId, decimal quantity, MovementType type, string note)
        {
            //malzeme bul abiiii
            var material = await _context.Materials.FindAsync(materialId);
            if (material == null || material.IsDeleted) return NotFound("Malzeme bulunamadı");

            //stok miktarını bir zahmet güncelle
            if (type == MovementType.Exit || type == MovementType.Waste) // malzeme gidiyorsaaa
            {
                if (material.StockCount < quantity) return BadRequest("Yetersiz stok");
                material.StockCount -= (int)quantity;
            }

            else if (type == MovementType.Entry || type == MovementType.Return) //ifin tersi bir zahmet anla
            {
                material.StockCount += (int)quantity;
            }

            var movement = new StockMovement
            {
                MaterialId = materialId,
                ConstructionSiteId = siteId,
                Quantity = quantity,
                MovementType = type,
                MovementDate = DateTime.Now,
                ProcessedBy = "Admin"

            };

            _context.StockMovements.Add(movement);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Stok hareketi başarıyla kaydedildi.", NewStock = material.StockCount });

        }

        [HttpGet("history/{materialId}")]
        public async Task<IActionResult> GetMaterialHistory(int materialId)
        {
            var history = await _context.StockMovements
                .Where(x => x.MaterialId == materialId)
                .OrderByDescending(x => x.MovementDate)
                .ToListAsync();
            return Ok(history);

        }

        [HttpGet("export-excel")]
        public IActionResult ExportToExcel()
        {
            var materials = _context.Materials.Where(x => !x.IsDeleted).ToList();

            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Malzeme Listesi");

                //Başlıkları oluşturrr
                worksheet.Cell(1, 1).Value = "ID";
                worksheet.Cell(1, 2).Value = "Malzeme Adı";
                worksheet.Cell(1, 3).Value = "Stok Miktarı";
                worksheet.Cell(1, 4).Value = "Birim";
                worksheet.Cell(1, 5).Value = "Kritik Limiti";


                //verileri doldurrr hadiii
                for (int i = 0; i < materials.Count; i++)
                {
                    var currentRow = i + 2;
                    worksheet.Cell(currentRow, 1).Value = materials[i].Id;
                    worksheet.Cell(currentRow, 2).Value = materials[i].Name;
                    worksheet.Cell(currentRow, 3).Value = materials[i].StockCount;
                    worksheet.Cell(currentRow, 4).Value = materials[i].Unit;
                    worksheet.Cell(currentRow, 5).Value = materials[i].MinStockLimit;

                    // Eğer stok miktarı kritik limitin altındaysa o satırı KIRMIZI yap!
                    if (materials[i].StockCount < materials[i].MinStockLimit)
                    {
                        worksheet.Row(currentRow).Style.Fill.BackgroundColor = XLColor.Alizarin; // Şık bir kırmızı
                        worksheet.Row(currentRow).Style.Font.FontColor = XLColor.White; // Beyaz yazı
                    }
                }

                //tabloyu güzelleştirim birazz :)
                worksheet.Columns().AdjustToContents();

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();

                    return File(
                        content,
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                        "Santiye_Stok_Raporu.xlsx"
                    );
                }
            }
        }
    }
}