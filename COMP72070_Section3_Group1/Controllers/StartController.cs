using COMP72070_Section3_Group1.Models;
using COMP72070_Section3_Group1.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;

namespace COMP72070_Section3_Group1.Controllers
{
    /// <summary>
    /// THIS MUST RUN FIRST 
    /// Will redirect to the home/index page dw
    /// </summary>
    public class StartController : Controller
    {
        private readonly UserManager _userManager; // user manager object SINGLETON

        private readonly Client _client; // client object SINGLETON

        public StartController(UserManager visitorManager, Client client)
        {
            this._userManager = visitorManager;
            this._client = client;
        }

        /// <summary>
        /// First action
        /// creates and adds a new user to the user manager
        /// </summary>
        public IActionResult Index()
        {
            // get the user id from the session dic
            string userId = HttpContext.Session.GetString("UserId");

            if(userId == null)
            {
                // generate new UNIQUE user id  
                userId = Guid.NewGuid().ToString(); 

                // add the user id to the session dic
                HttpContext.Session.SetString("UserId", userId);

                // create a new user object
                User user = new User(userId);

                // add the user to the user manager
                _userManager.AddUser(user);
            }
            else
            {
                //check if the user is already in the user manager
                if(!_userManager.users.ContainsKey(userId))
                {
                    // create a new user object
                    User visitor = new User(userId);

                    // add the user to the user manager
                    _userManager.AddUser(visitor);
                }
            }

            return RedirectToAction("Index", "Home"); // redirect to the home page
        }

        public IActionResult ExampleAccount(string username, string password)
        {
            string combinedInfo = $"Username: {username}, Password: {password}";

            // new session code
            string userId = HttpContext.Session.GetString("UserId");
            User visitor = _userManager.GetVisitor(userId);

            Packet packet = new Packet(visitor.id,Packet.Type.Post, false, Encoding.ASCII.GetBytes(combinedInfo));

            _client.SendPacket(packet);

            // Return to the current view
            return RedirectToAction("Index", "Home");
        }
    }
}
