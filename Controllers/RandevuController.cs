using FitnessApp.Entities;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FitnessApp.Controllers
{
    [Authorize] // Sadece giriş yapmış üyeler randevu alabilir
    public class RandevuController : Controller
    {
        private readonly FitnessDbContext _context;

        public RandevuController(FitnessDbContext context)
        {
            _context = context;
        }

        // 1. Randevu Alma Sayfasını Aç (GET)
        [HttpGet]
        public IActionResult Al()
        {
            // Dropdownları dolduruyoruz
            ViewBag.Hizmetler = new SelectList(_context.Hizmetler.ToList(), "Id", "Ad");

            // Eğitmen isimlerini "Ad Soyad" olarak birleştirip dropdown'a koyuyoruz
            var antrenorler = _context.Antrenorler
                .Select(a => new { Id = a.Id, AdSoyad = a.Ad + " " + a.Soyad })
                .ToList();
            ViewBag.Antrenorler = new SelectList(antrenorler, "Id", "AdSoyad");

            return View();
        }

        // 2. Randevuyu Kaydet (POST)
        [HttpPost]
        public IActionResult Al(RandevuTalepVM model)
        {
            if (ModelState.IsValid)
            {
                // A) Tarih ve Saati birleştir (DateTime objesi yap)
                // Modelden gelen Saat string'ini (Örn: "09:00") parçalayıp saate çeviriyoruz.
                TimeSpan zaman;
                if (!TimeSpan.TryParse(model.Saat, out zaman))
                {
                    ModelState.AddModelError("Saat", "Geçersiz saat formatı.");
                    LoadDropdowns(); // Dropdownları tekrar yükle
                    return View(model);
                }

                DateTime randevuZamani = model.Tarih.Date + zaman;

                // B) Kontrol: Geçmişe randevu alınamaz
                if (randevuZamani < DateTime.Now)
                {
                    ModelState.AddModelError("Tarih", "Geçmiş bir tarihe randevu alamazsınız.");
                    LoadDropdowns();
                    return View(model);
                }

                // C) Kontrol: O saatte o hoca dolu mu?
                bool doluMu = _context.Randevular.Any(r =>
                    r.AntrenorId == model.AntrenorId &&
                    r.TarihSaat == randevuZamani &&
                    r.Durum != 2 // İptal edilmişse orası boş sayılır
                );

                if (doluMu)
                {
                    ModelState.AddModelError("", "Seçtiğiniz eğitmen bu tarih ve saatte dolu. Lütfen başka bir saat seçiniz.");
                    LoadDropdowns();
                    return View(model);
                }

                // D) Kayıt İşlemi
                // Giriş yapan kullanıcının ID'sini bul
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                Randevu yeniRandevu = new Randevu
                {
                    UserId = userId,
                    HizmetId = model.HizmetId,
                    AntrenorId = model.AntrenorId,
                    TarihSaat = randevuZamani,
                    Durum = 0 // Beklemede
                };

                _context.Randevular.Add(yeniRandevu);
                _context.SaveChanges();

                TempData["Message"] = "Randevunuz başarıyla oluşturuldu!";
                return RedirectToAction("Index", "Home"); // Şimdilik Anasayfaya gitsin
            }

            LoadDropdowns();
            return View(model);
        }

        // Yardımcı Metod: Hata durumunda dropdownları tekrar doldurmak için
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