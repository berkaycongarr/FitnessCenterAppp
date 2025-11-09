using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApp.Models
{
    public class Member : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
