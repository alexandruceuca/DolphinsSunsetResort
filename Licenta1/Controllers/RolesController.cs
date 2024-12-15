using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Dictionaries;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Twilio.TwiML.Messaging;

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

        #region Admin


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts(string emailFilter,string phoneFilter,string roleFilter)
        {
			ViewBag.roles = await _roleManager.Roles.Select(r => r.Name).ToListAsync();

            var userFilters = new UserFilterViewModel
            {
                EmailFilter= emailFilter,
                PhoneNumberFilter= phoneFilter,
                RoleFilter= roleFilter

            };

			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {

                return ViewComponent("UserList", new { methodName = "InvokeAsync", userFilters = userFilters });
            }

            return View("/Views/Roles/Admin/Index.cshtml");
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
                FristName = user.FirstName,
                LastName = user.LastName,
                Roles = roles,
                UserRoles = userRoles
            };

            return View("/Views/Roles/Admin/EditRoles.cshtml", model);
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

        #endregion

        #region RoomCleaner


        [Authorize(Roles = "Admin,Manager,Reception,RoomCleaner")]
        public async Task<IActionResult> GetRoomsToClean()
        {
            return View("/Views/Roles/RoomCleaner/Index.cshtml");
        }

        [Authorize(Roles = "Admin,Manager,Reception,RoomCleaner")]
        [HttpPost]
        public async Task<IActionResult> MarkAsReadyForCheckIn(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null)
            {
                return Json(new { success = false, message = "Room not found" });
            }

            room.RoomStatus = RoomStatus.ReadyForCheckIn;
            await _context.SaveChangesAsync();

            // Return success response
            return Json(new { success = true });
        }

        #endregion


        #region Reception

        [Authorize(Roles = "Admin,Manager,Reception")]
        public async Task<IActionResult> GetBookingsToday(int bookingIdFilter, string phoneFilter, string emailFilter)
        {

			var bookingFilters = new BookingFilterViewModel
			{
				EmailFilter = emailFilter,
				PhoneNumberFilter = phoneFilter,
                BookingIdFilter=bookingIdFilter,

			};


			if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
               
                return ViewComponent("TodaysBookingsList",bookingFilters);
            }

            
            return View("/Views/Roles/Reception/TodaysBookings.cshtml");
        }

        #endregion
    }
}
