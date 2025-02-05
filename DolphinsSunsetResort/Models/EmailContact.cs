using DolphinsSunsetResort.Areas.Identity.Data;

namespace DolphinsSunsetResort.Models
{
	public class EmailContact:Notification
	{
		public string To { get; set; } = null!;

		public string? Subject { get; set; } = null!;

		public string? Name { get; set; }

		public string? Email { get; set; }

		public string? PhoneNumber { get; set; }


		public EmailContact()
		{

		}

		public EmailContact(AplicationUser? user, string name,string email,string subject,string phoneNumber,string message)
		{
			this.To = user.Email;
			this.Email = email;
			this.Name = name;
			this.PhoneNumber= phoneNumber;
			this.Subject = subject;
			this.Message = string.Format(ContactFormTemplate, name, email, phoneNumber,message);

		}
	}
}
