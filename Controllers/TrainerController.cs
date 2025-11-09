using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApp.Controllers
{
    public class TrainerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
