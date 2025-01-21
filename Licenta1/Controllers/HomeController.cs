using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Dictionaries;
using DolphinsSunsetResort.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Diagnostics;

namespace DolphinsSunsetResort.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
		private readonly AuthDbContext _context;

		public HomeController(ILogger<HomeController> logger, AuthDbContext context)
        {
            _logger = logger;
			_context = context;
		}

        public IActionResult Index()
        {
			var bookingCount=_context.Bookings.Where(b=>b.CheckInDate.Year==(DateTime.Now.Year-1)).Count();
            ViewBag.BookingsCount = bookingCount;
            return View();
        }




		public IActionResult Facilities()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}