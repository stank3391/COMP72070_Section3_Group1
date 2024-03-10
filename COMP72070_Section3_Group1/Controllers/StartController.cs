using COMP72070_Section3_Group1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net.Sockets;
using System.Text;

namespace COMP72070_Section3_Group1.Controllers
{
    public class StartController : Controller
    {
        public IActionResult Index()
        {
            Console.WriteLine("RUNNN");

            TcpClient me = new TcpClient();
            string serverIp = "127.0.0.1";
            int serverPort = 27000;


            me.Connect(serverIp, serverPort);

            if (me.Connected)
            {
                Console.WriteLine("Connected to server");



                /*
                 * SAMPLE CODE: SEND PACKET START
                 */

                // create a packet
                Packet packetOut = new Packet(Packet.Type.Post, false, Encoding.ASCII.GetBytes("Herro from client!!"));

                // serialize the packet
                byte[] serializedPacket = Packet.SerializePacket(packetOut);

                // send the packet to the server
                NetworkStream stream = me.GetStream();
                stream.Write(serializedPacket, 0, serializedPacket.Length);

                Console.WriteLine($"Packet sent: {packetOut.ToString()}\n");

                /*
                 * SAMPLE CODE: SEND PACKET END
                 */


                /*
                 * SAMPLE CODE: RECEIVE PACKET START
                 */

                // receive data from server
                byte[] bufferIn = new byte[1024]; // buffer for incoming data
                stream.Read(bufferIn, 0, bufferIn.Length);

                // deserialize the packet
                Packet packetIn = Packet.DeserializePacket(bufferIn);

                // print the packet
                Console.WriteLine($"Packet received: {packetIn.ToString()}\n");

                /*
                 * SAMPLE CODE: SEND PACKET END
                 */


                return RedirectToAction("Index", "Home");
            }
            else
            {
                Console.WriteLine("Failed to connect to server");
                return RedirectToAction("Error", "Home");
            }
        }
    }
}
