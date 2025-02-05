using DolphinsSunsetResort.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DolphinsSunsetResort.ViewComponents
{
    [Authorize(Roles = "Admin,Manager,Reception,RoomCleaner")]
    public class RoomsCleaningListViewComponent : ViewComponent
    {
        private readonly AuthDbContext _context;

        public RoomsCleaningListViewComponent(AuthDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Manager,Reception,RoomCleaner")]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roomsNeedingCleaning = await _context.Rooms
                .Where(r => r.RoomStatus == Dictionaries.RoomStatus.NeedsCleaning)
                .ToListAsync();

            // Dictionary to store next booking for each room
            var nextBookingTimes = new Dictionary<int, DateTime?>();

            foreach (var room in roomsNeedingCleaning)
            {

                var nextBooking = await _context.BookingRooms
                    .Where(br => br.RoomId == room.RoomId 
                    && br.Booking.CheckInDate > DateTime.Now
                    && br.Booking.CheckInDate != DateTime.MinValue
                    && br.Booking.Status == Dictionaries.BookingStatus.Confirmed)
                    .OrderBy(br => br.Booking.CheckInDate)
                    .Select(br => br.Booking.CheckInDate)
                    .FirstOrDefaultAsync();

                nextBookingTimes[room.RoomId] = nextBooking;
            }


            ViewBag.NextBookingTimes = nextBookingTimes;

            return View("/Views/Shared/Components/Roles/RoomCleaning/RoomsCleaningList.cshtml", roomsNeedingCleaning);
        }
    }
}
