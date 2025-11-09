using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApp.Models
{
    public class Appointment : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
