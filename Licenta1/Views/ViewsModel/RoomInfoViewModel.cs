using DolphinsSunsetResort.Models;

namespace DolphinsSunsetResort.Views.ViewsModel
{
    public class RoomInfoViewModel
    {
        public Room Room { get; set; }
		public DateTime? CheckInDate { get; set; }
		public DateTime? CheckOutDate { get; set; }
		public List<string> ImagePaths { get; set; }
    }
}
