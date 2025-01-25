using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace DolphinsSunsetResort.Models
{
    public class Cart
    {
        [Key]
        public int RecordId { get; set; }
        public string CartId { get; set; }

        public int RoomId { get; set; }
        public decimal Price { get; set; }
        public int BreakfastCount { get; set; }

		public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual Room Room { get; set; }


    }
}
