using System.Diagnostics;
using COMP72070_Section3_Group1.Models;
using Microsoft.AspNetCore.Mvc;

// tcp stuff
using System.Net.Sockets;
using System.Text;
using Microsoft.Build.ObjectModelRemoting;
using NuGet.LibraryModel;
using COMP72070_Section3_Group1.Comms;
using Newtonsoft.Json;
using DocumentFormat.OpenXml.Wordprocessing;


namespace COMP72070_Section3_Group1.Controllers
{
    /// <summary>
    /// Main controller for the website
    /// Handles all the routing
    /// </summary>
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger; // logger for debugging
        private readonly VisitorManager _visitorManager; // visitor manager for handling visitors
        private readonly Client _client; // client for sending and receiving packets
        private readonly List<Post> _postList; // list of posts singleton   
        private readonly IWebHostEnvironment _environment; // environment for file handling (getting file path)

        /// <summary>
        /// Constructor for the HomeController
        /// Initializes the logger, visitor manager, client, post list, and environment
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="visitorManager"></param>
        /// <param name="client"></param>
        /// <param name="postList"></param>
        /// <param name="environment"></param>
        public HomeController(ILogger<HomeController> logger, VisitorManager visitorManager, Client client, List<Post> postList, IWebHostEnvironment environment)
        {
            _logger = logger;

            this._visitorManager = visitorManager;
            this._client = client;
            this._postList = postList;
            this._environment = environment;
        }

        /// <summary>
        /// Main page of the website 
        /// Displays all the posts
        /// Adds the visitor to the viewbag for authenticated view
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // add visitor to viewbag
            string visitorId = HttpContext.Session.GetString("VisitorId");
            Visitor visitor = _visitorManager.GetVisitor(visitorId);
            ViewBag.Visitor = visitor; // Pass visitor to the view via ViewBag

            return View(_postList);
        }

        /// <summary>
        /// Privacy page of the website
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            // add visitor to viewbag
            string visitorId = HttpContext.Session.GetString("VisitorId");
            Visitor visitor = _visitorManager.GetVisitor(visitorId);
            ViewBag.Visitor = visitor; // Pass visitor to the view via ViewBag

            return View();
        }

        /// <summary>
        /// AstroPost page
        /// Displays the form for posting
        /// Adds the visitor to the viewbag for authenticated view
        /// Redirects to login page if not authenticated
        /// Adds a message if not authenticated
        /// </summary>
        /// <returns></returns>
        public IActionResult AstroPost()
        {
            // add visitor to viewbag
            string visitorId = HttpContext.Session.GetString("VisitorId");
            Visitor visitor = _visitorManager.GetVisitor(visitorId);
            ViewBag.Visitor = visitor; // Pass visitor to the view via ViewBag

            if (visitor.isAuthenicated == false)
            {
                TempData["Message"] = "You must be logged in to post"; // special variable that persists for one redirect
                return RedirectToAction("Login", "Home");
            }

            return View();
        }

        /// <summary>
        /// Profile page
        /// Displays the profile of the user
        /// Allows the user to update their profile picture
        /// </summary>
        /// <returns></returns>
        public IActionResult Profile()
        {
            // get visitor from session
            string visitorId = HttpContext.Session.GetString("VisitorId");
            Visitor visitor = _visitorManager.GetVisitor(visitorId);
            ViewBag.Visitor = visitor;
            
            // add visitor to view
            return View(visitor);
        }

        /// <summary>
        /// SubmitProfilePic action
        /// Submits the profile picture to the server
        /// Updates the visitor with the new image name
        /// Adds a message if the file extension is invalid
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IActionResult SubmitProfilePic(IFormFile file)
        {
            // get visitor from session
            string visitorId = HttpContext.Session.GetString("VisitorId");
            Visitor visitor = _visitorManager.GetVisitor(visitorId);
            ViewBag.Visitor = visitor;

            // check if the file is empty
            string fileName = "";
            if (file != null && file.Length > 0)
            {
                Console.WriteLine($"HomeController.SubmitProfilePic(): File received {file.FileName}");

                // check if file extension is valid
                if (Path.GetExtension(file.FileName) != ".jpg" && Path.GetExtension(file.FileName) != ".png")
                {
                    Console.WriteLine($"HomeController.SubmitProfilePic(): Invalid file extension {file.FileName}");
                    TempData["Message"] = "Invalid file extension"; // special variable that persists for one redirect
                    return RedirectToAction("Profile", "Home");
                }

                // generate unique file name
                fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

                // save to wwwroot/images
                string path = Path.Combine(_environment.WebRootPath, "images", fileName);

                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    file.CopyTo(fileStream);
                }
                Console.WriteLine($"HomeController.SubmitProfilePic(): File saved {fileName}");

                // send image 
                _client.SendImage(path);

                // send update account packet
                if (visitor.username == null)
                {
                    // should not happen, b/c vistor should only be able to reach this page if logged in
                    ViewData["Message"] = "You must be logged in to update your profile picture";
                    return RedirectToAction("Login", "Home");
                }
                _client.SendUpdateAccount(visitor.username, "imageName", fileName);

                // update visitor with new image name
                visitor.imageName = fileName;
                _visitorManager.UpdateVisitor(visitor);
            }

            return RedirectToAction("Profile", "Home");
        }

        /// <summary>
        /// Login page
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// CreateAccount page
        /// </summary>
        /// <returns></returns>
        public IActionResult CreateAccount()
        {
            return View();
        }

        /// <summary>
        /// Error page
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /// <summary>
        /// SubmitPost action
        /// Submits the post to the server
        /// Adds the post to the post list
        /// Adds alert message if the content is empty
        /// Adds alert message if the file extension is invalid
        /// </summary>
        /// <param name="content"></param>
        /// <param name="file"></param>
        /// <returns></returns>
        public IActionResult SubmitPost(string content, IFormFile file)
        {
            //Account test = new Account();
            //PostController test = new PostController();
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

                return RedirectToAction("AstroPost", "Home");
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
                    TempData["Message"] = "Invalid file extension"; // special variable that persists for one redirect
                    return RedirectToAction("AstroPost", "Home");
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

            // wait for ack
            _client.WaitForAck();

            // add post to top of list
            _postList.Insert(0, post);

            //return to home page;
            return RedirectToAction("Index", "Home");

        }

        /// <summary>
        /// CreateAccountAction
        /// Creates an account for the user
        /// Adds alert message if the username is invalid
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IActionResult CreateAccountAction(string username, string password)
        {
            //validate username and password
            // - cannot be empty
            // - cannot contain ','
            if (username == null || username == "" || password == null || password == "" || username.Contains(",") || password.Contains(","))
            {
                TempData["Message"] = "Invalid username"; // special variable that persists for one redirect
                return RedirectToAction("CreateAccount", "Home");
            }

            string combinedInfo = $"Username: {username}, Password: {password}";

            // get visitor id from session
            string visitorId = HttpContext.Session.GetString("VisitorId");
            if (visitorId == null)
            {
                visitorId = Guid.NewGuid().ToString();
                HttpContext.Session.SetString("VisitorId", visitorId);
                Visitor tempVisitor = new Visitor(visitorId);
                _visitorManager.AddVisitor(tempVisitor);
            }
            Visitor visitor = _visitorManager.GetVisitor(visitorId);

            // send account creation request to server
            Packet packet = new Packet(visitor.id, Packet.Type.Acc, Encoding.ASCII.GetBytes(combinedInfo));
            _client.SendPacket(packet);

            // receive response from server
            Console.WriteLine("HomeController.CreateAccountAction(): Waiting Acc for response");
            Packet response = _client.ReceivePacket();

            // check if account creation was successful
            if (response.header.packetType == Packet.Type.AccSuccess)
            {
                // set visitor as authenticated
                visitor.isAuthenicated = true;

                // set visitor username
                visitor.username = username;

                // update visitor
                _visitorManager.UpdateVisitor(visitor);

                // redirect to home page
                return RedirectToAction("Index", "Home");
            }
            else
            {
                visitor.isAuthenicated = false;

                TempData["Message"] = "Account creation failed"; // special variable that persists for one redirect
                return RedirectToAction("CreateAccount", "Home");
            }


        }

        /// <summary>
        /// LoginAction
        /// Logs the user in by sending the username and password to the server
        /// Adds alert message if the login fails
        /// update visitor info with username and image name
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public IActionResult LoginAction(string username, string password)
        {
            string combinedInfo = $"Username: {username}, Password: {password}";

            // get current visitor id   
            string visitorId = HttpContext.Session.GetString("VisitorId");
            Visitor visitor = _visitorManager.GetVisitor(visitorId);

            // send auth packet
            Packet packet = new Packet(visitor.id, Packet.Type.Auth, Encoding.ASCII.GetBytes(combinedInfo));
            _client.SendPacket(packet);

            // receive response
            Packet response = _client.ReceivePacket();


            // check if auth was successful
            if (response.header.packetType == Packet.Type.AuthSuccess)
            {
                // set visitor as logged in
                visitor.isAuthenicated = true;

                // set visitor username
                visitor.username = username;

                // get image name from body
                string imageName = Encoding.ASCII.GetString(response.body);

                // set visitor image name
                visitor.imageName = imageName;

                // update visitor
                _visitorManager.UpdateVisitor(visitor);

                TempData["Message"] = $"Welcome {visitor.username}"; //!!! special variable that persists for one redirect

                // redirect to home page need success message
                return RedirectToAction("Index", "Home");
            }
            else
            {
                visitor.isAuthenicated = false;
                TempData["Message"] = "Login Fail"; //!!! special variable that persists for one redirect

                // return to login page need error message
                return RedirectToAction("Login", "Home");
            }
        }

        /// <summary>
        /// LogoutAction
        /// Logs the user out
        /// </summary>
        /// <returns></returns>
        public IActionResult LogoutAction()
        {
            // get current visitor id
            string visitorId = HttpContext.Session.GetString("VisitorId");
            Visitor visitor = _visitorManager.GetVisitor(visitorId);

            // set visitor as logged out
            visitor.isAuthenicated = false;
            visitor.username = "";

            // update visitor
            _visitorManager.UpdateVisitor(visitor);

            TempData["Message"] = "You have been logged out"; // special variable that persists for one redirect

            // redirect to home page
            return RedirectToAction("Index", "Home");
        }
    }
}
