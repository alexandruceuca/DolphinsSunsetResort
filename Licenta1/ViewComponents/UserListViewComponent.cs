namespace DolphinsSunsetResort.ViewComponents
{
	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Identity;
	using System.Linq;
	using System.Threading.Tasks;
	using DolphinsSunsetResort.Views.ViewsModel;
	using DolphinsSunsetResort.Areas.Identity.Data;
	using Microsoft.EntityFrameworkCore;
	using Microsoft.AspNetCore.Authorization;
	using DolphinsSunsetResort.Data;

	[Authorize(Roles = "Admin")]
	public class UserListViewComponent : ViewComponent
	{
        private readonly AuthDbContext _context;
        private readonly UserManager<AplicationUser> _userManager;

		public UserListViewComponent(UserManager<AplicationUser> userManager, AuthDbContext context)
		{
			_userManager = userManager;
			_context = context;
		}
		[Authorize(Roles = "Admin")]
		public async Task<IViewComponentResult> InvokeAsync(UserFilterViewModel userFilters)
		{
			var query = _userManager.Users.AsQueryable();

			if (userFilters != null)
			{
				// Apply Email filter
				if (!string.IsNullOrEmpty(userFilters.EmailFilter))
				{
					query = query.Where(u => u.Email.Contains(userFilters.EmailFilter));
				}

				// Apply Phone Number filter
				if (!string.IsNullOrEmpty(userFilters.PhoneNumberFilter))
				{
					query = query.Where(u => u.PhoneNumber.Contains(userFilters.PhoneNumberFilter));
				}

                // Apply Role filter 
                if (!string.IsNullOrEmpty(userFilters.RoleFilter))
                {

                    query = from u in query
                            join ur in _context.UserRoles on u.Id equals ur.UserId
                            join r in _context.Roles on ur.RoleId equals r.Id
                            where r.Name == userFilters.RoleFilter
                            select u;
                }
            }
			
			var users = await query
				.Select(u => new UserWithRolesViewModel
				{
					Id = u.Id,
					FirstName = u.FirstName,
					LastName = u.LastName,
					Email = u.Email,
					PhoneNumber = u.PhoneNumber,
					Roles = new List<string>()
				}).ToListAsync();

		
			foreach (var user in users)
			{
				user.Roles = (await _userManager.GetRolesAsync(await _userManager.FindByIdAsync(user.Id))).ToList();
			}

			return View("/Views/Shared/Components/Roles/Admin/UserList.cshtml", users);
		}
	}

}
