using DolphinsSunsetResort.Models;

namespace DolphinsSunsetResort.Views.ViewsModel
{
    public class BookingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
		public DateTime CheckInDate { get; set; }
		public DateTime CheckOutDate { get; set; }
		public decimal CartTotal { get; set; }
    }
}
