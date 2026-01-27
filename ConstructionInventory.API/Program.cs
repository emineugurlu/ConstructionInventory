using ConstructionInventory.API.Services;
using ConstructionInventory.Data;
using ConstructionInventory.Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
var builder = WebApplication.CreateBuilder(args);

// 1. Servisleri Kaydet
builder.Services.AddControllers(); // Bu satýr Controller'larýný sisteme tanýtýr!
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger (test ekraný) için gerekli
builder.Services.AddScoped<IMaterialService, MaterialService>();

// Veritabaný baðlantýsý
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// 2. HTTP Ýstek Hattýný Yapýlandýr
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Tarayýcýda /swagger yazýnca açýlan ekran
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 3. Rotalarý Eþleþtir
app.MapControllers(); // Adres çubuðuna yazýlanlarý Controller'lara yönlendirir

app.MapGet("/", () => "Sistem Hazýr! Test için /api/materials veya /swagger adresine gidin.");

app.Run();