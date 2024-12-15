using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Models;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Data;

namespace DolphinsSunsetResort.ViewComponents
{
    public class ReceptionBookingsListViewComponent : ViewComponent
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<AplicationUser> _userManager;

        public ReceptionBookingsListViewComponent(UserManager<AplicationUser> userManager, AuthDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Authorize(Roles = "Admin,Manager,Reception")]
        public async Task<IViewComponentResult> InvokeAsync(BookingFilterViewModel filters)
        {
            var bookings = _context.Bookings.Include(b => b.AplicationUser)
                                            .AsQueryable();

            // Apply filters if values are provided
            if (filters != null)
            {
                if (filters.BookingIdFilter != null && filters.BookingIdFilter != 0)
                {
                    bookings = bookings.Where(b => b.BookingId == filters.BookingIdFilter);
                }
                if (!string.IsNullOrEmpty(filters.PhoneNumberFilter))
                {
                    bookings = bookings.Where(b => b.AplicationUser.PhoneNumber.Contains(filters.PhoneNumberFilter));
                }
                if (!string.IsNullOrEmpty(filters.EmailFilter))
                {
                    bookings = bookings.Where(b => b.AplicationUser.Email.Contains(filters.EmailFilter));
                }

                if (filters.AllBookings == false)
                {
                    bookings = bookings.Where(b => b.CheckInDate.Date <= DateTime.Now.Date && b.CheckOutDate.Date >= DateTime.Now.Date);
                }
                else
                {
                    bookings = bookings.Where(b => b.CheckInDate >= filters.CheckInDate);

                    bookings = bookings.Where(b => b.CheckOutDate <= filters.CheckOutDate);

                }
            }
            return View("/Views/Shared/Components/Roles/Reception/ReceptionBookingsList.cshtml", bookings.ToList());
        }
    }
}
