using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApp.Models
{
    public class Gym : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
