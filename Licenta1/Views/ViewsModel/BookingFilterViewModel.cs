using DolphinsSunsetResort.Dictionaries;
using DolphinsSunsetResort.Models;
namespace DolphinsSunsetResort.Views.ViewsModel
{
	public class BookingFilterViewModel
	{
		public List<Booking> Bookings { get; set; }
		public DateTime? CheckInDate { get; set; }
		public DateTime? CheckOutDate { get; set; }

		public string? EmailFilter { get; set; }

		public string? PhoneNumberFilter { get; set; }

		public int? BookingIdFilter { get; set; }

		public BookingStatus? Status { get; set; }
	}
}
