using DolphinsSunsetResort.Models;

namespace DolphinsSunsetResort.Views.ViewsModel
{
	public class RoomFilterViewModel
	{
		public List<Room> Rooms { get; set; }
		public DateTime? CheckInDate { get; set; }
		public DateTime? CheckOutDate { get; set;}
	}
}
