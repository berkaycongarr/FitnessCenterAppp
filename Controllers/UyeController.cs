using FitnessApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessApp.Controllers
{
    [Authorize] 
    public class UyeController : Controller
    {
        private readonly FitnessDbContext _context;

        public UyeController(FitnessDbContext context)
        {
            _context = context;
        }

     
        public IActionResult Index()
        {
            
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            
            var randevular = _context.Randevular
                                     .Include(r => r.Hizmet)
                                     .Include(r => r.Antrenor)
                                     .Where(r => r.UserId == userId)
                                     .OrderByDescending(r => r.TarihSaat) 
                                     .ToList();

            return View(randevular);
        }

        
        public IActionResult IptalEt(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            
            var randevu = _context.Randevular.FirstOrDefault(r => r.Id == id && r.UserId == userId);

            if (randevu != null)
            {
                
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