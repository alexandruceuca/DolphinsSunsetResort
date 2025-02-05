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

        public async Task<IViewComponentResult> InvokeAsync(BookingFilterViewModel filterBookings, int page)
        {
            int pageSize = 5;
            var userId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var bookings = _context.Bookings.Include(b => b.BookingRooms)
                .ThenInclude(r => r.Room)
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.BookingDate)
                .AsQueryable();


            if(filterBookings!=null)
            {
                bookings = bookings.Where(b => b.CheckInDate >= filterBookings.CheckInDate);

                bookings = bookings.Where(b => b.CheckOutDate <= filterBookings.CheckOutDate);
                // Apply Status filter
                if (filterBookings.Status != BookingStatus.None)
                {
                    bookings = bookings.Where(b => b.Status == filterBookings.Status);
                }
            }

            // Pagination
            var totalBookings = await bookings.CountAsync();
            var totalPages = (int)Math.Ceiling(totalBookings / (double)pageSize);
            var paginatedNews = await bookings.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var model = new PaginatedBookingListViewModel
            {
                Bookings = paginatedNews,
                CurrentPage = page,
                TotalPages = totalPages,
                PageSize = pageSize
            };

            return View("/Views/Shared/Components/Booking/BookingFilterList.cshtml", model);
        }

    }
}
