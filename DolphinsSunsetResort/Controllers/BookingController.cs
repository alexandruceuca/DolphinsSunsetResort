﻿using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Dictionaries;
using DolphinsSunsetResort.Models;
using DolphinsSunsetResort.Service;
using DolphinsSunsetResort.Utils;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Security.Claims;

namespace DolphinsSunsetResort.Controllers
{
	public class BookingController : Controller
	{
		private readonly AuthDbContext _context;
		private readonly UserManager<AplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public BookingController(AuthDbContext context, UserManager<AplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		public async Task<IActionResult> Index(string checkInDate, string checkOutDate, BookingStatus bookingStatus, int page)
		{
			// Check if the bookingStatus is not set 
			if (string.IsNullOrEmpty(Request.Query["bookingStatus"]))
			{
				bookingStatus = BookingStatus.None;
			}

			var (parsedStartDate, parsedEndDate) = UtilsDate.ParseAndSetBookingDates(checkInDate, checkOutDate);

			//// Parse the dates
			//DateTime parsedStartDate = string.IsNullOrEmpty(checkInDate) ? DateTime.MinValue : DateTime.Parse(checkInDate);
			//DateTime parsedEndDate = string.IsNullOrEmpty(checkOutDate) ? DateTime.MaxValue : DateTime.Parse(checkOutDate);

			////Set time
			//parsedStartDate = new DateTime(parsedStartDate.Year, parsedStartDate.Month, parsedStartDate.Day, 13, 0, 0);
			//parsedEndDate = new DateTime(parsedEndDate.Year, parsedEndDate.Month, parsedEndDate.Day, 9, 0, 0);


			var bookingFilters = new BookingFilterViewModel
			{
				CheckInDate = parsedStartDate,
				CheckOutDate = parsedEndDate,
				Status = bookingStatus
			};
			// Get all enum values except "None"
			var bookingStatusList = Enum.GetValues(typeof(BookingStatus))
				.Cast<BookingStatus>()
				.Where(status => status != BookingStatus.None && status != BookingStatus.NoShow)
				.ToList();

			// Pass the filtered enum values to the view
			ViewBag.bookingStatus = bookingStatusList;
			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				return ViewComponent("BookingFilter", new { filterBookings = bookingFilters, page = page });
			}


			return View("/Views/Booking/Index.cshtml");
		}

		[HttpPost]
		public async Task<IActionResult> CancelBooking(int bookingId)
		{
			var booking = _context.Bookings.Include(b => b.AplicationUser)
											.FirstOrDefault(b => b.BookingId == bookingId);

			if (booking != null && booking.Status == BookingStatus.Confirmed)
			{
				booking.Status = BookingStatus.Cancelled;
				_context.SaveChanges();
				//send confirmation email
				var notificationEmail = new EmailNotification(booking.AplicationUser, "Cancel Booking", "", booking.BookingId, booking.CheckInDate, booking.CheckOutDate);

				var manager = new NotificationManager();
				manager.SendNotification(notificationEmail);

				_context.EmailNotification.Add(notificationEmail);

				if (booking.AplicationUser.PhoneNumber != null)
				{
					var notificationSms = new SmsNotification(booking.AplicationUser, booking.BookingId, booking.CheckInDate, booking.CheckOutDate);
					manager.SendNotification(notificationSms);

					_context.SmsNotification.Add(notificationSms);


				}

				await _context.SaveChangesAsync();
				return Json(new { success = true });
			}
			return Json(new { success = false, message = "Booking not found or already cancelled." });
		}

		[HttpPost]
		[Authorize(Roles = "Admin,Manager,Reception")]
		public IActionResult CheckInBooking(int bookingId)
		{
			// Retrieve the booking from the database
			var booking = _context.Bookings
				.Include(b => b.BookingRooms)
				.ThenInclude(br => br.Room)
				.FirstOrDefault(b => b.BookingId == bookingId);

			if (booking != null && booking.Status == BookingStatus.Confirmed)
			{
				// Update the booking status
				booking.Status = BookingStatus.CheckIn;

				// Update the status of all rooms associated with this booking
				foreach (var bookingRoom in booking.BookingRooms)
				{
					if (bookingRoom.Room != null)
					{
						bookingRoom.Room.RoomStatus = RoomStatus.Occupied;
					}
				}

				// Save changes to the database
				_context.SaveChanges();

				return Json(new { success = true });
			}

			return Json(new { success = false, message = "Booking not found or already checked in." });
		}

		[HttpPost]
		[Authorize(Roles = "Admin,Manager,Reception")]
		public async Task<IActionResult> CheckOutBooking(int bookingId)
		{
			string roomsNumber = string.Empty;
			// Retrieve the booking from the database
			var booking = _context.Bookings
				.Include(b => b.BookingRooms)
				.ThenInclude(br => br.Room)
				.FirstOrDefault(b => b.BookingId == bookingId);

			if (booking != null && booking.Status == BookingStatus.CheckIn)
			{
				// Update the booking status
				booking.Status = BookingStatus.CheckOut;

				// Update the status of all rooms associated with this booking
				foreach (var bookingRoom in booking.BookingRooms)
				{
					if (bookingRoom.Room != null)
					{
						bookingRoom.Room.RoomStatus = RoomStatus.NeedsCleaning;
						roomsNumber = roomsNumber + bookingRoom.Room.Number + " ,";
					}
				}
				roomsNumber = roomsNumber.TrimEnd(',', ' ');
				// Save changes to the database
				_context.SaveChanges();

				// Get users in the role
				var cleaningRole = _roleManager.FindByNameAsync("RoomCleaner");
				if (cleaningRole == null)
				{
					return NotFound("The 'RoomCleaner' role does not exist.");
				}
				var usersCleaning = await _userManager.GetUsersInRoleAsync("RoomCleaner");


				//send notification for cleaning
				var notification = new EmailNotification(usersCleaning, "New Rooms", roomsNumber);
				var manager = new NotificationManager();
				manager.SendNotification(notification);


				_context.EmailNotification.Add(notification);
				await _context.SaveChangesAsync();
				return Json(new { success = true });
			}

			return Json(new { success = false, message = "Booking not found or already checked in." });
		}

		[Authorize(Roles = "Admin,Manager,Reception")]
		public IActionResult DetailBooking(int bookingId)
		{
			var bookingDetail = _context.Bookings.Include(a => a.AplicationUser)
												.Include(r => r.BookingRooms)
													.ThenInclude(rm => rm.Room)
												.FirstOrDefault(b => b.BookingId == bookingId);
			var totalBreakfastCount = 0;
			var numberBookings = 0;

			foreach (BookingRoom b in bookingDetail.BookingRooms)
			{
				totalBreakfastCount += b.BreakfastCount;
			}

			var bookings = _context.Bookings.Where(b=>b.UserId==bookingDetail.UserId && b.Status==Dictionaries.BookingStatus.CheckOut);

			ViewBag.TotalBreakfastCount = totalBreakfastCount;
			ViewBag.NumberBookings = numberBookings+bookings.Count();

			return View(bookingDetail);
		}



		[HttpPost]
		public async Task<IActionResult> RateBooking(int bookingId, int rating, Recommendation recommendation)
		{
			var booking = await _context.Bookings.FindAsync(bookingId);
			if (booking != null && rating <= 5 && rating >= 1)
			{
				// Set the rating
				booking.Rating = rating;

				// Map the enum to its database ID
				var recommendationEntity = await _context.DictionaryRecommendations
					.FirstOrDefaultAsync(r => r.RecommendationName == recommendation.ToString());

				if (recommendationEntity != null)
				{
					booking.RecommendationId = recommendationEntity.RecommendationId;
				}

				await _context.SaveChangesAsync();
				return Json(new { success = true });
			}

			return Json(new { success = false });
		}


		public async Task<IActionResult> GetBookingRating(int bookingId)
		{
			var booking = await _context.Bookings
				.Include(b => b.DictionaryRecommendation)
				.FirstOrDefaultAsync(b => b.BookingId == bookingId);

			if (booking == null)
			{
				return Json(new { success = false, message = "Booking not found." });
			}

			var response = new
			{
				rating = booking.Rating,
				recommendation = booking.RecommendationId
			};

			return Json(response);
		}


	}
}
