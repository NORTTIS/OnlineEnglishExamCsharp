using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN222_English_Exam.Models;
using System.Diagnostics;

namespace PRN222_English_Exam.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("ADMIN"))
                {
                    return RedirectToAction("Logout", "Account");
                }
                else if (User.IsInRole("USER"))
                {
                    return View();
                }
            }
            return View();
        }
        [Authorize(Roles = "USER")]
        [HttpPost]
        public IActionResult Search(string searchValue)
        {
            TempData["SearchValue"] = searchValue;
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
