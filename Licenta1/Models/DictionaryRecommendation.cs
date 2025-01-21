using DolphinsSunsetResort.Areas.Identity.Data;

namespace DolphinsSunsetResort.Models
{
	public class DictionaryRecommendation
	{
		public int RecommendationId { get; set; }

		public string RecommendationName { get;set; }

		public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();
	}
}
