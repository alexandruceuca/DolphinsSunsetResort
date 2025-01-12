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
	public class NewsListFilterViewComponent : ViewComponent
	{
		private readonly AuthDbContext _context;
		private readonly UserManager<AplicationUser> _userManager;

		public NewsListFilterViewComponent(UserManager<AplicationUser> userManager, AuthDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}
		[Authorize(Roles = "Admin,Manager")]
		public async Task<IViewComponentResult> InvokeAsync(NewsFilterViewModel filters, int page)
		{

			int pageSize = 10;
			var news = _context.News.Include(n => n.Image).OrderByDescending(n => n.PublishedDate).AsQueryable();

			// Apply filters if values are provided
			if (filters != null)
			{
				if (!string.IsNullOrEmpty(filters.Title))
				{
					news = news.Where(b => b.Title.Contains(filters.Title));
				}

				news = news.Where(b => b.PublishedDate >= filters.StartDate);
				news = news.Where(b => b.PublishedDate <= filters.EndDate);

			}

			// Pagination
			var totalNews = await news.CountAsync();
			var totalPages = (int)Math.Ceiling(totalNews / (double)pageSize);
			var paginatedNews = await news.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

			var model = new PaginatedNewsListViewModel
			{
				News = paginatedNews,
				CurrentPage = page,
				TotalPages = totalPages,
				PageSize = pageSize
			};

			return View("/Views/Shared/Components/News/NewsList.cshtml", model);
		}
	}
}
