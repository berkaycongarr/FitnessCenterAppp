using FitnessApp.Entities;
using FitnessApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FitnessApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly FitnessDbContext _context;

        public AccountController(FitnessDbContext context)
        {
            _context = context;
        }

        // GET: /Account/Login
        public IActionResult Login()
        {
            // Zaten giriş yapmışsa anasayfaya at
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                // 1. Kullanıcıyı Veritabanında Ara (Email veya Username ile)
                var user = _context.Users.FirstOrDefault(x =>
                    (x.Email == model.Username || x.Username == model.Username) &&
                    x.Password == model.Password);

                if (user != null)
                {
                    // 2. Kullanıcı bulundu, Kimlik Kartı (Claims) Oluştur
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role) // Admin mi Üye mi?
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true // Tarayıcı kapansa da hatırla
                    };

                    // 3. Sisteme Giriş Yap (Cookie Oluştur)
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // 4. Yönlendirme
                    // Eğer admin ise Admin paneline, değilse anasayfaya
                    if (user.Role == "admin")
                    {
                        // Admin panelini henüz yapmadık ama yönlendirmesini hazırlayalım
                        // return RedirectToAction("Index", "Admin"); 
                        return RedirectToAction("Index", "Home"); // Şimdilik buraya gitsin
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Kullanıcı adı veya şifre hatalı!");
                }
            }
            return View(model);
        }



        // ... (Login metodlarının altına ekle)

        // GET: /Account/Register
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                // 1. Email kontrolü (Daha önce kayıtlı mı?)
                if (_context.Users.Any(x => x.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Bu email adresi zaten kayıtlı.");
                    return View(model);
                }

                // 2. Yeni Kullanıcı Oluştur
                User newUser = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password, // Gerçek projede şifrele (Hash) ama ödev için böyle kalsın.
                    Phone = model.Phone,
                    Gender = model.Gender,
                    Role = "uye", // Varsayılan rol ÜYE
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                // 3. Başarılı ise Login sayfasına yönlendir
                TempData["Message"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }

            return View(model);
        }





        // Çıkış Yap
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // Yetkisiz Giriş Sayfası
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}