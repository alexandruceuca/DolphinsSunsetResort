using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using System.Web;
using System.Net.Http;

namespace DolphinsSunsetResort.Service
{
	public class CartService
	{
		private readonly AuthDbContext _context;
		private readonly HttpContext _httpContext;
		string BookingCartId { get; set; }
		public const string CartSessionKey = "CartId";

		public CartService(AuthDbContext context, HttpContext httpContext)
		{
			_context = context;
			_httpContext = httpContext;

		}
		public static CartService GetCart(AuthDbContext context, HttpContext httpContext)
		{
			var cart = new CartService(context, httpContext);
			cart.BookingCartId = cart.GetCartId(httpContext);
			return cart;
		}

		private string GetCartId(HttpContext httpContext)
		{
			var cartSessionKey = httpContext.Session.GetString(CartSessionKey);
			var userIdentityName = httpContext.User.Identity.Name;
			if (cartSessionKey == null)
			{
				if (!string.IsNullOrWhiteSpace(userIdentityName))
				{
					httpContext.Session.SetString(CartSessionKey, userIdentityName);
				}
				else
				{
					// Generate a new random GUID using System.Guid class
					Guid tempCartId = Guid.NewGuid();
					// Send tempCartId back to client as a cookie
					httpContext.Session.SetString(CartSessionKey, tempCartId.ToString());
				}
			}
			return httpContext.Session.GetString(CartSessionKey);
		}

		public void AddToCart(Room room, DateTime checkInDate, DateTime checkOutDate,int breakfastCount)
		{

			var cartItem = new Cart
			{
				RoomId = room.RoomId,
				CartId = BookingCartId,
				CheckInDate = checkInDate,
				CheckOutDate = checkOutDate,
				CreatedDate = DateTime.Now,
				Room = room,
				Price = CalculatePrice(room.Price, checkInDate, checkOutDate),
				BreakfastCount = breakfastCount
			};
			_context.Carts.Add(cartItem);


			// Save changes
			_context.SaveChanges();
		}

		public int RemoveFromCart(int id)
		{
			// Get the cart
			var cartItem = _context.Carts.SingleOrDefault(
				cart => cart.CartId == BookingCartId
				&& cart.RecordId == id);

			int itemCount = 0;

			if (cartItem != null)
			{
				_context.Carts.Remove(cartItem);

				// Save changes
				_context.SaveChanges();
			}
			return itemCount;
		}

		public void EmptyCart()
		{
			var cartItems = _context.Carts.Where(
				cart => cart.CartId == BookingCartId);

			foreach (var cartItem in cartItems)
			{
				_context.Carts.Remove(cartItem);
			}
			// Save changes
			_context.SaveChanges();
		}
		public List<Cart> GetCartItems()
		{
			return _context.Carts.Include(r => r.Room).
				Where(cart => cart.CartId == BookingCartId).ToList();

		}

		private decimal CalculatePrice(Price price, DateTime checkInDate, DateTime checkOutDate)
		{
			int totalDays = checkOutDate.Day - checkInDate.Day;
			decimal totalPrice = 0;

			for (int day = 0; day < totalDays; day++)
			{
				var date = checkInDate.AddDays(day);

				// Check if the price is applicable to the current date
				if (price.StartDate <= date && price.EndDate >= date)
				{
					decimal dailyPrice = price.BasePrice;
					if (price.DiscountIsActive)
						dailyPrice -= (dailyPrice * price.Discount / 100); // Apply discount if active

					totalPrice += dailyPrice;
				}
				else
				{
					totalPrice += price.BasePrice;
				}
			}

			return totalPrice;
		}

		public decimal GetTotal()
		{

			decimal? total = (from cartItems in _context.Carts
							  where cartItems.CartId == BookingCartId
							  select cartItems.Price).Sum();

			return total ?? decimal.Zero;
		}

		public int GetCount()
		{
			// Get the count of each item in the cart and sum them up
			int? count = (from cartItems in _context.Carts
						  where cartItems.CartId == BookingCartId
						  select cartItems.RoomId).Sum();
			// Return 0 if all entries are null
			return count ?? 0;
		}
		public bool IsRoomBooked(int roomId, DateTime checkInDate, DateTime checkOutDate)
		{
			// Get bookings that have a room with the given roomId
			var checkRoom = _context.Bookings
				.Include(b => b.BookingRooms)
				.AsQueryable();

			// Check if any booking already exists for this room within the date range
			var roomIsBooked = checkRoom.Any(b =>
				b.BookingRooms.Any(br => br.RoomId == roomId) && 
																
				!(checkOutDate <= b.CheckInDate || checkInDate >= b.CheckOutDate) 
				&& b.Status==Dictionaries.BookingStatus.Confirmed
			);

			return roomIsBooked;
		}

		public int CreateBooking(Booking booking)
		{
			decimal orderTotal = 0;
			var cartItems = GetCartItems();
			// Iterate over the items in the cart, 
			// adding the order details for each
			foreach (var item in cartItems)
			{
				if (IsRoomBooked(item.RoomId, cartItems.First().CheckInDate, cartItems.First().CheckOutDate))
				{
					throw new InvalidOperationException($"Room {item.RoomId} is already booked for the selected dates.");
				}
				var bookingDetail = new BookingRoom
				{
					RoomId = item.RoomId,
					BookingId = booking.BookingId,
					Price = item.Price,
					BreakfastCount =item.BreakfastCount,
				};
				// Set the order total of the booking cart
				orderTotal += item.Price;

				_context.BookingRooms.Add(bookingDetail);

			}
			// Set the booking's total to the orderTotal checkin and checkout
			booking.TotalPrice = orderTotal;
			booking.CheckInDate = cartItems.First().CheckInDate;
			booking.CheckOutDate = cartItems.First().CheckOutDate;
			booking.Status = Dictionaries.BookingStatus.Confirmed;

			
			_context.SaveChanges();
		
			EmptyCart();
		
			return booking.BookingId;
		}

		// When a user has logged in, migrate their booking cart to
		// be associated with their username
		public void MigrateCart(string userName)
		{
			var bookingCart = _context.Carts.Where(
				c => c.CartId == BookingCartId);

			foreach (Cart item in bookingCart)
			{
				item.CartId = userName;
			}
			_context.SaveChanges();
		}
	}

}