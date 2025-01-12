using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DolphinsSunsetResort.Areas.Identity.Pages.Account.Manage
{
	public class NewsletterModel : PageModel
	{
		private readonly UserManager<AplicationUser> _userManager;
		private readonly AuthDbContext _context;
		public NewsletterModel(
			UserManager<AplicationUser> userManager,

			AuthDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}
		public async Task<IActionResult> OnGet()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}
			ViewData["EmailNewsYN"] = user.EmailNewsYN;

			return Page();

		}
		public async Task<IActionResult> OnPostSubscribeAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			user.EmailNewsYN = true;
			//ViewData["EmailNewsYN"] = user.EmailNewsYN;
			await _userManager.UpdateAsync(user);
			return RedirectToPage("./Newsletter");

		}

		public async Task<IActionResult> OnPostUnsubscribeAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			user.EmailNewsYN = false;
			//ViewData["EmailNewsYN"] = user.EmailNewsYN;
			await _userManager.UpdateAsync(user);
			return RedirectToPage("./Newsletter");

		}
	}
}
