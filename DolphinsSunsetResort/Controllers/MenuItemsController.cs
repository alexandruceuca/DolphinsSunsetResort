using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Models;
using DolphinsSunsetResort.Migrations;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace DolphinsSunsetResort.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly AuthDbContext _context;

        public MenuItemsController(AuthDbContext context)
        {
            _context = context;
        }
		[Authorize(Roles = "Admin,Manager")]
		// GET: MenuItems
		public async Task<IActionResult> Index(string titleFilter, bool? activeYN, int  categoryId, int page)
        {
          

			ViewData["CategoryId"] = new SelectList(_context.MenuItemCategories, "MenuItemCategoryId", "MenuItemCategoryName");

			var menuItemsFilters = new MenuItemsListViewModel
			{
				Title = titleFilter,
				ActiveYN = activeYN,
				CategoryId = categoryId,
			};


			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{

				return ViewComponent("MenuItemsListFilter", new { filters = menuItemsFilters, page = page });
			}


			return View();
			
        }
		[Authorize(Roles = "Admin,Manager")]
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

		// GET: MenuItems/Create
		[Authorize(Roles = "Admin,Manager")]
		public IActionResult Create()
        {
            ViewData["ImageId"] = new SelectList(_context.AppFiles, "Id", "Id");
            ViewData["CategoryId"] = new SelectList(_context.MenuItemCategories, "MenuItemCategoryId", "MenuItemCategoryName");
            ViewData["PrinceId"] = new SelectList(_context.Prices, "PriceId", "BasePrice");
            return View();
        }


        [HttpPost]
		[Authorize(Roles = "Admin,Manager")]
		public async Task<IActionResult> Create(MenuItem menuItem, IFormFile FileUpload)
        {
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
                            menuItem.ImageId = file.Id;
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "The file is too large.");
							return View("Error");
						}
                    }
                }
                if (menuItem.Price.Discount > 100)
                {
                    ModelState.AddModelError(string.Empty, "The Discount is too large.");
					ViewData["CategoryId"] = new SelectList(_context.MenuItemCategories, "MenuItemCategoryId", "MenuItemCategoryName", menuItem.CategoryId);
					return View(menuItem);
				}

				if (menuItem.Price.StartDate > menuItem.Price.EndDate)
				{
					ModelState.AddModelError(string.Empty, "Start Date cannot be after End Date. ");
					ViewData["CategoryId"] = new SelectList(_context.MenuItemCategories, "MenuItemCategoryId", "MenuItemCategoryName", menuItem.CategoryId);
					return View(menuItem);
				}

				if (menuItem.Price.StartDate < DateTime.Today || menuItem.Price.StartDate < DateTime.Today)
				{
					ModelState.AddModelError(string.Empty, "Date cannot be before today.");
					ViewData["CategoryId"] = new SelectList(_context.MenuItemCategories, "MenuItemCategoryId", "MenuItemCategoryName", menuItem.CategoryId);
					return View(menuItem);
				}

			}
            catch (Exception ex)
            {

                ViewData["ErrorMessage"] = "An unexpected error occurred. Please try again later.";
                return View("Error");
            }



			_context.Add(menuItem);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

		// GET: MenuItems/Edit/5
		[Authorize(Roles = "Admin,Manager")]
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.MenuItems == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems.Include(m => m.Price).Include(m => m.Image).FirstOrDefaultAsync(m => m.MenuItemId == id);
            if (menuItem == null)
            {
                return NotFound();
            }
            ViewData["ImageId"] = new SelectList(_context.AppFiles, "Id", "Id", menuItem.ImageId);
            ViewData["CategoryId"] = new SelectList(_context.MenuItemCategories, "MenuItemCategoryId", "MenuItemCategoryName", menuItem.CategoryId);

            return View(menuItem);
        }

		[Authorize(Roles = "Admin,Manager")]
		[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, MenuItem menuItem, IFormFile FileUpload)
        {
            if (id != menuItem.MenuItemId)
            {
                return NotFound();
            }

            try
            {
                bool removeImage = Request.Form["RemoveImage"] == "on";
                if (removeImage && menuItem.ImageId != null)
                {
                    var appFile = _context.AppFiles.FirstOrDefault(f => f.Id == menuItem.ImageId);
                    _context.AppFiles.Remove(appFile);
                    menuItem.ImageId = null;
                }
                else
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
                                if (menuItem.ImageId == null)
                                {
                                    var file = new AppFile
                                    {
                                        FileName = randomFileName,
                                        Content = memoryStream.ToArray(),
                                        ContentType = FileUpload.ContentType
                                    };

                                    _context.AppFiles.Add(file);
                                    await _context.SaveChangesAsync();

                                    // Link the uploaded file to the news item
                                    menuItem.ImageId = file.Id;
                                }
                                else
                                {
                                    menuItem.Image.FileName = randomFileName;
                                    menuItem.Image.Content = memoryStream.ToArray();
                                    menuItem.Image.ContentType = FileUpload.ContentType;


                                }
                            }
                            else
                            {
                                ModelState.AddModelError(string.Empty, "The file is too large.");
								ViewData["CategoryId"] = new SelectList(_context.MenuItemCategories, "MenuItemCategoryId", "MenuItemCategoryName", menuItem.CategoryId);
								return View(menuItem);
							}
							if (menuItem.Price.Discount > 100)
							{
								ModelState.AddModelError(string.Empty, "The Discount is too large.");
								ViewData["CategoryId"] = new SelectList(_context.MenuItemCategories, "MenuItemCategoryId", "MenuItemCategoryName", menuItem.CategoryId);
								return View(menuItem);
							}

							if (menuItem.Price.StartDate > menuItem.Price.EndDate)
							{
								ModelState.AddModelError(string.Empty, "Start Date cannot be after End Date. ");
								ViewData["CategoryId"] = new SelectList(_context.MenuItemCategories, "MenuItemCategoryId", "MenuItemCategoryName", menuItem.CategoryId);
								return View(menuItem);
							}

							if (menuItem.Price.StartDate < DateTime.Today || menuItem.Price.StartDate < DateTime.Today)
							{
								ModelState.AddModelError(string.Empty, "Date cannot be before today.");
								ViewData["CategoryId"] = new SelectList(_context.MenuItemCategories, "MenuItemCategoryId", "MenuItemCategoryName", menuItem.CategoryId);
								return View(menuItem);
							}
						}
                    }
                }

                MenuItem update = _context.MenuItems.Include(m => m.Price).FirstOrDefault(m => m.MenuItemId == id);

                if (update == null)
                {
                    ModelState.AddModelError("Update", "An unexpected error occurred. Please try again later.");
                }


                update.Name = menuItem.Name;
                update.Description = menuItem.Description;
                update.Quantity= menuItem.Quantity;
                update.ImageId = menuItem.ImageId;
                if (menuItem.ImageId != null)
                    update.Image = menuItem.Image;
                update.ActiveYN= menuItem.ActiveYN;
                update.CategoryId= menuItem.CategoryId;
                update.Price.BasePrice = menuItem.Price.BasePrice;
                update.Price.Discount = menuItem.Price.Discount;
                update.Price.DiscountIsActive = menuItem.Price.DiscountIsActive;
                update.Price.StartDate = menuItem.Price.StartDate;
                update.Price.EndDate = menuItem.Price.EndDate;

                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                if (!MenuItemExists(menuItem.MenuItemId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

		
			return RedirectToAction(nameof(Index));


        }
		[Authorize(Roles = "Admin,Manager")]
		// GET: MenuItems/Delete/5
		public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.MenuItems == null)
            {
                return NotFound();
            }

            var menuItem = await _context.MenuItems
                .Include(m => m.Image)
                .Include(m => m.Price)
                .Include(m => m.MenuItemCategory)
                .FirstOrDefaultAsync(m => m.MenuItemId == id);
            if (menuItem == null)
            {
                return NotFound();
            }

            return View(menuItem);
        }

		[Authorize(Roles = "Admin,Manager")]
		// POST: MenuItems/Delete/5
		[HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.MenuItems == null)
            {
                return Problem("Entity set 'AuthDbContext.MenuItems' is null.");
            }

            var menuItem = await _context.MenuItems.FindAsync(id);

            if (menuItem != null)
            {
                // Remove related AppFile if it exists
                if (menuItem.ImageId.HasValue)
                {
                    var appFile = await _context.AppFiles.FindAsync(menuItem.ImageId.Value);
                    if (appFile != null)
                    {
                        _context.AppFiles.Remove(appFile);
                    }
                }

                // Remove related Price if it exists
                if (menuItem.PriceId != 0)
                {
                    var price = await _context.Prices.FindAsync(menuItem.PriceId);
                    if (price != null)
                    {
                        _context.Prices.Remove(price);
                    }
                }

                // Remove the MenuItem
                _context.MenuItems.Remove(menuItem);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        private bool MenuItemExists(int id)
        {
            return (_context.MenuItems?.Any(e => e.MenuItemId == id)).GetValueOrDefault();
        }
    }
}
