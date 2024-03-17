using System.Diagnostics;
using COMP72070_Section3_Group1.Models;
using Microsoft.AspNetCore.Mvc;

// tcp stuff
using System.Net.Sockets;
using System.Text;
using Microsoft.Build.ObjectModelRemoting;
using NuGet.LibraryModel;
using COMP72070_Section3_Group1.Comms;


namespace COMP72070_Section3_Group1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Account _account;
        private readonly VisitorManager _visitorManager;
        private readonly Client _client;
        private readonly List<Post> _postList;
        private readonly IWebHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, Account account, VisitorManager visitorManager, Client client, List<Post> postList, IWebHostEnvironment environment)
        {
            _logger = logger;
            _account = account;

            this._visitorManager = visitorManager;
            this._client = client;
            this._postList = postList;
            this._environment = environment;
        }
        public IActionResult Index()
        {
            return View(_postList);
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

        public IActionResult SubmitPost(string content, IFormFile file)
        {
            string VisitorId = HttpContext.Session.GetString("VisitorId");

            if (VisitorId == null) // if no visitor id, create a new one
            {
                VisitorId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("VisitorId", VisitorId);
                Visitor tempVisitor = new Visitor(VisitorId);
                _visitorManager.AddVisitor(tempVisitor);

            }

            Visitor visitor = _visitorManager.GetVisitor(VisitorId);

            // check if content 
            if(content == null || content == "")
            {
                Console.WriteLine($"HomeController.SubmitPost(): No content in post");
                // no content show error message?
                return RedirectToAction("Index", "Home");
            }

            // check if the file is empty
            string fileName = "";
            if (file != null && file.Length > 0)
            {
                Console.WriteLine($"HomeController.SubmitPost(): File received {file.FileName}");

                // check if file extension is valid
                if (Path.GetExtension(file.FileName) != ".jpg" && Path.GetExtension(file.FileName) != ".png")
                {
                    Console.WriteLine($"HomeController.SubmitPost(): Invalid file extension {file.FileName}");
                    // invalid file extension show error message?
                    return RedirectToAction("Index", "Home");
                }

                // generate unique file name
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // save to wwwroot/images
                string path = Path.Combine(_environment.WebRootPath, "images", fileName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                Console.WriteLine($"HomeController.SubmitPost(): File saved {fileName}");
                
                // send image 
                _client.SendImage(path);
            }

            Post post = new Post(visitor, content, fileName);
            
            // send post to server
            _client.SendPost(visitor.id, post);

            Console.WriteLine($"HomeController.SubmitPost(): Post sent {post.content}");

            // add post to top of list
            _postList.Insert(0, post);

            return RedirectToAction("Index", "Home");

        }

    }
}
