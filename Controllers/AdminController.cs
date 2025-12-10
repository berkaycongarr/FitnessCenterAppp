using FitnessApp.Entities;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FitnessApp.Controllers
{
    // BU SATIR ÇOK ÖNEMLİ: Sadece 'admin' rolündeki kullanıcılar buraya girebilir.
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly FitnessDbContext _context;

        public AdminController(FitnessDbContext context)
        {
            _context = context;
        }

        // 1. Admin Paneli Anasayfası (Dashboard)
        public IActionResult Index()
        {
            return View();
        }

        // ==========================================
        // HİZMET YÖNETİMİ (Services)
        // ==========================================

        // Hizmetleri Listele
        public IActionResult Hizmetler()
        {
            var hizmetler = _context.Hizmetler.ToList();
            return View(hizmetler);
        }

           // Hizmet Ekleme Sayfasını Aç
        [HttpGet]
        public IActionResult HizmetEkle()
        {
            return View();
        }

        // Hizmet Ekleme İşlemi (POST)
        [HttpPost]
        public IActionResult HizmetEkle(Hizmet hizmet)
        {
            if (ModelState.IsValid)
            {
                _context.Hizmetler.Add(hizmet);
                _context.SaveChanges();
                TempData["Message"] = "Hizmet başarıyla eklendi.";
                return RedirectToAction("Hizmetler");
            }
            return View(hizmet);
        }

        // Hizmet Silme İşlemi
        public IActionResult HizmetSil(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet != null)
            {
                // İlişkili randevular varsa silmeyi engellemek gerekebilir (Restrict kullanmıştık)
                try
                {
                    _context.Hizmetler.Remove(hizmet);
                    _context.SaveChanges();
                    TempData["Message"] = "Hizmet silindi.";
                }
                catch
                {
                    TempData["Message"] = "Bu hizmete ait randevular olduğu için silinemez!";
                }
            }
            return RedirectToAction("Hizmetler");
     
     
        }



        // ==========================================
        // ANTRENÖR YÖNETİMİ
        // ==========================================

        // 1. Antrenörleri Listele
        public IActionResult Antrenorler()
        {
            // Kullanıcı bilgileriyle beraber (Include) antrenörleri getir
            var antrenorler = _context.Antrenorler
                                      .Include(a => a.User)
                                      .ToList();
            return View(antrenorler);
        }

        // 2. Ekleme Sayfasını Aç
        [HttpGet]
        public IActionResult AntrenorEkle()
        {
            return View();
        }

        // 3. Antrenör Kaydetme İşlemi (Çift Tablo Kaydı)
        [HttpPost]
        public IActionResult AntrenorEkle(AntrenorEkleVM model)
        {
            if (ModelState.IsValid)
            {
                // A) Önce User (Kullanıcı) tablosuna kayıt açıyoruz
                User newUser = new User
                {
                    Username = model.Ad + " " + model.Soyad,
                    Email = model.Email,
                    Phone = model.Phone,
                    Password = model.Password,
                    Role = "antrenor", // Rolü mutlaka 'antrenor' olmalı
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(newUser);
                _context.SaveChanges(); // Kaydet ki ID oluşsun

                // B) Oluşan User'ın ID'sini alıp Antrenör tablosuna ekliyoruz
                Antrenor newTrainer = new Antrenor
                {
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    UzmanlikAlani = model.UzmanlikAlani,
                    UserId = newUser.Id // Bağlantıyı kuruyoruz
                };

                _context.Antrenorler.Add(newTrainer);
                _context.SaveChanges();

                TempData["Message"] = "Antrenör sisteme başarıyla eklendi.";
                return RedirectToAction("Antrenorler");
            }

            return View(model);
        }

        // 4. Antrenör Silme
        public IActionResult AntrenorSil(int id)
        {
            var antrenor = _context.Antrenorler.Include(x => x.User).FirstOrDefault(x => x.Id == id);
            if (antrenor != null)
            {
                // Önce User'ı silersek, Cascade ayarına göre Antrenör de silinir 
                // Ama biz garanti olsun diye manuel silelim.

                var userId = antrenor.UserId;
                var user = _context.Users.Find(userId);

                _context.Antrenorler.Remove(antrenor); // Hoca kaydını sil
                if (user != null)
                    _context.Users.Remove(user);       // Kullanıcı hesabını da sil

                _context.SaveChanges();
                TempData["Message"] = "Antrenör ve kullanıcı hesabı silindi.";
            }
            return RedirectToAction("Antrenorler");
        }






    }
}