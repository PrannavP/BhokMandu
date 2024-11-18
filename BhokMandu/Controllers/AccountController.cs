using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BhokMandu.Data;
using BhokMandu.Models;
using Microsoft.AspNetCore.Authorization;

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
        public IActionResult Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var user = _context.User.FirstOrDefault(u => u.Email == email);

                if(user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
                {

                    Console.WriteLine(user.FullName);
                    Console.WriteLine(user.Email);
                    Console.WriteLine(user.PasswordHash);
                    Console.WriteLine(user.Role);
                    // Store user info in a session
                    HttpContext.Session.SetString("Email", user.Email);
                    HttpContext.Session.SetString("FullName", user.FullName);
                    HttpContext.Session.SetString("Role", user.Role);

                    // Redirect based on role
                    if(user.Role == "Admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Invalid username or password");
            }

            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult ManageUsers()
        {
            return View();
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}