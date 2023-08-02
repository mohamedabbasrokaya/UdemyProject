using Microsoft.AspNetCore.Mvc;

namespace Udemy.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
