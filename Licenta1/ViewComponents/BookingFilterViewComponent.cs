using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Dictionaries;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;


namespace DolphinsSunsetResort.ViewComponents
{
    public class BookingFilterViewComponent : ViewComponent
    {
        private readonly AuthDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BookingFilterViewComponent(AuthDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync(BookingFilterViewModel filterBookings)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookings = _context.Bookings.Include(b => b.BookingRooms)
                .ThenInclude(r => r.Room)
                .Where(b => b.UserId == userId)
                .AsQueryable();

            if (filterBookings == null)
            {
                filterBookings = new BookingFilterViewModel()
                {
                    Bookings = await bookings.ToListAsync()
                };
                return View("/Views/Shared/Components/Booking/BookingFilterList.cshtml", filterBookings);
            }
            bookings = bookings.Where(b => b.CheckInDate >= filterBookings.CheckInDate);

            bookings = bookings.Where(b => b.CheckOutDate <= filterBookings.CheckOutDate);

            // Apply Status filter
            if (filterBookings.Status != BookingStatus.None)
            {
                bookings = bookings.Where(b => b.Status == filterBookings.Status);
            }

            filterBookings.Bookings = await bookings.ToListAsync();


            return View("/Views/Shared/Components/Booking/BookingFilterList.cshtml", filterBookings);
        }

    }
}
