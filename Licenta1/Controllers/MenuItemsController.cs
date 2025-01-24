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

namespace DolphinsSunsetResort.Controllers
{
    public class MenuItemsController : Controller
    {
        private readonly AuthDbContext _context;

        public MenuItemsController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: MenuItems
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.MenuItems.Include(m => m.Image).Include(m => m.MenuItemCategory).Include(m => m.Price);
            return View(await authDbContext.ToListAsync());
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

        // GET: MenuItems/Create
        public IActionResult Create()
        {
            ViewData["ImageId"] = new SelectList(_context.AppFiles, "Id", "Id");
            ViewData["CategoryId"] = new SelectList(_context.MenuItemCategories, "MenuItemCategoryId", "MenuItemCategoryName");
            ViewData["PrinceId"] = new SelectList(_context.Prices, "PriceId", "BasePrice");
            return View();
        }


        [HttpPost]

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
                            ModelState.AddModelError("File", "The file is too large.");
                        }
                    }
                }
                if (menuItem.Price.Discount > 100)
                {
                    ModelState.AddModelError("Discount", "The Discount is too large.");
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
                                ModelState.AddModelError("File", "The file is too large.");
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
                if (menuItem.PriceId != null)
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
