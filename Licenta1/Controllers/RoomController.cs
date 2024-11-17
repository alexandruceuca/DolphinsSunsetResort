using Licenta1.Data;
using Licenta1.Views.ViewsModel;
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
			var room = await _context.Rooms
									 .Include(p => p.Price)
									 .FirstOrDefaultAsync(r => r.RoomId == id);

			if (room == null)
			{
				
				return NotFound();
			}

			// Define the folder path for room images
			string roomFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/rooms", $"room-{id}");

			List<string> imageFiles = new List<string>();
			if (Directory.Exists(roomFolderPath))
			{
				// Get all image file names in the folder
				imageFiles = Directory.GetFiles(roomFolderPath, "*.jpg") // Adjust the filter for other extensions if needed
									  .Select(fileName => Path.GetFileName(fileName)) // Get file names only
									  .ToList();
			}

			// Create a ViewModel to hold both room and images
			var viewModel = new RoomInfoViewModel
			{
				Room = room,
				ImagePaths = imageFiles
			};

			// Pass the ViewModel to the Info view
			return View(viewModel);
		}


	}
}
