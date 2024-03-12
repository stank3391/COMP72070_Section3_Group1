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
        private readonly VisitorManager _visitorManager;

        public StartController(VisitorManager visitorManager)
        {
            _visitorManager = visitorManager;
        }

        /// <summary>
        /// First action
        /// creates and adds a new visitor to the visitor manager
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

                // create a new visitor object
                Visitor visitor = new Visitor(userId);

                // add the visitor to the visitor manager
                _visitorManager.AddVisitor(visitor);
            }
            else
            {
                //check if the visitor is already in the visitor manager
                if(!_visitorManager.Visitors.ContainsKey(userId))
                {
                    // create a new visitor object
                    Visitor visitor = new Visitor(userId);

                    // add the visitor to the visitor manager
                    _visitorManager.AddVisitor(visitor);
                }
            }

            return RedirectToAction("Index", "Home"); // redirect to the home page
        }
    }
}
