using System.Diagnostics;
using COMP72070_Section3_Group1.Models;
using Microsoft.AspNetCore.Mvc;

// tcp stuff
using System.Net.Sockets;
using System.Text;
using COMP72070_Section3_Group1.Visitors;


namespace COMP72070_Section3_Group1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Account _account;
        private readonly VisitorManager _visitorManager;
        private readonly Client _client;
        private readonly List<Post> _postList;

        public HomeController(ILogger<HomeController> logger, Account account, VisitorManager visitorManager, Client client, List<Post> postList)
        {
            _logger = logger;
            _account = account;

            this._visitorManager = visitorManager;
            this._client = client;
            this._postList = postList;
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

		public IActionResult loginwgoogle()
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
