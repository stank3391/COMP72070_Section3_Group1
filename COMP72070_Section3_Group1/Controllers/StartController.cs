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
    /// </summary>
    public class StartController : Controller
    {
        private readonly ClientManager clientManager;

        public StartController(ClientManager clientManager)
        {
            this.clientManager = clientManager;
        }

        /// <summary>
        /// First action when entering StartView
        /// </summary>
        public IActionResult Index()
        {
            Console.WriteLine("RUNNNN");

            
            if (HttpContext.Session.GetString("Client") == null) // if the client is not alrdy conected
            {
                Client client = new Client();
                client.connect();

                HttpContext.Session.SetString("Client", "Connected"); // set the session variable to concneted
                
                // add the client to the client manager
                int key = clientManager.AddClient(client);

                HttpContext.Session.SetInt32("ClientKey", key); // set the session variable to the client id
                
            }

            return RedirectToAction("Index", "Home"); // redirect to the home page
        }

        /// <summary>
        /// Action to send a message to the server
        /// </summary>
        [HttpPost]
        public IActionResult ExampleSendMsg(string inputText)
        {
            Packet packet = new Packet(Packet.Type.Post, false, Encoding.ASCII.GetBytes(inputText)); // create a packet

            int clientKey = (int)HttpContext.Session.GetInt32("ClientKey"); // get the client id from the session variable

            Client client = clientManager.GetClient(clientKey); // get the client from the client manager

            client.SendPacket(packet); // send the packet

            return RedirectToAction("AstroFans", "Home");
        }

        public IActionResult ExampleAccount(string username, string password)
        {
            string combinedInfo = $"Username: {username}, Password: {password}";

            Packet packet = new Packet(Packet.Type.Post, false, Encoding.ASCII.GetBytes(combinedInfo));

            int clientKey = (int)HttpContext.Session.GetInt32("ClientKey");

            Client client = clientManager.GetClient(clientKey);

            client.SendPacket(packet);

            // Return to the current view
            return RedirectToAction("loginwgoogle", "Home");
        }
    }
}
