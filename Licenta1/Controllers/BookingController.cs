using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Dictionaries;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace DolphinsSunsetResort.Controllers
{
	public class BookingController : Controller
	{
		private readonly AuthDbContext _context;

		public BookingController(AuthDbContext context)
		{
			_context = context;
		}

		public async Task<IActionResult> Index(string checkInDate, string checkOutDate, BookingStatus bookingStatus)
		{
            // Check if the bookingStatus is not set (default value)
            if (string.IsNullOrEmpty(Request.Query["bookingStatus"]))
            {
                bookingStatus = BookingStatus.None; // Set to None when not provided
            }
            // Parse the dates
            DateTime parsedStartDate = string.IsNullOrEmpty(checkInDate) ? DateTime.MinValue : DateTime.Parse(checkInDate);
			DateTime parsedEndDate = string.IsNullOrEmpty(checkOutDate) ? DateTime.MaxValue : DateTime.Parse(checkOutDate);

			//Set time
			parsedStartDate = new DateTime(parsedStartDate.Year, parsedStartDate.Month, parsedStartDate.Day, 13, 0, 0);
			parsedEndDate = new DateTime(parsedEndDate.Year, parsedEndDate.Month, parsedEndDate.Day, 9, 0, 0);


			var bookingFilters = new BookingFilterViewModel
			{
				CheckInDate = parsedStartDate,
				CheckOutDate = parsedEndDate,
				Status= bookingStatus
			};
			// Get all enum values except "None"
			var bookingStatusList = Enum.GetValues(typeof(BookingStatus))
				.Cast<BookingStatus>()
				.Where(status => status != BookingStatus.None)
				.ToList();

			// Pass the filtered enum values to the view
			ViewBag.bookingStatus = bookingStatusList;
			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return ViewComponent("BookingFilter", new {filterBookings = bookingFilters });
			}

            
            return View("/Views/Booking/Index.cshtml");
		}

		[HttpPost]
		public IActionResult CancelBooking(int bookingId)
		{
			var booking = _context.Bookings.FirstOrDefault(b => b.BookingId == bookingId);

			if (booking != null && booking.Status == BookingStatus.Confirmed)
			{
				booking.Status = BookingStatus.Cancelled;
				_context.SaveChanges();
				return Json(new { success = true });
			}
			return Json(new { success = false, message = "Booking not found or already cancelled." });
		}

	}
}
