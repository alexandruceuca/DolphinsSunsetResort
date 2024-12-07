using Microsoft.AspNetCore.Mvc;
using DolphinsSunsetResort.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Cors.Infrastructure;
using DolphinsSunsetResort.Service;
using DolphinsSunsetResort.Data;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using DolphinsSunsetResort.Views.ViewsModel;
using System.Text.Encodings.Web;

public class BookingCartController : Controller
{
	private readonly AuthDbContext _context;

	public BookingCartController(AuthDbContext context)
	{
		_context = context;
	}


	public async Task<IActionResult> AddToCart(int roomId, string checkInDate, string checkOutDate)
	{
		try
		{
			// Parse the dates
			DateTime parsedStartDate =  DateTime.Parse(checkInDate);
			DateTime parsedEndDate =  DateTime.Parse(checkOutDate);

			//Set time
			parsedStartDate = new DateTime(parsedStartDate.Year, parsedStartDate.Month, parsedStartDate.Day, 13, 0, 0);
			parsedEndDate = new DateTime(parsedEndDate.Year, parsedEndDate.Month, parsedEndDate.Day, 9, 0, 0);

			var checkRoom = _context.BookingRooms.Include(b => b.Booking)
				.Where(br => br.RoomId == roomId &&
				 (parsedStartDate < br.Booking.CheckOutDate &&
				  parsedEndDate > br.Booking.CheckInDate)
				 ).Any();

			if (checkRoom)
			{
				// Room already booked
				return Json(new { success = false, message = "The room is already booked" });

			}

			// Retrieve the room and price from the database
			var addedRoom = _context.Rooms.Include(p => p.Price)
				.Single(room => room.RoomId == roomId);

			// Add it to the shopping cart
			var cart = CartService.GetCart(_context, this.HttpContext);

			// Get all items in the cart
			var cartItems = cart.GetCartItems();

			// Check if the room already exists in the cart
			if (cartItems.Any(r => r.RoomId == roomId))
			{
				// Room already added
				return Json(new { success = false, message = "The room is already added" });
			}


			// If there are items in the cart, check the dates
			if (cartItems.Any())
			{
				var existingCartItem = cartItems.First();
				if (existingCartItem.CheckInDate != parsedStartDate || existingCartItem.CheckOutDate != parsedEndDate)
				{
					// Dates are different, return a JSON error response
					return Json(new { success = false, message = "The periods are different. Please select the same dates for all rooms." });
				}
			}
			try
			{
				cart.AddToCart(addedRoom, parsedStartDate, parsedEndDate);
			}
			catch (InvalidOperationException ex)
			{
				return Json(new { success = false, message = ex.Message });

			}


			// Return a JSON success response
			return Json(new { success = true, message = "Room added to cart successfully." });
		}
		catch (Exception ex)
		{
			return Json(new { success = false, message = ex.Message });
		}
	}



	[HttpPost]
	public ActionResult RemoveFromCart(int id)
	{
		string roomName = string.Empty;
		// Remove the item from the cart
		var cart = CartService.GetCart(_context, this.HttpContext);

		// Get the name of the room to display confirmation
		var item = _context.Carts.Include(r => r.Room)
			.SingleOrDefault(item => item.RecordId == id);
		if (item != null)
			roomName = item.Room.Name;

		// Remove from cart
		int itemCount = cart.RemoveFromCart(id);

		// Display the confirmation message
		var results = new BookingCartRemoveViewModel
		{
			Message = HtmlEncoder.Default.Encode(roomName) +
				" has been removed from your shopping cart.",
			CartTotal = cart.GetTotal(),
			DeleteId = id
		};
		return Json(results);
	}


	public ActionResult Index()
	{
		var cart = CartService.GetCart(_context, this.HttpContext);

		// Set up our ViewModel
		var viewModel = new BookingCartViewModel
		{
			CartItems = cart.GetCartItems(),
			CartTotal = cart.GetTotal()
		};

		// If there are cart items, return them to the view
		return View("/Views/Cart/Index.cshtml", viewModel);
	}


}

