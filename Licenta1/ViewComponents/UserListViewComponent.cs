using Microsoft.AspNetCore.Mvc;

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

    [Authorize(Roles = "Admin")]
    public class UserListViewComponent : ViewComponent
    {
        private readonly UserManager<AplicationUser> _userManager;

        public UserListViewComponent(UserManager<AplicationUser> userManager)
        {
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        public async Task<IViewComponentResult> InvokeAsync(string emailFilter )
        {
            var users = await _userManager.Users
                .Where(u => string.IsNullOrEmpty(emailFilter) || u.Email.Contains(emailFilter))
                .Select(u => new UserWithRolesViewModel
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Email = u.Email,
                    Roles = _userManager.GetRolesAsync(u).Result.ToList()
                })
                .ToListAsync();

            return View("/Views/Shared/Components/Roles/Admin/UserList.cshtml",users);
        }
    }

}
