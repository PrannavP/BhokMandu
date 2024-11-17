using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BhokMandu.Data;
using BhokMandu.Models;

namespace BhokMandu.Controllers
{
    [Route("admin/[controller]")]
    public class RestaurantsController : Controller
    {
        private readonly BhokManduContext _context;

        public RestaurantsController(BhokManduContext context)
        {
            _context = context;
        }

        // Get: /Restaurants
        [HttpGet("")]
        public async Task<IActionResult> Index(string searchString)
        {
            if(_context.Restaurant == null)
            {
                return Problem("Entity set 'BhokMandu.Restaurant' is null");
            }

            var restaurants = from m in _context.Restaurant
                              select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                restaurants = restaurants.Where(s => s.RestaurantName!.ToUpper().Contains(searchString.ToUpper()));
            }

            return View(await restaurants.ToListAsync());
        }

        // GET: admin/restaurant/details/5
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(m => m.Id == id);
            if(restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // GET: admin/restaurant/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

		// POST: admin/restaurant/create
		[HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id, RestaurantName, Address, PhoneNumber, OpeningHours, Rating")] Restaurant restaurant)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurant);
                await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
            return View(restaurant);
        }

        // GET: admin/restaurants/edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {   
            if(id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.FindAsync(id);
            if(restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: admin/restaurant/edit/5
        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id, RestaurantName, Address, PhoneNumber, OpeningHours, Rating")] Restaurant restaurant)
        {
            if (id != restaurant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantExists(restaurant.Id))
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
            return View(restaurant);
        }

        // GET: admin/restaurants/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }

            var restaurant = await _context.Restaurant.FirstOrDefaultAsync(m => m.Id == id);
            if(restaurant == null)
            {
                return NotFound();
            }

            return View(restaurant);
        }

        // POST: admin/restaurants/delete/5
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var restaurant = await _context.Restaurant.FindAsync(id);
            if(restaurant != null)
            {
                _context.Restaurant.Remove(restaurant);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantExists(int id)
        {
            return _context.Restaurant.Any(e => e.Id == id);
        }
    }
}