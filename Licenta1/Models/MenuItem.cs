namespace DolphinsSunsetResort.Models
{
	public class MenuItem
	{
		public int MenuItemId { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public int? ImageId { get; set; }

		public int CategoryId { get; set; }

		public int PriceId { get; set; }
		public AppFile? Image { get; set; }

		public MenuItemCategory MenuItemCategory { get; set; }

		public Price Price { get; set; }

	}
}
