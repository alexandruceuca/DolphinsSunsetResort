using DolphinsSunsetResort.Areas.Identity.Data;

namespace DolphinsSunsetResort.Models
{
    public class EmailNotification : Notification
    {
        public string To { get; set; } = null!;

        public string? Cc { get; set; }

        public string Subject { get; set; } = null!;


        public EmailNotification()
        {

        }

        //BookingUserTemplate
        public EmailNotification(AplicationUser? user, string subject, string cc, int bookingNumber, decimal price, DateTime startdate, DateTime endDate)
        {
            this.To = user.Email;
            this.Subject = subject;
            this.Message = string.Format(BookingUserTemplate, user.FirstName + " " + user.LastName, bookingNumber, price, startdate.ToString("dd-MM-yyyy"), startdate.ToString("HH:mm:ss"), endDate.ToString("dd-MM-yyyy"),endDate.ToString("HH:mm:ss"));
            this.Cc = cc;

        }

        //CancelBookingUserTemplate
        public EmailNotification(AplicationUser? user, string subject, string cc, int bookingNumber, DateTime startdate, DateTime endDate)
        {
            this.To = user.Email;
            this.Subject = subject;
            this.Message = string.Format(CancelBookingUserTemplate, user.FirstName + " " + user.LastName, bookingNumber, startdate.ToString("dd-MM-yyyy"), startdate.ToString("HH:mm:ss"), endDate.ToString("dd-MM-yyyy"),endDate.ToString("HH:mm:ss"));
            this.Cc = cc;

        }

        //BookingManagerTemplate
        public EmailNotification(IList<AplicationUser> managers, AplicationUser? user, string subject, int bookingNumber, decimal price, DateTime startdate, DateTime endDate)
        {
            this.To = managers.FirstOrDefault().Email;
            this.Subject = subject;
            this.Message = string.Format(BookingManagerTemplate, user.FirstName + " " + user.LastName, bookingNumber, startdate.ToString("dd-MM-yyyy"), endDate.ToString("dd-MM-yyyy"), price);
            foreach (AplicationUser manager in managers)
            {
                this.Cc = this.Cc + manager.Email+", ";
            }
            this.Cc = this.Cc.TrimEnd(',', ' ');
        }

        //CleaningRoomTemplate
        public EmailNotification(IList<AplicationUser> cleaning, string subject, string rooms)
        {
            this.To = cleaning.FirstOrDefault().Email;
            this.Subject = subject;
            this.Message = string.Format(CleaningRoomTemplate, rooms);
         
        }


		//CleaningRoomCompletedTemplate
		public EmailNotification(IList<AplicationUser> reception, string subject, int room)
		{
			this.To = reception.FirstOrDefault().Email;
			this.Subject = subject;
			this.Message = string.Format(CleaningRoomCompletedTemplate, room);

		}


		
	}
}
