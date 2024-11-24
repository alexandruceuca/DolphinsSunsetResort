using DolphinsSunsetResort.Models;

namespace DolphinsSunsetResort.Service
{
	public interface INotificationService
	{
		public void Send(Notification notification);
	}
}
