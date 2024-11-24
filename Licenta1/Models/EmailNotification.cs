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

		//same as SmsNotification
	}
}
