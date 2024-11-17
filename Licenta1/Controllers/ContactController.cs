using Microsoft.AspNetCore.Mvc;

namespace Licenta1.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
