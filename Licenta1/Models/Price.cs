using NuGet.Versioning;

namespace Licenta1.Models
{
	public class Price
	{
		public int PriceId { get; set; }

		public float BasePrice { get; set; }

		public float Discount { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public bool DiscountIsActive { get; set; }

		public virtual ICollection<Room> Rooms { get; set;}=new List<Room>();
	}
}
