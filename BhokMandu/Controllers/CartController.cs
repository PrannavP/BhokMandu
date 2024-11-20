using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using BhokMandu.Data;
using BhokMandu.Models;

namespace BhokMandu.Controllers
{
    public class CartController : Controller
    {
        private readonly BhokManduContext _context;

        public CartController(BhokManduContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // Return the view for the cart
            return View();
        }

        [HttpPost]
        public IActionResult OrderNow([FromBody] List<OrderItem> cartItems)
        {
            // Log received items for debugging
            Console.WriteLine("Received Cart Items: " + JsonConvert.SerializeObject(cartItems));

            if (cartItems == null || cartItems.Count == 0)
            {
                return BadRequest("No items in the cart.");
            }

            // Retrieve user information from session
            string customerName = HttpContext.Session.GetString("FullName") ?? "Guest";

            // Create a new Order object
            var order = new Order
            {
                CustomerName = customerName,
                OrderDate = DateTime.Now,
                TotalAmount = CalculateTotalAmount(cartItems),
                Items = cartItems,
                Status = OrderStatus.Pending
            };

            _context.Order.Add(order);

            try
            {
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                return BadRequest("Error saving order: " + ex.Message);
            }

            return Ok(); // Return OK status on success
        }

        private decimal CalculateTotalAmount(List<OrderItem> items)
        {
            decimal total = 0;
            foreach(var item in items)
            {
                total += item.Price + item.Quantity;
            }
            return total;
        }
    }
}