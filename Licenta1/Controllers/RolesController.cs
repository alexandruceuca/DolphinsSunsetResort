using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DolphinsSunsetResort.Controllers
{

	public class RolesController : Controller
	{
		private readonly AuthDbContext _context;
		private readonly UserManager<AplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;

		public RolesController(AuthDbContext context, UserManager<AplicationUser> userManager, RoleManager<IdentityRole> roleManager)
		{
			_context = context;
			_userManager = userManager;
			_roleManager = roleManager;
		}

		[Authorize(Roles = "Admin")]
		public IActionResult GetAllAccounts()
		{
			var users = _userManager.Users
			.Select(user => new
			{
				user.Id,
				user.UserName,
				user.Email,
				Role = _userManager.GetRolesAsync(user).Result.FirstOrDefault()  // Fetch the first role
			}).ToList();
			return View("/Views/Roles/Admin/Index.cshtml",users);
		}

		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditRoles(string id)
		{
			var user = await _userManager.FindByIdAsync(id);
			if (user == null) return NotFound();

			// Get all roles in the system
			var roles = await _roleManager.Roles.ToListAsync();

			// Get the user's currently assigned roles
			var userRoles = await _userManager.GetRolesAsync(user);

			// Prepare the model for the view
			var model = new EditRolesViewModel
			{
				UserId = user.Id,
				UserName = user.UserName,
				Roles = roles,
                UserRoles = userRoles
            };

			return View("/Views/Roles/Admin/EditRoles.cshtml",model);
		}

		[HttpPost]
		[Authorize(Roles = "Admin")]
		public async Task<IActionResult> EditRoles(string userId, List<string> selectedRoles)
		{
			var user = await _userManager.FindByIdAsync(userId);
			if (user == null) return NotFound();

			// Get the current roles for the user
			var currentRoles = await _userManager.GetRolesAsync(user);

			// Determine the roles to add and remove
			var rolesToAdd = selectedRoles.Except(currentRoles).ToList();
			var rolesToRemove = currentRoles.Except(selectedRoles).ToList();

			// Add new roles
			if (rolesToAdd.Any())
			{
				await _userManager.AddToRolesAsync(user, rolesToAdd);
			}

			// Remove old roles
			if (rolesToRemove.Any())
			{
				await _userManager.RemoveFromRolesAsync(user, rolesToRemove);
			}


			return RedirectToAction("GetAllAccounts", "Roles");
		}

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false });
        }


    }
}
