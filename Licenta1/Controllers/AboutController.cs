using Microsoft.AspNetCore.Mvc;

namespace DolphinsSunsetResort.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
