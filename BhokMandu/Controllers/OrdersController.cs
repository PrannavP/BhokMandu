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
            int? userId = HttpContext.Session.GetInt32("UserId");

            if (string.IsNullOrEmpty(customerName) || userId == null)
            {
                // Redirect to login if session values are not found
                return RedirectToAction("Login", "Account");
            }

            // Debug logging to verify session data (remove in production)
            Console.WriteLine($"FullName: {customerName}, UserId: {userId}");

            var userOrders = _context.Order
                .Where(o => o.UserId == userId.Value)
                .Include(o => o.Items)
                .ToList();

            return View(userOrders);
        }
    }
}
