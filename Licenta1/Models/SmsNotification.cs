namespace DolphinsSunsetResort.Models
{
	public class SmsNotification:Notification
	{
		public string? PhoneNumber { get; set; }

		public SmsNotification() { }

		//next constructor for the times of notifications user mananeger reception roomservice
	}
}
