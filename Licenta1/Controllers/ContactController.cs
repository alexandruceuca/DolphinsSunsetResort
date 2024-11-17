using Microsoft.AspNetCore.Mvc;

namespace DolphinsSunsetResort.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
