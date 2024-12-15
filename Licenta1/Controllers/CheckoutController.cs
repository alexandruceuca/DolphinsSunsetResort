using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Models;
using DolphinsSunsetResort.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DolphinsSunsetResort.Controllers
{
	[Authorize]
	public class CheckoutController : Controller
	{
		private readonly AuthDbContext _context;
        private readonly UserManager<AplicationUser> _userManager;
        public CheckoutController(AuthDbContext context, UserManager<AplicationUser> userManager)
		{
			_context = context;
			_userManager = userManager;
		}
		public async Task<ActionResult> Payment()
		{
			var booking = new Booking();

			try
			{
				var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;


				if (string.IsNullOrEmpty(userId))
				{
					throw new Exception("User is not logged in or the user ID is not available.");
				}

				booking.UserId = userId;
				booking.BookingDate = DateTime.Now;

				//Save booking
				_context.Bookings.Add(booking);
				_context.SaveChanges();

				//Process the booking
				var cart = CartService.GetCart(_context, this.HttpContext);
				cart.CreateBooking(booking);

				var user = await  _userManager.GetUserAsync(User);

				var notification = new EmailNotification(user, "Booking", "", booking.BookingId, booking.TotalPrice, booking.CheckInDate, booking.CheckOutDate);

				var manager = new NotificationManager();
				manager.SendNotification(notification);

                return RedirectToAction("Complete",
					new { id = booking.BookingId });

			}
			catch
			{
				//Invalid - redisplay with errors
				return View(booking);
			}
		}

		public ActionResult Complete(int id)
		{
			var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;


			if (string.IsNullOrEmpty(userId))
			{
				throw new Exception("User is not logged in or the user ID is not available.");
			}

			// Validate customer owns this booking
			bool isValid = _context.Bookings.Any(
				b => b.BookingId == id &&
				b.UserId == userId);

			if (isValid)
			{
				return View("/Views/CheckOut/Complete.cshtml",id);
			}
			else
			{
				return View("Error");
			}
		}

		public IActionResult Index()
		{
			return View();
		}


	}
}
