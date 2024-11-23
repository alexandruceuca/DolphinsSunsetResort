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
            DateTime parsedStartDate = string.IsNullOrEmpty(checkInDate) ? DateTime.Today : DateTime.Parse(checkInDate);
            DateTime parsedEndDate = string.IsNullOrEmpty(checkOutDate) ? DateTime.Today : DateTime.Parse(checkOutDate);

            // Retrieve the room and price from the database
            var addedRoom = _context.Rooms.Include(p => p.Price)
                .Single(room => room.RoomId == roomId);

            // Add it to the shopping cart
            var cart = CartService.GetCart(_context, this.HttpContext);

            cart.AddToCart(addedRoom, parsedStartDate, parsedEndDate);
            // Redirect to the Cart page
            return RedirectToAction("Index", "BookingCart");
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
            return RedirectToAction("Index", "Home");
        }
    }

    [HttpPost]
    public ActionResult RemoveFromCart(int id)
    {
        string roomName=string.Empty;
		// Remove the item from the cart
		var cart = CartService.GetCart(_context,this.HttpContext);

        // Get the name of the room to display confirmation
        var item = _context.Carts.Include(r => r.Room)
            .SingleOrDefault(item => item.RecordId == id);
        if(item!=null)
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

