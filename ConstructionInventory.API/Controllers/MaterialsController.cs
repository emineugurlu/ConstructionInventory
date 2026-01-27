using ClosedXML.Excel;
using ConstructionInventory.Domain.Entities;
using ConstructionInventory.Domain.Enums;
using ConstructionInventory.Domain.Services;
using Microsoft.AspNetCore.Mvc;

namespace ConstructionInventory.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MaterialsController : ControllerBase
    {
        private readonly IMaterialService _materialService;
        public MaterialsController(IMaterialService materialService) { _materialService = materialService; }

        [HttpGet] public async Task<IActionResult> GetAll() => Ok(await _materialService.GetAllActiveAsync());

        [HttpPost] public async Task<IActionResult> Create(Material m) { await _materialService.CreateAsync(m); return Ok(m); }

        [HttpPut("{id}")] public async Task<IActionResult> Update(int id, Material m) => await _materialService.UpdateAsync(id, m) ? Ok("Güncellendi") : NotFound();

        [HttpDelete("{id}")] public async Task<IActionResult> Delete(int id) => await _materialService.DeleteAsync(id) ? Ok("Silindi") : NotFound();

        [HttpGet("dashboard")] public async Task<IActionResult> GetDashboard() => Ok(await _materialService.GetDashboardStatsAsync());

        [HttpGet("critical-stock")] public async Task<IActionResult> GetCritical() => Ok(await _materialService.GetCriticalStockAsync());

        [HttpGet("archive")] public async Task<IActionResult> GetArchive() => Ok(await _materialService.GetArchivedAsync());

        [HttpPost("restore/{id}")] public async Task<IActionResult> Restore(int id) => await _materialService.RestoreAsync(id) ? Ok("Geri yüklendi") : NotFound();

        [HttpPost("move-stock")]
        public async Task<IActionResult> MoveStock(int materialId, int siteId, decimal quantity, MovementType type, string note) =>
            await _materialService.MoveStockAsync(materialId, siteId, quantity, type, note) ? Ok("Başarılı") : BadRequest("İşlem başarısız.");

        [HttpGet("export-excel")]
        public async Task<IActionResult> ExportToExcel()
        {
            var materials = await _materialService.GetAllActiveAsync();
            using var workbook = new XLWorkbook();
            var ws = workbook.Worksheets.Add("Stok");
            ws.Cell(1, 1).Value = "Ad"; ws.Cell(1, 2).Value = "Stok";
            for (int i = 0; i < materials.Count; i++)
            {
                ws.Cell(i + 2, 1).Value = materials[i].Name;
                ws.Cell(i + 2, 2).Value = materials[i].StockCount;
            }
            using var ms = new MemoryStream();
            workbook.SaveAs(ms);
            return File(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Rapor.xlsx");
        }
    }
}