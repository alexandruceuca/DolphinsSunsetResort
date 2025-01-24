using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DolphinsSunsetResort.ViewComponents
{
	public class MenuItemsListFilterViewComponent : ViewComponent
	{
		private readonly AuthDbContext _context;
		private readonly UserManager<AplicationUser> _userManager;

		public MenuItemsListFilterViewComponent(UserManager<AplicationUser> userManager, AuthDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}
		[Authorize(Roles = "Admin,Manager")]
		public async Task<IViewComponentResult> InvokeAsync(MenuItemsListViewModel filters, int page)
		{

			int pageSize = 5;
			var menuItems = _context.MenuItems.Include(n => n.Image).Include(n=>n.Price).Include(n=>n.MenuItemCategory).AsQueryable();

			// Apply filters if values are provided
			if (filters != null)
			{
				if (!string.IsNullOrEmpty(filters.Title))
				{
					menuItems = menuItems.Where(b => b.Name.Contains(filters.Title));
				}

				if(filters.CategoryId!=null && filters.CategoryId!=0)
				{
					menuItems = menuItems.Where(b =>b.CategoryId==filters.CategoryId);
				}

				if (filters.ActiveYN != null)
				{
					menuItems = menuItems.Where(b => b.ActiveYN == filters.ActiveYN);
				}


			}

			// Pagination
			var totalNews = await menuItems.CountAsync();
			var totalPages = (int)Math.Ceiling(totalNews / (double)pageSize);
			var paginatedMenuItems = await menuItems.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			var model = new PaginatedMenuItemsListViewModel
			{
				MenuItems = paginatedMenuItems,
				CurrentPage = page,
				TotalPages = totalPages,
				PageSize = pageSize
			};

			return View("/Views/Shared/Components/MenuItems/Index.cshtml", model);
		}
	}
}
