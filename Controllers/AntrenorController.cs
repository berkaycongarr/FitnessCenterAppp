using Microsoft.AspNetCore.Mvc;

namespace FitnessApp.Controllers
{
    public class AntrenorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}