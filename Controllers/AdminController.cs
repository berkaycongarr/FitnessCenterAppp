using FitnessApp.Entities;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace FitnessApp.Controllers
{
   
    [Authorize(Roles = "admin")]
    public class AdminController : Controller
    {
        private readonly FitnessDbContext _context;

        public AdminController(FitnessDbContext context)
        {
            _context = context;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Hizmetler()
        {
            var hizmetler = _context.Hizmetler.ToList();
            return View(hizmetler);
        }

           
        [HttpGet]
        public IActionResult HizmetEkle()
        {
            return View();
        }

        
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

        
        public IActionResult HizmetSil(int id)
        {
            var hizmet = _context.Hizmetler.Find(id);
            if (hizmet != null)
            {
                
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



        
        public IActionResult Antrenorler()
        {
            
            var antrenorler = _context.Antrenorler
                                      .Include(a => a.User)
                                      .ToList();
            return View(antrenorler);
        }

        
        [HttpGet]
        public IActionResult AntrenorEkle()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult AntrenorEkle(AntrenorEkleVM model)
        {
            if (ModelState.IsValid)
            {
               
                User newUser = new User
                {
                    Username = model.Ad + " " + model.Soyad,
                    Email = model.Email,
                    Phone = model.Phone,
                    Password = model.Password,
                    Role = "antrenor", 
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(newUser);
                _context.SaveChanges(); 

                Antrenor newTrainer = new Antrenor
                {
                    Ad = model.Ad,
                    Soyad = model.Soyad,
                    UzmanlikAlani = model.UzmanlikAlani,
                    UserId = newUser.Id,
                    // YENİ EKLENENLER:
                    BaslangicSaati = model.BaslangicSaati,
                    BitisSaati = model.BitisSaati
                };

                _context.Antrenorler.Add(newTrainer);
                _context.SaveChanges();

                TempData["Message"] = "Antrenör sisteme başarıyla eklendi.";
                return RedirectToAction("Antrenorler");
            }

            return View(model);
        }

      
        public IActionResult AntrenorSil(int id)
        {
            var antrenor = _context.Antrenorler.Include(x => x.User).FirstOrDefault(x => x.Id == id);
            if (antrenor != null)
            {
                

                var userId = antrenor.UserId;
                var user = _context.Users.Find(userId);

                _context.Antrenorler.Remove(antrenor); 
                if (user != null)
                    _context.Users.Remove(user);       

                _context.SaveChanges();
                TempData["Message"] = "Antrenör ve kullanıcı hesabı silindi.";
            }
            return RedirectToAction("Antrenorler");
        }

       

        public IActionResult Randevular()
        {
            
            var randevular = _context.Randevular
                .Include(r => r.User)       
                .Include(r => r.Antrenor)   
                .Include(r => r.Hizmet)     
                .OrderByDescending(r => r.TarihSaat) 
                .ToList();

            return View(randevular);
        }

      
        public IActionResult RandevuOnayla(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu != null)
            {
                randevu.Durum = 1; 
                _context.SaveChanges();
                TempData["Message"] = "Randevu onaylandı.";
            }
            return RedirectToAction("Randevular");
        }

       
        public IActionResult RandevuReddet(int id)
        {
            var randevu = _context.Randevular.Find(id);
            if (randevu != null)
            {
                randevu.Durum = 2; 
                _context.SaveChanges();
                TempData["Message"] = "Randevu reddedildi.";
            }
            return RedirectToAction("Randevular");
        }




    }
}