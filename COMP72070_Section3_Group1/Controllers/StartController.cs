using COMP72070_Section3_Group1.Comms;
using COMP72070_Section3_Group1.Models;
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
    /// creates a new visitor object and adds it to the visitor manager
    /// used to track and manage visitors
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

            if (visitorId == null) // new visitor, add to the session dic and visitor manager 
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
                if (!_visitorManager.visitors.ContainsKey(visitorId))
                {
                    // create a new visitor object
                    Visitor visitor = new Visitor(visitorId);

                    // add the visitor to the visitor manager
                    _visitorManager.AddVisitor(visitor);
                }
            }

            return RedirectToAction("Index", "Home"); // redirect to the home page
        }
    }
}
