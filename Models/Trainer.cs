using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApp.Models
{
    public class Trainer : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
