using FitnessApp.Entities;
using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Controllers
{
    public class HizmetController : Controller
    {
        private readonly FitnessDbContext _context;

        public HizmetController(FitnessDbContext context)
        {
            _context = context;
        }

      
        public IActionResult Index()
        {
            
            var hizmetler = _context.Hizmetler.ToList();
            return View(hizmetler);
        }
    }
}