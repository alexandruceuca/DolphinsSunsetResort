using DolphinsSunsetResort.Models;
using System.Text.Json;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace DolphinsSunsetResort.Service
{
	public class SmsService : INotificationService
	{
		public void Send(Notification notification)
		{
			SmsNotification sms_notification = new SmsNotification();
			sms_notification = (SmsNotification)notification;


			try
			{
				var twilioConfigString = File.ReadAllText("twilioConfig.json");
				var twilioConfig = JsonSerializer.Deserialize<TwilioConfig>(twilioConfigString);

				TwilioClient.Init(twilioConfig.AccountSID, twilioConfig.AuthToken);

				var message = MessageResource.Create(
					  body: sms_notification.Message,
					  from: new Twilio.Types.PhoneNumber("+18596671235"),
					  to: new Twilio.Types.PhoneNumber("+4" + sms_notification.PhoneNumber)

					  );

				notification.SentDate = DateTime.Now;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception caught: {ex.Message}");
			}



		}
	}
}
