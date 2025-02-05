using DolphinsSunsetResort.Models;

namespace DolphinsSunsetResort.Views.ViewsModel
{
	public class PaginatedMenuItemsListViewModel
	{
		public IEnumerable<MenuItem> MenuItems { get; set; }
		public int CurrentPage { get; set; }
		public int TotalPages { get; set; }
		public int PageSize { get; set; }
	}
}
