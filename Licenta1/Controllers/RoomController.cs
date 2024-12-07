using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.Globalization;

namespace DolphinsSunsetResort.Controllers
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

		public IActionResult GetFilteredRooms(string startDate, string endDate)
		{

			return ViewComponent("RoomsList", new { methodName = "GetFilteredRooms",  startDate,  endDate });

		}

		public async Task<IActionResult> Info(int id, string startDate, string endDate)
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

			//Ensure the DateTime format is the same
			DateTime? parsedStartDate = string.IsNullOrEmpty(startDate)
				? (DateTime?)null
	:			DateTime.ParseExact(startDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

			DateTime? parsedEndDate = string.IsNullOrEmpty(endDate)
				? (DateTime?)null
				: DateTime.ParseExact(endDate, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);

			// Create a ViewModel to hold both room and images
			var viewModel = new RoomInfoViewModel
			{
				Room = room,
				ImagePaths = imageFiles,
				CheckInDate = parsedStartDate,
				CheckOutDate = parsedEndDate
			};

			// Pass the ViewModel to the Info view
			return View(viewModel);
		}


	}
}
