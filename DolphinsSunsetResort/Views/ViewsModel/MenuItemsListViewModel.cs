using DolphinsSunsetResort.Models;

namespace DolphinsSunsetResort.Views.ViewsModel
{
	public class MenuItemsListViewModel
	{
		public List<MenuItem> MenuItems { get; set; }

		public string? Title { get; set; }

		public bool? ActiveYN { get; set; }

		public int? CategoryId { get; set; }
	}
}
