namespace DolphinsSunsetResort.Models
{
	public class Notification
	{
		public int Id { get; set; }

		public string Message { get; set; }

		public DateTime? SentDate { get; set; }

		public string BookingUserTemplate = "Hello {0},\n\nYour booking at the resort has been successfully confirmed!\n\nDetails:\nBooking Number: {1}\nPrice: {2}\nCheck-in Date: {3}\nCheck-out Date: {4}\n\nThank you for choosing us!";


		public string BookingManagerTemplate = "Hello,You have a new booking";
	}
}
