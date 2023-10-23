using Microsoft.AspNetCore.Mvc;

namespace YourProjectNamespace.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
