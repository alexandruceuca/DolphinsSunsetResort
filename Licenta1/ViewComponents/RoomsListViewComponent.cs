using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
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
		public async Task<IViewComponentResult> InvokeAsync(string methodName, DateTime? startDate, DateTime? endDate)
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

		public IViewComponentResult GetFilteredRooms(DateTime? startDate, DateTime? endDate)
		{
			// Retrieve the rooms with necessary related data
			var rooms = _context.Rooms
				.Include(r => r.Price)
				.Include(r => r.BookingRooms)
				.ThenInclude(br => br.Booking)
				.AsQueryable();

			// Initialize the RoomFilterViewModel
			var roomFilter = new RoomFilterViewModel
			{
				Rooms = rooms.ToList(), // Default to all rooms before filtering
				CheckInDate = startDate,
				CheckOutDate = endDate
			};

			// If both dates are null, return all rooms
			if (startDate == null && endDate == null)
			{
				return View("/Views/Shared/Components/Rooms/RoomsFiltered.cshtml", roomFilter);
			}

			// Set default date values if one of the dates is null
			startDate ??= DateTime.MinValue;
			endDate ??= DateTime.MaxValue;

			// Filter rooms based on booking availability
			rooms = rooms.Where(r => !r.BookingRooms
				.Any(br => br.Booking.CheckInDate < endDate && br.Booking.CheckOutDate > startDate));

			// Update the RoomFilterViewModel with filtered data
			roomFilter.Rooms = rooms.ToList();

			// Return the view with the updated model
			return View("/Views/Shared/Components/Rooms/RoomsFiltered.cshtml", roomFilter);
		}

	}
}
