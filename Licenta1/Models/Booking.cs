using DolphinsSunsetResort.Areas.Identity.Data;

namespace DolphinsSunsetResort.Models
{
	public class Booking
	{
		public int BookingId { get; set; }
		public string UserId { get; set; }

		public DateTime BookingDate { get; set; }
		public DateTime CheckInDate { get; set; }
		public DateTime CheckOutDate { get; set;}

		public float TotalPrice { get; set; }

		public virtual AplicationUser AplicationUser { get; set; } = null;
		public virtual ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();
	}
}
