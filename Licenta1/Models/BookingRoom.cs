using Licenta1.Areas.Identity.Data;

namespace Licenta1.Models
{
	public class BookingRoom
	{
		public int BookingRoomId { get; set; }

		public int BookingId { get; set; }
		
		public int RoomId { get; set; }

		public virtual Room Room { get; set; } = null;
		public virtual Booking Booking { get; set; } = null;
	}
}
