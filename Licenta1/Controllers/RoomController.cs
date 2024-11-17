using Licenta1.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Licenta1.Controllers
{
    public class RoomController : Controller
    {
        private readonly AuthDbContext _context;

        public RoomController(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var rooms = await _context.Rooms.ToListAsync();

            return View(rooms);
        }

		public async Task<IActionResult> Info(int id)
		{
			// Fetch the room from the database using the provided RoomId
			var room = await _context.Rooms.Include(p=>p.Price)
									 .FirstOrDefaultAsync(r => r.RoomId == id);

			if (room == null)
			{
				// Handle the case when the room is not found
				return NotFound();
			}

			// Pass the room details to the Info view
			return View(room);
		}

	}
}
