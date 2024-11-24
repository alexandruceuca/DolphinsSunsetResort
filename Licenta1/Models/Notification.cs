namespace DolphinsSunsetResort.Models
{
	public class Notification
	{
		public int Id { get; set; }

		public string Message { get; set; }

		public DateTime? SentDate { get; set; }

		public string BookingUserTemplate = "Hello, you succefuly booked ...";

		public string BookingManagerTemplate = "Hello,You have a new booking";
	}
}
