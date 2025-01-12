using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Models;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DolphinsSunsetResort.Controllers
{
	public class NewsController : Controller
	{
		private readonly AuthDbContext _context;
		private readonly UserManager<AplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public NewsController(AuthDbContext context, UserManager<AplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public async Task<IActionResult> Index(string titleFilter, string startDate, string endDate, int page)
		{
			// Parse the dates
			DateTime parsedStartDate = string.IsNullOrEmpty(startDate) ? DateTime.MinValue : DateTime.Parse(startDate);
			DateTime parsedEndDate = string.IsNullOrEmpty(endDate) ? DateTime.MaxValue : DateTime.Parse(endDate);



			var newsFilters = new NewsFilterViewModel
			{
				Title = titleFilter,
				StartDate = parsedStartDate,
				EndDate = parsedEndDate,
			};


			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{

				return ViewComponent("NewsListFilter", new { filters = newsFilters, page = page });
			}


			return View("/Views/News/Index.cshtml");
		}

		public  IActionResult Details(News news)
		{

			return View("/Views/News/Details.cshtml",news);
		}




		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Manager")]
		public async Task<IActionResult> Create(News news, IFormFile imageFile)
		{
			if (ModelState.IsValid)
			{
				if (imageFile != null)
				{
					var filePath = Path.Combine("wwwroot/images", imageFile.FileName);
					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await imageFile.CopyToAsync(stream);
					}
					news.ImageUrl = $"/images/{imageFile.FileName}";
				}

				news.PublishedDate = DateTime.Now;
				_context.Add(news);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(news);
		}

	}
}
