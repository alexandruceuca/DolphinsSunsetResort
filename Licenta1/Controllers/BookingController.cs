using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Dictionaries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        public IActionResult Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookings = _context.Bookings.Include(b => b.BookingRooms)
                .ThenInclude(r => r.Room)
                .Where(b => b.UserId == userId)
                .ToList();

            return View(bookings);
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
