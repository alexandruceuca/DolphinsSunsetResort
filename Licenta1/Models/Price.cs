using NuGet.Versioning;

namespace DolphinsSunsetResort.Models
{
	public class Price
	{
		public int PriceId { get; set; }

		public decimal BasePrice { get; set; }

		public decimal Discount { get; set; }

		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }

		public bool DiscountIsActive { get; set; }

		public virtual ICollection<Room> Rooms { get; set;}=new List<Room>();

		public virtual MenuItem MenuItem { get; set; }
	}
}
