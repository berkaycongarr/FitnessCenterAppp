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

        
        public IActionResult Login()
        {
      
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (ModelState.IsValid)
            {
                
                var user = _context.Users.FirstOrDefault(x =>
                    (x.Email == model.Username || x.Username == model.Username) &&
                    x.Password == model.Password);

                if (user != null)
                {
                    
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Role, user.Role) 
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true 
                    };

                    
                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    
                    if (user.Role == "admin")
                    {
                       
                        return RedirectToAction("Index", "Home"); 
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



       

        
        public IActionResult Register()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                
                if (_context.Users.Any(x => x.Email == model.Email))
                {
                    ModelState.AddModelError("Email", "Bu email adresi zaten kayıtlı.");
                    return View(model);
                }

               
                User newUser = new User
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password, 
                    Phone = model.Phone,
                    Gender = model.Gender,
                    Role = "uye", 
                    CreatedAt = DateTime.Now
                };

                _context.Users.Add(newUser);
                _context.SaveChanges();

                
                TempData["Message"] = "Kayıt başarılı! Giriş yapabilirsiniz.";
                return RedirectToAction("Login");
            }

            return View(model);
        }





        
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}