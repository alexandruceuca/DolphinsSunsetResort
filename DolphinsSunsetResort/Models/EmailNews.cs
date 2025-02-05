using DolphinsSunsetResort.Areas.Identity.Data;

namespace DolphinsSunsetResort.Models
{
	public class EmailNews : Notification
	{
		public string To { get; set; } = null!;

		public string? Cc { get; set; }

		public string Subject { get; set; } = null!;

		public EmailNews()
		{

		}

		//BookingUserTemplate
		public EmailNews(AplicationUser? user, string subject, string cc, string message)
		{
			this.To = user.Email;
			this.Subject = subject;
			this.Message = string.Format(NewsTemplate, user.FirstName + " " + user.LastName, message);
			this.Cc = cc;

		}
	}
}
