using DolphinsSunsetResort.Models;
using System.Net.Mail;

namespace DolphinsSunsetResort.Service
{
	public class EmailNewsService : INotificationService
	{
		public void Send(Notification notification)
		{
			EmailNews email_notification = new EmailNews();
			email_notification = (EmailNews)notification;

			try
			{
				var smtpClient = new SmtpClient("localhost")
				{
					Port = 25,
					// Credentials = new NetworkCredential("your_email@example.com", "your_password"),
					EnableSsl = false,
				};

				var mailMessage = new MailMessage
				{
					From = new MailAddress("dolphinsSunsetResort@test.com"),
					Subject = email_notification.Subject,
					Body = email_notification.Message,
					IsBodyHtml = false,
				};
				mailMessage.To.Add(email_notification.To.ToLower());
				if (email_notification.Cc != null && email_notification.Cc != string.Empty)
				{
					mailMessage.CC.Add(email_notification.Cc.ToLower());
				}

				smtpClient.Send(mailMessage);

				//Console.WriteLine("Email sent successfully!");
				notification.SentDate = DateTime.Now;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Exception caught: {ex.Message}");
			}
		}

	}
}
