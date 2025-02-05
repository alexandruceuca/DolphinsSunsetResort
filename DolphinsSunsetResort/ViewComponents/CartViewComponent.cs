using DolphinsSunsetResort.Data;
using DolphinsSunsetResort.Service;
using Microsoft.AspNetCore.Mvc;

namespace DolphinsSunsetResort.ViewComponents
{
    public class CartViewComponent: ViewComponent
    {
        private readonly AuthDbContext _context;

        public CartViewComponent(AuthDbContext context)
        {
            _context = context;
        }
        public IViewComponentResult Invoke()
        {
            var cart = CartService.GetCart(_context,this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return View("CartSummary");
        }
    }
}
