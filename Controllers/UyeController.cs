using FitnessApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessApp.Controllers
{
    [Authorize] // Sadece giriş yapmış kullanıcılar girebilir
    public class UyeController : Controller
    {
        private readonly FitnessDbContext _context;

        public UyeController(FitnessDbContext context)
        {
            _context = context;
        }

        // 1. Randevularım Listesi
        public IActionResult Index()
        {
            // Giriş yapan kullanıcının ID'sini bul
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Veritabanından bu kişinin randevularını çek
            // Include komutları ile Hizmet ve Antrenör isimlerini de getiriyoruz
            var randevular = _context.Randevular
                                     .Include(r => r.Hizmet)
                                     .Include(r => r.Antrenor)
                                     .Where(r => r.UserId == userId)
                                     .OrderByDescending(r => r.TarihSaat) // En yeni en üstte olsun
                                     .ToList();

            return View(randevular);
        }

        // 2. Randevu İptal Etme
        public IActionResult IptalEt(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            // Sadece bu kullanıcıya ait olan randevuyu bul (Güvenlik Önlemi)
            var randevu = _context.Randevular.FirstOrDefault(r => r.Id == id && r.UserId == userId);

            if (randevu != null)
            {
                // Durumu 'İptal' (2) yapıyoruz. İstersen direkt silebilirsin de (_context.Remove(randevu))
                randevu.Durum = 2;
                _context.SaveChanges();
                TempData["Message"] = "Randevu iptal edildi.";
            }
            else
            {
                TempData["Message"] = "Randevu bulunamadı veya işlem yetkiniz yok.";
            }

            return RedirectToAction("Index");
        }
    }
}