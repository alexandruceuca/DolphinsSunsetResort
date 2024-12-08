using DolphinsSunsetResort.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace DolphinsSunsetResort.ViewComponents
{
    [Authorize(Roles = "Admin,Manager,Reception,RoomCleaner")]
    public class RoomsCleaningListViewComponent : ViewComponent
    {
        private readonly AuthDbContext _context;

        public RoomsCleaningListViewComponent(AuthDbContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Admin,Manager,Reception,RoomCleaner")]
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var roomsNeedingCleaning = await _context.Rooms
                .Where(r => r.RoomStatus == Dictionaries.RoomStatus.NeedsCleaning)
                .ToListAsync();

            return View("/Views/Shared/Components/Roles/RoomCleaning/RoomsCleaningList.cshtml", roomsNeedingCleaning);
        }
    }
}
