﻿using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Dictionaries;

namespace DolphinsSunsetResort.Models
{
	public class Booking
	{
		public int BookingId { get; set; }
		public string UserId { get; set; }

		public DateTime BookingDate { get; set; }
		public DateTime CheckInDate { get; set; }
		public DateTime CheckOutDate { get; set;}

		public decimal TotalPrice { get; set; }
        public BookingStatus Status { get; set; }

		public int Rating { get; set; }

		public int RecommendationId { get; set; }

        public virtual AplicationUser AplicationUser { get; set; } = null;
		public virtual ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();

		public virtual DictionaryRecommendation DictionaryRecommendation { get; set; }
	}
}
