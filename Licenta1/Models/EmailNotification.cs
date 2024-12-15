using DolphinsSunsetResort.Areas.Identity.Data;

namespace DolphinsSunsetResort.Models
{
	public class EmailNotification:Notification
	{
		public string To { get; set; } = null!;

		public string? Cc { get; set; }

		public string Subject { get; set; } = null!;


		public EmailNotification()
		{

		}

        public EmailNotification(AplicationUser? user, string subject, string cc, int bookingNumber, decimal price, DateTime startdate, DateTime endDate)
        {
            this.To = user.Email;
            this.Subject = subject;
            this.Message = string.Format(BookingUserTemplate, user.FirstName+" "+user.LastName, bookingNumber, price, startdate.ToString("dd-MM-yyyy"), startdate.ToString("HH:mm:ss"), endDate.ToString("HH:mm:ss"));
            this.Cc = cc;

        }

        //same as SmsNotification
    }
}
