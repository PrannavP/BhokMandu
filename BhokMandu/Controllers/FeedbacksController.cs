using BhokMandu.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BhokMandu.Controllers
{
    public class FeedbacksController : Controller
    {
        private readonly BhokManduContext _context;

        public FeedbacksController(BhokManduContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("FullName, Email, Message, PhoneNumber")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                // setting timestamp
                feedback.CreatedAt = DateTime.Now;

                _context.Add(feedback);
                await _context.SaveChangesAsync();

                // Set success message in TempData
                TempData["SuccessMessage"] = "Feedback sent successfully!";

                return RedirectToAction(nameof(Index));
            }

            return View(feedback);

        }
    }
}
