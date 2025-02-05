using DolphinsSunsetResort.Models;

namespace DolphinsSunsetResort.Views.ViewsModel
{
	public class PaginatedNewsListViewModel
	{
		public IEnumerable<News> News { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int PageSize { get; set; }
	}
}
