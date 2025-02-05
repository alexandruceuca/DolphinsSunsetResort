using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Models;
using DolphinsSunsetResort.Service;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DolphinsSunsetResort.Controllers
{
	public class ContactController : Controller
	{
		private readonly AuthDbContext _context;
		private readonly UserManager<AplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public ContactController(AuthDbContext context, UserManager<AplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}
		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Submit(EmailContact model)
		{


			var managerRole = await _roleManager.FindByNameAsync("Manager");
			if (managerRole == null)
			{
				return NotFound("The 'Manager' role does not exist.");
			}

			// Get users in the role
			var usersInManagerRole = await _userManager.GetUsersInRoleAsync("Manager");

			var contactMessage = new EmailContact
			(usersInManagerRole.FirstOrDefault(),
				 model.Name,
				 model.Email,
				 model.Subject,
				 model.PhoneNumber,
				 model.Message
			);

			

			var manager = new NotificationManager();
			manager.SendNotification(contactMessage);

			_context.EmailContact.Add(contactMessage);
			await _context.SaveChangesAsync();

			// Return success message or redirect
			TempData["SuccessMessage"] = "Your message has been sent successfully!";
			return RedirectToAction("Index");

		}
	}
}
