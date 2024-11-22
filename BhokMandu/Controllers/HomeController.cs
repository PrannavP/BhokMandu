using BhokMandu.Data; // Ensure this namespace is included
using BhokMandu.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace BhokMandu.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BhokManduContext _context;

        public HomeController(ILogger<HomeController> logger, BhokManduContext context)
        {
            _logger = logger;
            _context = context;
        }

        //[HttpGet("")]
        public IActionResult Index(string searchQuery)
        {
            Console.WriteLine(HttpContext.Session.GetString("FullName"));

            // Retrieve all foods from the database
            var foods = _context.Food.AsQueryable();

            // If a search query is provided, filter the foods based on FoodName
            if (!string.IsNullOrEmpty(searchQuery))
            {
                foods = foods.Where(f => EF.Functions.Like(f.Name, $"%{searchQuery}%"));
                ViewData["searchQuery"] = searchQuery; // Retain the search query in the input field
            }

            // Return the filtered or all foods to the view
            return View(foods.ToList());
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}