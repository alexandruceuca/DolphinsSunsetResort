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

		public async Task<IActionResult> Details(int id)
		{
			var news = await _context.News.Include(n => n.Image).FirstOrDefaultAsync(n => n.Id == id);
			if (news == null)
			{
				return NotFound();
			}

			return View("/Views/News/Details.cshtml", news);
		}


		[Authorize(Roles = "Admin,Manager")]
		public IActionResult Edit(int id)
		{
			var news = _context.News.Include(n => n.Image).FirstOrDefault(n => n.Id == id);
			if (news == null)
			{
				return NotFound();
			}

			return View("/Views/News/Edit.cshtml", news);
		}

		[Authorize(Roles = "Admin,Manager")]
		[HttpPost]
		public async Task<IActionResult> EditSave(News model, IFormFile FileUpload)
		{
			News news;

			if (model.Id == 0)
			{
				// Create new news item if no ID is provided
				news = new News();
				news.PublishedDate = DateTime.Now;
				_context.News.Add(news);
			}
			else
			{
				// Edit existing news item
				news = await _context.News.FindAsync(model.Id);
				if (news == null)
				{
					return NotFound();
				}

			}
			// Update the title and content
			news.Title = model.Title;
			news.Content = model.Content;
			bool removeImage = Request.Form["RemoveImage"] == "on";
			try
			{
				// Handle the file upload
				if (FileUpload != null)
				{
					using (var memoryStream = new MemoryStream())
					{
						await FileUpload.CopyToAsync(memoryStream);
						// Upload the file if less than 2 MB
						if (memoryStream.Length < 2097152)
						{
							// Generate a random file name for security
							string randomFileName = $"{Guid.NewGuid().ToString()}.jpg";
							var file = new AppFile
							{
								FileName = randomFileName,
								Content = memoryStream.ToArray(),
								ContentType = FileUpload.ContentType
							};

							_context.AppFiles.Add(file);
							await _context.SaveChangesAsync();

							// Link the uploaded file to the news item
							news.ImageId = file.Id;
						}
						else
						{
							ModelState.AddModelError("File", "The file is too large.");
						}
					}
				}
				else if (removeImage)
				{
					// If the user has selected to remove the image, set the ImageId to null
					news.ImageId = null;
				}

				await _context.SaveChangesAsync();
				return RedirectToAction("Index", "News");
			}
			catch (Exception ex)
			{
				return View(model);
			}
		}



		[Authorize(Roles = "Admin,Manager")]
		[HttpPost]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{

			var news = await _context.News.FindAsync(id);


			if (news == null)
			{
				return NotFound();
			}

			try
			{
				_context.News.Remove(news);
				await _context.SaveChangesAsync();
				return Json(new { success = true });
			}
			catch (Exception ex)
			{
				return Json(new { success = false });
			}

		}


		[Authorize(Roles = "Admin,Manager")]
		public IActionResult Add()
		{

			return View("/Views/News/Edit.cshtml");
		}


		[HttpPost]
		[Authorize(Roles = "Admin,Manager")]
		public IActionResult AddSave()
		{

			return View("/Views/News/Edit.cshtml");
		}

	}
}
