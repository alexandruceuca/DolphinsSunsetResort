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
        private readonly RoleManager<IdentityRole> _roleManager;
        public CheckoutController(AuthDbContext context, UserManager<AplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
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
				
				//Send notification
				var user = await  _userManager.GetUserAsync(User);

				var managerRole = await _roleManager.FindByNameAsync("Manager");
				if (managerRole == null)
				{
					return NotFound("The 'Manager' role does not exist.");
				}

				// Get users in the role
				var usersInManagerRole = await _userManager.GetUsersInRoleAsync("Manager");


                var notificationUser = new EmailNotification(user, "Booking", "", booking.BookingId, booking.TotalPrice, booking.CheckInDate, booking.CheckOutDate);
                var notificationManager = new EmailNotification(usersInManagerRole, user, "Booking", booking.BookingId, booking.TotalPrice, booking.CheckInDate, booking.CheckOutDate);
                var manager = new NotificationManager();
				manager.SendNotification(notificationUser);
				manager.SendNotification(notificationManager);
				_context.EmailNotification.Add(notificationUser);
				_context.EmailNotification.Add(notificationManager);



				if (user.PhoneNumber != null)
				{
					var notificationSms = new SmsNotification(user, booking.BookingId, booking.TotalPrice, booking.CheckInDate, booking.CheckOutDate);
					manager.SendNotification(notificationSms);
					_context.SmsNotification.Add(notificationSms);
				}
				await _context.SaveChangesAsync();
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
