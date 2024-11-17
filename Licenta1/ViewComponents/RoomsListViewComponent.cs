using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
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
			var rooms = _context.Rooms.Include(p => p.Price)
				.Include(bx => bx.BookingRooms)
				.ThenInclude(b => b.Booking)
				.AsQueryable();
			if (startDate == null && endDate == null)
			{
				return View("/Views/Shared/Components/Rooms/RoomsFiltered.cshtml", rooms.ToList());
			}
			if (startDate == null)
			{
				startDate = DateTime.MinValue;
			}
			if (endDate == null)
			{
				endDate = DateTime.MaxValue;
			}

			// Filter the rooms based on the availability dates of bookings
			rooms = rooms.Where(r => !r.BookingRooms
									   .Any(br => br.Booking.CheckInDate < endDate && br.Booking.CheckOutDate > startDate));

			return View("/Views/Shared/Components/Rooms/RoomsFiltered.cshtml", rooms.ToList());

		}
	}
}
