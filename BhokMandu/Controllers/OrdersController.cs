using BhokMandu.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhokMandu.Controllers
{
    public class OrdersController : Controller
    {
        private readonly BhokManduContext _context;
        public OrdersController(BhokManduContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "User")]
        public IActionResult OrderHistory()
        {
            string customerName = HttpContext.Session.GetString("FullName");

            if(string.IsNullOrEmpty(customerName))
            {
                return RedirectToAction("Login", "Account");
            }

            // Query orders and their associated items
            var userOrders = _context.Order
                .Where(o => o.CustomerName == customerName)
                .Include(o => o.Items)
                .ToList();

            return View(userOrders);
        }
    }
}
