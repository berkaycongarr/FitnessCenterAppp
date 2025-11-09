using Microsoft.AspNetCore.Mvc;

namespace FitnessCenterApp.Controllers
{
    public class MemberController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
