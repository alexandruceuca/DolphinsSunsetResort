using Licenta1.Areas.Identity.Data;
using Licenta1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Licenta1.ViewComponents
{
    public class RoomsListViewComponent : ViewComponent
    {
        private readonly AuthDbContext _context;

        public RoomsListViewComponent(AuthDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync(string methodName)
        {
            if (methodName == "DisplayRoomListHomeAsync")
                return await DisplayRoomListHomeAsync();
            if (methodName == "DisplayRoomListAsync")
                return await DisplayRoomListAsync();

            throw new ArgumentException("Invalid method name");
        }

        public async Task<IViewComponentResult> DisplayRoomListHomeAsync()
        {
            var rooms = await _context.Rooms.ToListAsync();
            return View("/Views/Shared/Components/Rooms/RoomsListHome.cshtml", rooms);

        }

        public async Task<IViewComponentResult> DisplayRoomListAsync()
        {
            var rooms = await _context.Rooms.Include(d=>d.Price).ToListAsync();
            return View("/Views/Shared/Components/Rooms/RoomsList.cshtml", rooms);

        }
    }
}
