using DolphinsSunsetResort.Models;

namespace DolphinsSunsetResort.Service
{
	public class NotificationManager
	{
		public readonly Dictionary<Type, INotificationService> _services;

		public NotificationManager()
		{
			_services = new Dictionary<Type, INotificationService>
			{
		{ typeof(EmailNotification), new EmailService() },
		{ typeof(SmsNotification), new SmsService() },
		{ typeof(EmailContact), new EmailContactService() },
		{ typeof(EmailNews), new EmailNewsService() }
			 };
		}


		public void SendNotification(Notification notification)
		{
			var type = notification.GetType();
			if (_services.ContainsKey(type))
			{
				_services[type].Send(notification);
			}
			else
			{
				throw new NotSupportedException("Notification type not supported.");
			}
		}
	}
}

