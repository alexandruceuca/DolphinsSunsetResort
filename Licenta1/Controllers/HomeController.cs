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

		public async Task<IActionResult> Index()
		{
			var bookingCount = _context.Bookings.Where(b => b.CheckInDate.Year == (DateTime.Now.Year - 1)).Count();


			var totalRecommendations = await _context.Bookings.CountAsync(b => b.Status == BookingStatus.CheckOut && b.RecommendationId != (int)Recommendation.NoComment);
			var yesRecommendations = await _context.Bookings
				.CountAsync(b => b.RecommendationId == (int)Recommendation.Yes);
			var percentageYes = totalRecommendations > 0
							? (yesRecommendations / (double)totalRecommendations) * 100
							: 0;

			var rating = _context.Bookings.Where(b => b.Status == BookingStatus.CheckOut && b.Rating!=0).ToList();

		

			var ratingFinal = 0;

			foreach (var booking in rating)
			{
				ratingFinal = booking.Rating + ratingFinal;
			}
			ratingFinal = ratingFinal / rating.Count();

			ViewBag.BookingsCount = bookingCount;
			ViewBag.RatingFinal = ratingFinal;
			ViewBag.Recommendations = percentageYes;
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