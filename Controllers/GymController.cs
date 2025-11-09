using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApp.Controllers
{
    public class GymController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
