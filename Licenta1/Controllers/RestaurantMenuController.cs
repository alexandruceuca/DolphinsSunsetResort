using DolphinsSunsetResort.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DolphinsSunsetResort.Controllers
{
    public class RestaurantMenuController : Controller
    {
        private readonly AuthDbContext _context;
        public RestaurantMenuController(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var categories = _context.MenuItemCategories
       .Include(c => c.MenuItems.Where(m=>m.ActiveYN==true))
           .ThenInclude(i => i.Price)
       .Include(c => c.MenuItems)
           .ThenInclude(i => i.Image);


            return View(await categories.ToListAsync());
        }


		// GET: MenuItems/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.MenuItems == null)
			{
				return NotFound();
			}

			var menuItem = await _context.MenuItems
				.Include(m => m.Image)
				.Include(m => m.MenuItemCategory)
				.Include(m => m.Price)
				.FirstOrDefaultAsync(m => m.MenuItemId == id);
			if (menuItem == null)
			{
				return NotFound();
			}

			return View(menuItem);
		}
	}
}
