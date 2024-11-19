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

        [HttpGet("")]
        public async Task<IActionResult> Index()
        {
            var foods = await _context.Food.ToListAsync();
            return View(foods);
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