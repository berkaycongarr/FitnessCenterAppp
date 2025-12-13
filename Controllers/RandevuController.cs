using FitnessApp.Entities;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessApp.Controllers
{
    [Authorize] 
    public class RandevuController : Controller
    {
        private readonly FitnessDbContext _context;

        public RandevuController(FitnessDbContext context)
        {
            _context = context;
        }

        
        [HttpGet]
        public IActionResult Al(int? hizmetId)
        {
            var model = new RandevuTalepVM();

            
            if (hizmetId.HasValue)
            {
                model.HizmetId = hizmetId.Value;
            }

            LoadDropdowns(); 
            return View(model);
        }

     
        [HttpPost]
        public IActionResult Al(RandevuTalepVM model)
        {
            if (ModelState.IsValid)
            {
             
                TimeSpan zaman;
                if (!TimeSpan.TryParse(model.Saat, out zaman))
                {
                    ModelState.AddModelError("Saat", "Geçersiz saat formatı.");
                    LoadDropdowns();
                    return View(model);
                }

                DateTime randevuZamani = model.Tarih.Date + zaman;

                
                if (randevuZamani < DateTime.Now)
                {
                    ModelState.AddModelError("Tarih", "Geçmiş bir tarihe randevu alamazsınız.");
                    LoadDropdowns();
                    return View(model);
                }

                
                var secilenHoca = _context.Antrenorler.Find(model.AntrenorId);
                int randevuSaati = zaman.Hours;

                if (randevuSaati < secilenHoca.BaslangicSaati || randevuSaati >= secilenHoca.BitisSaati)
                {
                    ModelState.AddModelError("Saat", $"Seçtiğiniz eğitmen sadece {secilenHoca.BaslangicSaati}:00 - {secilenHoca.BitisSaati}:00 saatleri arasında çalışmaktadır.");
                    LoadDropdowns();
                    return View(model);
                }

                bool doluMu = _context.Randevular.Any(r =>
                    r.AntrenorId == model.AntrenorId &&
                    r.TarihSaat == randevuZamani &&
                    r.Durum != 2 
                );

                if (doluMu)
                {
                    ModelState.AddModelError("", "Seçtiğiniz eğitmen bu tarih ve saatte dolu.");
                    LoadDropdowns();
                    return View(model);
                }

               
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                Randevu yeniRandevu = new Randevu
                {
                    UserId = userId,
                    HizmetId = model.HizmetId,
                    AntrenorId = model.AntrenorId,
                    TarihSaat = randevuZamani,
                    Durum = 0
                };

                _context.Randevular.Add(yeniRandevu);
                _context.SaveChanges();

                TempData["Message"] = "Randevunuz başarıyla oluşturuldu!";
                return RedirectToAction("Index", "Home");
            }

            LoadDropdowns();
            return View(model);
        }

        
        private void LoadDropdowns()
        {
            ViewBag.Hizmetler = new SelectList(_context.Hizmetler.ToList(), "Id", "Ad");

            var antrenorler = _context.Antrenorler
                .Select(a => new { Id = a.Id, AdSoyad = a.Ad + " " + a.Soyad })
                .ToList();
            ViewBag.Antrenorler = new SelectList(antrenorler, "Id", "AdSoyad");
        }
    }
}