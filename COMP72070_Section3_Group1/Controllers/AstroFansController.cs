using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System.Text;
using COMP72070_Section3_Group1.Comms;
using COMP72070_Section3_Group1.Models;

namespace COMP72070_Section3_Group1.Controllers
{
    public class AstroFansController : Controller
    {
        private readonly VisitorManager _visitorManager;
        private readonly Client _client;

        public AstroFansController(VisitorManager visitorManager, Client client)
        {
            this._visitorManager = visitorManager;
            this._client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SendMsg(string inputText)
        {
            // get the user id from the session dic
            string visitorId = HttpContext.Session.GetString("VisitorId");

            // get the visitor from the visitor manager
            Visitor visitor = _visitorManager.GetVisitor(visitorId);

            // send the message to the server
            Packet packet = new Packet(visitor.id, Packet.Type.Test, Encoding.UTF8.GetBytes(inputText));

            _client.SendPacket(packet);

            return RedirectToAction("AstroFans", "Home");
        }
    }
}
