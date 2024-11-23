using DolphinsSunsetResort.Areas.Identity.Data;

namespace DolphinsSunsetResort.Models
{
	public class BookingRoom
	{
		public int BookingRoomId { get; set; }

		public int BookingId { get; set; }
		
		public int RoomId { get; set; }

		public decimal Price { get; set; }

		public virtual Room Room { get; set; } = null;
		public virtual Booking Booking { get; set; } = null;
	}
}
