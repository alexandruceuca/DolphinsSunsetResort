namespace DolphinsSunsetResort.Models
{
    public class Notification
    {
        public int Id { get; set; }

        public string Message { get; set; }

        public DateTime? SentDate { get; set; }

        public string BookingUserTemplate = "Hello {0},\n\nYour booking at the resort has been successfully confirmed!\n\nDetails:\nBooking Number: {1}\nPrice: {2}\nCheck-in Date: {3}\nCheck-out Date: {4}\n\nThank you for choosing us!";

        public string CancelBookingUserTemplate =
        "Hello {0},\n\nWe regret to inform you that your booking at the resort has been canceled.\n\nDetails:\n" +
        "Booking Number: {1}\n" +
        "Original Check-in Date: {2}\n" +
        "Original Check-out Date: {3}\n\n" +
        "If you have any questions or need assistance, please contact us.\n\n" +
        "Thank you,\nYour Booking Team";

        public string BookingManagerTemplate =
        "Hello, \n\nYou have a new booking. Here are the details:\n" +
        "Customer Name: {0}\n" +
        "Booking Number:{1}\n" +
        "Booking Period: {2} to {3}\n" +
        "Total Price: {4}\n\n" +
        "Please log in to the management system for more details.\n\n" +
        "Best regards,\nYour Booking System";

        public string CleaningRoomTemplate = "Hello,\n\nThe following rooms need cleaning:\n" +
        "{0}\n\n" +
        "Please ensure they are cleaned as soon as possible.\n\n" +
        "Best regards,\nYour Cleaning Management System";

		public string CleaningRoomCompletedTemplate = "Hello,\n\nThe following room have been successfully cleaned and are now ready:\n" +
		"{0}\n\n" +
		"Thank you for your attention.\n\n" +
		"Best regards,\nYour Cleaning Management System";

	}
}
