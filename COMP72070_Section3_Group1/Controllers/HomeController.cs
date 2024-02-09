using System.Diagnostics;
using COMP72070_Section3_Group1.Models;
using Microsoft.AspNetCore.Mvc;

namespace COMP72070_Section3_Group1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Account _account;

        public HomeController(ILogger<HomeController> logger, Account account)
        {
            _logger = logger;
            _account = account;
        }

        public IActionResult Index()
        {
            return View(_account);
        }

        public IActionResult Privacy()
        {
            return View(_account);
        }
        public IActionResult AstroPost()
        {
            return View(_account);
        }

        public IActionResult AstroFans()
        {
            return View(_account);
        }

        public IActionResult AstroMessage()
        {
            return View(_account);
        }

        public IActionResult AstroSpace()
        {
            return View(_account);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
