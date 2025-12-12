using FitnessApp.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly FitnessDbContext _context;

        public ApiController(FitnessDbContext context)
        {
            _context = context;
        }

        // GET: api/Api/Antrenorler
        // İsteğe bağlı parametre: ?uzmanlik=Pilates
        [HttpGet("Antrenorler")]
        public IActionResult GetAntrenorler(string? uzmanlik)
        {
            // 1. Temel Sorgu (LINQ Başlangıcı)
            var sorgu = _context.Antrenorler.Include(a => a.User).AsQueryable();

            // 2. LINQ ile Filtreleme (Hocanın İstediği Kısım)
            if (!string.IsNullOrEmpty(uzmanlik))
            {
                sorgu = sorgu.Where(a => a.UzmanlikAlani.Contains(uzmanlik));
            }

            // 3. Veriyi Seçme (Select) - Döngüye girmesin diye özel obje oluşturuyoruz
            var sonuc = sorgu.Select(a => new
            {
                Id = a.Id,
                AdSoyad = a.Ad + " " + a.Soyad,
                Uzmanlik = a.UzmanlikAlani,
                Email = a.User.Email
            }).ToList();

            return Ok(sonuc); // JSON olarak döner (200 OK)
        }
    }
}