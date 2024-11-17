using Microsoft.AspNetCore.Mvc;

namespace Licenta1.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
