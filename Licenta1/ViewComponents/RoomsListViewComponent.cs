using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Utils;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DolphinsSunsetResort.ViewComponents
{
    public class RoomsListViewComponent : ViewComponent
    {
        private readonly AuthDbContext _context;

        public RoomsListViewComponent(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string methodName, string startDate, string endDate)
        {
            if (methodName == "DisplayRoomListHomeAsync")
                return await DisplayRoomListHomeAsync();
            if (methodName == "DisplayRoomListAsync")
                return await DisplayRoomListAsync();
            if (methodName == "GetFilteredRooms")
                return GetFilteredRooms(startDate, endDate);

            throw new ArgumentException("Invalid method name");
        }

        public async Task<IViewComponentResult> DisplayRoomListHomeAsync()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return View("/Views/Shared/Components/Rooms/RoomsListHome.cshtml", rooms);

        }

        public async Task<IViewComponentResult> DisplayRoomListAsync()
        {
            var rooms = await _context.Rooms.Include(d => d.Price).ToListAsync();
            return View("/Views/Shared/Components/Rooms/RoomsList.cshtml", rooms);

        }

        public IViewComponentResult GetFilteredRooms(string startDate, string endDate)
        {
            // Retrieve the rooms with necessary related data
            var rooms = _context.Rooms
                .Include(r => r.Price)
                .Include(r => r.BookingRooms)
                .ThenInclude(br => br.Booking)
                .AsQueryable();


            var roomFilter = new RoomFilterViewModel
            {
                Rooms = rooms.ToList()
            };

            // If both dates are null, return all rooms
            if (startDate == null && endDate == null)
            {
                return View("/Views/Shared/Components/Rooms/RoomsFiltered.cshtml", roomFilter);
            }
			var (parsedStartDate, parsedEndDate) = UtilsDate.ParseAndSetBookingDates(startDate, endDate);
			//// Parse the dates
			//DateTime parsedStartDate = string.IsNullOrEmpty(startDate) ? DateTime.MinValue : DateTime.Parse(startDate);
   //         DateTime parsedEndDate = string.IsNullOrEmpty(endDate) ? DateTime.MaxValue : DateTime.Parse(endDate);

   //         //Set time
   //         parsedStartDate = new DateTime(parsedStartDate.Year, parsedStartDate.Month, parsedStartDate.Day, 13, 0, 0);
   //         parsedEndDate = new DateTime(parsedEndDate.Year, parsedEndDate.Month, parsedEndDate.Day, 9, 0, 0);

			// Filter rooms based on booking availability
			rooms = rooms.Where(r => !r.BookingRooms
	                .Any(br => br.Booking.CheckInDate < parsedEndDate
		                 && br.Booking.CheckOutDate > parsedStartDate
		                 && (br.Booking.Status == Dictionaries.BookingStatus.Confirmed
			             || br.Booking.Status == Dictionaries.BookingStatus.CheckIn)));


			// Update the RoomFilterViewModel with filtered data
			roomFilter.Rooms = rooms.ToList();
            roomFilter.CheckInDate = parsedStartDate;
            roomFilter.CheckOutDate = parsedEndDate;

            // Return the view with the updated model
            return View("/Views/Shared/Components/Rooms/RoomsFiltered.cshtml", roomFilter);
        }

    }
}
