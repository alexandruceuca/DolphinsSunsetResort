namespace DolphinsSunsetResort.Models
{
	public class MenuItemCategory
	{
		public int MenuItemCategoryId { get; set; }

		public string MenuItemCategoryName { get;set; }


		public virtual ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

	}
}
