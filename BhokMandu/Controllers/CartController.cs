using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json; // Add this for JSON serialization
using System.Collections.Generic;

namespace BhokMandu.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            // Return the view for the cart
            return View();
        }
    }
}