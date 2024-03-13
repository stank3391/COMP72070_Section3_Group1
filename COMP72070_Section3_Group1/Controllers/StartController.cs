using COMP72070_Section3_Group1.Comms;
using COMP72070_Section3_Group1.Models;
using COMP72070_Section3_Group1.Visitors;
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
        private readonly VisitorManager _visitorManager; // visitor manager object SINGLETON

        private readonly Client _client; // client object SINGLETON

        private readonly List<Post> _postList; // list of posts to be displayed

        public StartController(VisitorManager visitorManager, Client client, List<Post> postList)
        {
            this._visitorManager = visitorManager;
            this._client = client;
            _postList = postList;
            
        }

        /// <summary>
        /// First action
        /// creates and adds a new visitor to the visitor manager
        /// </summary>
        public IActionResult Index()
        {

            // get the session id from the session dic
            string visitorId = HttpContext.Session.GetString("VisitorId");

            if(visitorId == null) // new visitor, add to the session dic and visitor manager 
            {
                // generate new UNIQUE visitor id  
                visitorId = Guid.NewGuid().ToString(); 

                // add the visitor id to the session dic
                HttpContext.Session.SetString("VisitorId", visitorId);

                // create a new visitor object
                Visitor visitor = new Visitor(visitorId);

                // add the visitor to the visitor manager
                _visitorManager.AddVisitor(visitor);
            }
            else // returning visitor, check if the visitor is already in the visitor manager
            {
                //check if the visitor is already in the visitor manager
                if(!_visitorManager.visitors.ContainsKey(visitorId))
                {
                    // create a new visitor object
                    Visitor visitor = new Visitor(visitorId);

                    // add the visitor to the visitor manager
                    _visitorManager.AddVisitor(visitor);
                }
            }
            
            return RedirectToAction("AstroFans", "Home"); // redirect to the home page
        }

        public IActionResult ExampleAccount(string username, string password)
        {
            string combinedInfo = $"Username: {username}, Password: {password}";

            // new session code
            string visitorId = HttpContext.Session.GetString("VisitorId");
            Visitor visitor = _visitorManager.GetVisitor(visitorId);

            Packet packet = new Packet(visitor.id,Packet.Type.Auth, false, Encoding.ASCII.GetBytes(combinedInfo));

            _client.SendPacket(packet);

            // Return to the current view
            return RedirectToAction("Index", "Home");
        }
    }
}
