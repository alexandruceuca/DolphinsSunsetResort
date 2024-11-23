using DolphinsSunsetResort.Models;

namespace DolphinsSunsetResort.Views.ViewsModel
{
    public class BookingCartViewModel
    {
        public List<Cart> CartItems { get; set; }
        public decimal CartTotal { get; set; }
    }
}
