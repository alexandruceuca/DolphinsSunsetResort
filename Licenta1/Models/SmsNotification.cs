using DolphinsSunsetResort.Areas.Identity.Data;

namespace DolphinsSunsetResort.Models
{
	public class SmsNotification:Notification
	{
		public string? PhoneNumber { get; set; }

		public SmsNotification() { }

		//BookingUserTemplate
		public SmsNotification(AplicationUser? user, int bookingNumber, decimal price, DateTime startdate, DateTime endDate)
		{
			this.PhoneNumber = user.PhoneNumber;
			this.Message = string.Format(BookingUserTemplate, user.FirstName + " " + user.LastName, bookingNumber, price, startdate.ToString("dd-MM-yyyy"), startdate.ToString("HH:mm:ss"), endDate.ToString("HH:mm:ss"));


		}

		//CancelBookingUserTemplate
		public SmsNotification(AplicationUser? user, int bookingNumber, DateTime startdate, DateTime endDate)
		{
			this.PhoneNumber = user.PhoneNumber;
			this.Message = string.Format(CancelBookingUserTemplate, user.FirstName + " " + user.LastName, bookingNumber, startdate.ToString("dd-MM-yyyy"), startdate.ToString("HH:mm:ss"), endDate.ToString("HH:mm:ss"));


		}

		//BookingManagerTemplate
		public SmsNotification(IList<AplicationUser> managers, AplicationUser? user, int bookingNumber, decimal price, DateTime startdate, DateTime endDate)
		{
			this.PhoneNumber = managers.FirstOrDefault().PhoneNumber;
			this.Message = string.Format(BookingManagerTemplate, user.FirstName + " " + user.LastName, bookingNumber, startdate.ToString("dd-MM-yyyy"), startdate.ToString("HH:mm:ss"), endDate.ToString("HH:mm:ss"), price);
			
		}

		//CleaningRoomTemplate
		public SmsNotification(IList<AplicationUser> cleaning, string rooms)
		{
			this.PhoneNumber = cleaning.FirstOrDefault().PhoneNumber;
			this.Message = string.Format(CleaningRoomTemplate, rooms);

		}


		//CleaningRoomCompletedTemplate
		public SmsNotification(IList<AplicationUser> reception, int room)
		{
			this.PhoneNumber = reception.FirstOrDefault().PhoneNumber;
			this.Message = string.Format(CleaningRoomCompletedTemplate, room);

		}
	}
}
