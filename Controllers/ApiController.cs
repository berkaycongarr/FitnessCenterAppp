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

        
        [HttpGet("Antrenorler")]
        public IActionResult GetAntrenorler(string? uzmanlik)
        {
            
            var sorgu = _context.Antrenorler.Include(a => a.User).AsQueryable();

            
            if (!string.IsNullOrEmpty(uzmanlik))
            {
                sorgu = sorgu.Where(a => a.UzmanlikAlani.Contains(uzmanlik));
            }

           
            var sonuc = sorgu.Select(a => new
            {
                Id = a.Id,
                AdSoyad = a.Ad + " " + a.Soyad,
                Uzmanlik = a.UzmanlikAlani,
                Email = a.User.Email
            }).ToList();

            return Ok(sonuc); 
        }
    }
}