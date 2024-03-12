using Microsoft.AspNetCore.Mvc;
using COMP72070_Section3_Group1.Visitors;

namespace COMP72070_Section3_Group1.Controllers
{
    public class AstroFansController : Controller
    {
        private readonly VisitorManager _visitorManager;
        private readonly Client _client;

        public AstroFansController(VisitorManager visitorManager, Client client)
        {
            _visitorManager = visitorManager;
            _client = client;
        }

        public IActionResult Index()
        {
            return View();
        }

        public void SendMsg(string inputText)
        {
            // get the user id from the session dic
            string userId = HttpContext.Session.GetString("UserId");

            // get the visitor from the visitor manager
            Visitor visitor = _visitorManager.GetVisitor(userId);

            // send the message to the server
            Packet packet = new Packet(visitor.id, Packet.Type.Test, false , inputText);

            _client.SendPacket(packet);
            

        }
    }
}
