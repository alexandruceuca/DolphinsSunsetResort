using DolphinsSunsetResort.Models;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using MailKit.Net.Smtp;
using System.Net.Mail;

namespace DolphinsSunsetResort.Service
{
	public class EmailSender : IEmailSender
	{
		private readonly EmailSettings _emailSettings;

		public EmailSender(EmailSettings emailSettings)
		{
			_emailSettings = emailSettings;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var emailMessage = new MimeMessage();
			emailMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
			emailMessage.To.Add(new MailboxAddress("", email));
			emailMessage.Subject = subject;

			var bodyBuilder = new BodyBuilder { HtmlBody = htmlMessage };
			emailMessage.Body = bodyBuilder.ToMessageBody();

			using (var client = new MailKit.Net.Smtp.SmtpClient())
			{
				try
				{
					await client.ConnectAsync(_emailSettings.MailServer, _emailSettings.MailPort, _emailSettings.UseSsl);
					await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.Password);
					await client.SendAsync(emailMessage);
				}
				catch
				{
					throw;
				}
				finally
				{
					await client.DisconnectAsync(true);
				}
			}
		}
	}
}
