﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BhokMandu.Data;
using BhokMandu.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace BhokMandu.Controllers
{
    public class AdminController : Controller
    {
        private readonly BhokManduContext _context;

        public AdminController(BhokManduContext context)
        {
            _context = context;
        }

        // Login Page
        public IActionResult AdminLogin()
        {
            return View();
        }

        // Handle Admin Login Post
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AdminLogin(string email, string password, string? returnUrl = null)
        {
            if (email == "admin@admin.com" && password == "admin")
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Dashboard");
            }

            ViewBag.ErrorMessage = "Invalid credentials";
            return View();
        }

        // Admin Dashboard
        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            ViewData["ActiveMenu"] = "Dashboard";

            var totalUsers = _context.User.Count(u => u.Role == "User");
            var totalOrders = _context.Order.Count();
            var totalFoods = _context.Food.Count();

            Console.WriteLine(_context.User.Count(u => u.Role == "User"));

            ViewData["TotalUsers"] = totalUsers;
            ViewData["TotalOrders"] = totalOrders;
            ViewData["TotalFoods"] = totalFoods;

            return View();
        }

        // List users
        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
            ViewData["ActiveMenu"] = "Users";

            var users = _context.User.ToList();
            return View(users);
        }

        // Logout
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("AdminLogin");
        }

        // GET: admin/users/edit/5
        [Authorize(Roles = "Admin")]
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: admin/users/edit/5
        // POST: admin/users/edit/5
        [HttpPost("edit/{id}")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FullName,Email,Role")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingUser = await _context.User.FindAsync(id);
                    if (existingUser == null)
                    {
                        return NotFound();
                    }

                    existingUser.FullName = user.FullName;
                    existingUser.Email = user.Email;
                    existingUser.Role = user.Role;

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Consider logging this exception for debugging
                    }
                }
                return RedirectToAction("Users");
            }

            // Return view with validation errors
            return View(user);
        }

        // GET: admin/users/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: admin/user/delete/5
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Find the user by ID including related orders and order items
            var user = await _context.User
                .Include(u => u.Orders)  // Include related Orders (plural)
                .ThenInclude(o => o.Items)  // Include related OrderItems for each Order
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            // Delete related OrderItems first
            foreach (var order in user.Orders)
            {
                _context.OrderItem.RemoveRange(order.Items);
            }

            // Delete orders
            _context.Order.RemoveRange(user.Orders);

            // Finally, delete the user
            _context.User.Remove(user);

            // Save changes to the database
            await _context.SaveChangesAsync();

            // Redirect to Users list or another appropriate action
            return RedirectToAction("Users");
        }

        // GET: /admin/order
        [Authorize(Roles = "Admin")]
        public IActionResult UserOrders()
        {
            ViewData["ActiveMenu"] = "UserOrders";

            var orders = _context.Order
                                .Include(o => o.Items)
                                .Include(o => o.User)
                                .ToList();
            return View(orders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateOrderStatus([FromBody] UpdateOrderStatusRequest request)
        {
            // Check if request is null
            if (request == null)
            {
                return BadRequest(new { message = "Request cannot be null." });
            }

            Console.WriteLine($"Received OrderId: {request.OrderId}, Status: {request.Status}");

            // Validate OrderId and Status
            if (request.OrderId <= 0 || string.IsNullOrEmpty(request.Status))
            {
                return BadRequest(new { message = "Invalid order ID or status." });
            }

            // Retrieve the order from the database
            var order = await _context.Order.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == request.OrderId);

            // Check if order was found
            if (order == null)
            {
                return NotFound(new { message = "Order not found." });
            }

            // Convert string status back to enum
            if (Enum.TryParse<OrderStatus>(request.Status, true, out var orderStatus))
            {
                order.Status = orderStatus; // Update the order status with the parsed enum value
            }
            else
            {
                return BadRequest(new { message = "Invalid status value." });
            }

            // Save changes to the database
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order status updated successfully." });
        }

        // feedback
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UserFeedbacks()
        {
            ViewData["ActiveMenu"] = "UserFeedbacks";

            // Fetch all feedbacks from db
            var feedbacks = await _context.FeedBack.ToListAsync();

            return View(feedbacks);
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}