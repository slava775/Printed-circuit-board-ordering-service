using Microsoft.AspNetCore.Mvc;

namespace Testing.API.Controllers
{
    public class HomeController : Controller
    {
        // GET: /Home/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: /Home/About
        public IActionResult About()
        {
            return View();
        }

        // Обработка ошибок
        public IActionResult Error()
        {
            return View();
        }
    }
}
