using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BhokMandu.Data;
using BhokMandu.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;

namespace BhokMandu.Controllers
{
    public class AccountController : Controller
    {
        private readonly BhokManduContext _context;

        public AccountController(BhokManduContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string fullname, string email, string password)
        {
            if (ModelState.IsValid)
            {
                // Check if the username or email is already in use
                if (_context.User.Any(u => u.FullName == fullname || u.Email == email))
                {
                    ModelState.AddModelError("", "Name or Email is already taken.");
                    return View();
                }

                // Hash the password
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(password);

                // Save the new user
                var user = new User
                {
                    FullName = fullname,
                    Email = email,
                    PasswordHash = passwordHash,
                    Role = "User"
                };

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                // Redirect to Login
                return RedirectToAction("Login");
            }

            return View();
        }

        [HttpPost]
		public async Task<IActionResult> Login(string email, string password)
		{
			if (ModelState.IsValid)
			{
				// Find the user by email
				var user = await _context.User.FirstOrDefaultAsync(u => u.Email == email);

				if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
				{
					// Create claims for authentication
					var claims = new List<Claim>
				{
					new Claim(ClaimTypes.Email, user.Email),
					new Claim(ClaimTypes.Name, user.FullName),
					new Claim(ClaimTypes.Role, user.Role) // Set the role for the user
                };

					var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
					var authProperties = new AuthenticationProperties
					{
						IsPersistent = true // This keeps the user logged in across sessions
					};

					// Sign in the user with the claims
					await HttpContext.SignInAsync("Cookies", new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Save fullname in session
                    HttpContext.Session.SetString("FullName", user.FullName);

					// Redirect based on role
					if (user.Role == "Admin")
					{
						return RedirectToAction("Dashboard", "Admin");
					}
					else
					{
						return RedirectToAction("Index", "Home");
					}
				}
				else
				{
					// Invalid credentials, add error to ModelState
					ModelState.AddModelError("", "Invalid email or password.");
				}
			}

			return View();
		}


		[Authorize(Roles = "Admin")]
        public IActionResult ManageUsers()
        {
            return View();
        }

        // GET: /Account/Profile
        [Authorize] // Make sure user is logged in
        public IActionResult Profile()
        {
            // Get the email of the currently logged-in user from claims
            var email = User.FindFirstValue(ClaimTypes.Email);

            // Retrieve the user details from the database
            var user = _context.User.FirstOrDefault(u => u.Email == email);

            if (user == null)
            {
                // If user is not found, return an error message
                return NotFound("User not found.");
            }

            // Pass the user details to the Profile view
            return View(user);
        }

        [Authorize]
        public IActionResult Order()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync("Cookies");
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Test()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            return Content($"Logged in as {email}, Role: {role}");
        }
    }
}