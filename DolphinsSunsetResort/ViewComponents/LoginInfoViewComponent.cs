using DolphinsSunsetResort.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace DolphinsSunsetResort.ViewComponents
{
    [ViewComponent]
    public class LoginInfoViewComponent : ViewComponent
    {
        private readonly UserManager<AplicationUser> _userManager;

        public LoginInfoViewComponent(UserManager<AplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var fullName = user != null ? $"{user.FirstName} {user.LastName}" : "Guest";
           return View("/Views/Shared/Components/LoginInfo/Default.cshtml", fullName);

        }
    }
}

