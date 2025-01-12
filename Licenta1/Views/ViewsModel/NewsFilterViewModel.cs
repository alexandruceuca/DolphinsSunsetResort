using DolphinsSunsetResort.Models;

namespace DolphinsSunsetResort.Views.ViewsModel
{
	public class NewsFilterViewModel
	{
		public List<News> News { get; set; }

		public string? Title { get; set; }
		public DateTime? StartDate { get; set; }

		public DateTime? EndDate { get; set; }


	}
}
