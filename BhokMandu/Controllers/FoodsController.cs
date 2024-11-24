using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BhokMandu.Data;
using BhokMandu.Models;
using Microsoft.AspNetCore.Authorization;

namespace BhokMandu.Controllers
{
    [Route("admin/[controller]")]
    [Authorize(Roles = "Admin")]
    public class FoodsController : Controller
    {
        private readonly BhokManduContext _context;

        public FoodsController(BhokManduContext context)
        {
            _context = context;
        }

        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: /admin/foods
        [HttpGet("")]
        //[HttpGet("/admin")]
        public async Task<IActionResult> Index(string foodCategory, string searchString)
        {
            ViewData["ActiveMenu"] = "Foods";

            if (_context.Food == null)
            {
                return Problem("Entity set 'BhokManduContext.Food' is null");
            }

            // Use LINQ to get list of categories.
            IQueryable<string> categoryQuery = from m in _context.Food
                                               orderby m.Category
                                               select m.Category;

            var foods = from m in _context.Food
                        select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                foods = foods.Where(s => s.Name!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(foodCategory))
            {
                foods = foods.Where(x => x.Category == foodCategory);
            }

            var foodsCategoryVM = new FoodCategoryViewModel
            {
                Categories = new SelectList(await categoryQuery.Distinct().ToListAsync()),
                Foods = await foods.ToListAsync()
            };

            return View(foodsCategoryVM);
        }

        // GET: admin/foods/details/5
        [HttpGet("details/{id}")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food.FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // GET: admin/foods/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: admin/foods/create
        [HttpPost("create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,Price,Category,Rating")] Food food, IFormFile? Image)
        {
            if (ModelState.IsValid)
            {
                if (Image != null)
                {
                    // Your existing image upload logic
                    var fileName = Path.GetFileName(Image.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await Image.CopyToAsync(stream);
                    }

                    food.ImagePath = "/images/" + fileName;
                }

                _context.Add(food);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(food);
        }



        // GET: admin/foods/edit/5
        [HttpGet("edit/{id}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        [HttpPost("edit/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Category,Rating")] Food food, IFormFile? Image)
        {
            if (id != food.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Retrieve the existing food item
                    var existingFood = await _context.Food.FindAsync(id);
                    if (existingFood == null)
                    {
                        return NotFound();
                    }

                    // Update properties
                    existingFood.Name = food.Name;
                    existingFood.Description = food.Description;
                    existingFood.Price = food.Price;
                    existingFood.Category = food.Category;
                    existingFood.Rating = food.Rating;

                    // Handle image upload
                    if (Image != null)
                    {
                        var fileName = Path.GetFileName(Image.FileName);
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);

                        // Ensure the directory exists
                        var directoryPath = Path.GetDirectoryName(filePath);
                        if (!Directory.Exists(directoryPath))
                        {
                            Directory.CreateDirectory(directoryPath);
                        }

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await Image.CopyToAsync(stream);
                        }

                        // Update ImagePath
                        existingFood.ImagePath = "/images/" + fileName;
                    }

                    // Save changes
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodExists(food.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Rethrow the exception for further handling
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(food);
        }

        // GET: admin/foods/delete/5
        [HttpGet("delete/{id}")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var food = await _context.Food.FirstOrDefaultAsync(m => m.Id == id);
            if (food == null)
            {
                return NotFound();
            }

            return View(food);
        }

        // POST: admin/foods/delete/5
        [HttpPost("delete/{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var food = await _context.Food.FindAsync(id);
            if (food != null)
            {
                _context.Food.Remove(food);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodExists(int id)
        {
            return _context.Food.Any(e => e.Id == id);
        }
    }
}