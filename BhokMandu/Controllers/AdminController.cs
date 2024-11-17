using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using BhokMandu.Data;
using BhokMandu.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

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
            Console.WriteLine($"Attempting login with email: {email}, returnUrl: {returnUrl}");
            // Replace with actual admin credential validation logic
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

                // Handle ReturnUrl
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    returnUrl = System.Web.HttpUtility.UrlDecode(returnUrl); // Decode the ReturnUrl
                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl); // Redirect to ReturnUrl if it's valid
                    }
                }

                return RedirectToAction("Dashboard");
            }

            // Invalid credentials
            ViewBag.ErrorMessage = "Invalid credentials";
            return View();
        }

        // Admin Dashboard
        [Authorize(Roles = "Admin")]
        public IActionResult Dashboard()
        {
            // Retrieve total users
            var totalUsers = _context.User.Count();

            ViewData["TotalUsers"] = totalUsers;
            return View();
        }

        // List users
        [Authorize(Roles = "Admin")]
        public IActionResult Users()
        {
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
            if(id == null)
            {
                return NotFound();
            }

            var user = await _context.User.FindAsync(id);
            if(user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: admin/users/edit/5
        [Authorize(Roles = "Admin")]
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, FullName, Email, PasswordHashed")] User user)
        {
            if(id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
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
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

		private bool UserExists(int id)
		{
			return _context.User.Any(e => e.Id == id);
		}
	}
}