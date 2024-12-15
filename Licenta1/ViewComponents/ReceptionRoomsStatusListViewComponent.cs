using DolphinsSunsetResort.Areas.Identity.Data;
using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Views.ViewsModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace DolphinsSunsetResort.ViewComponents
{
    public class ReceptionRoomsStatusListViewComponent : ViewComponent
    {
        private readonly AuthDbContext _context;
        private readonly UserManager<AplicationUser> _userManager;

        public ReceptionRoomsStatusListViewComponent(UserManager<AplicationUser> userManager, AuthDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

		[Authorize(Roles = "Admin,Manager,Reception")]
		public IViewComponentResult Invoke()
		{
            var roomsStatus=_context.Rooms.ToList();



			return View("/Views/Shared/Components/Roles/Reception/ReceptionRoomsStatusList.cshtml",roomsStatus);
		}
	}
}
