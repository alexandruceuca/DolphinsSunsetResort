namespace Licenta1.Models
{
	public class Room
	{
		public int RoomId { get; set; }	
		public string Name { get; set; }
		public string Description { get; set; }
		public int PriceId { get; set; }
		public string RoomType { get; set; }
		public int Capacity { get; set; }

        public int Number { get; set; }

        public string RoomStatus { get; set; }

		public virtual Price Price { get; set; } = null;
		public virtual ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();
	}
}
