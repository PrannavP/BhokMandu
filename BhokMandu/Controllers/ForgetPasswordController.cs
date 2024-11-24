using Microsoft.AspNetCore.Mvc;

namespace BhokMandu.Controllers
{
    public class ForgetPasswordController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}