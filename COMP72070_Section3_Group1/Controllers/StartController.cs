using COMP72070_Section3_Group1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Net.Sockets;
using System.Text;

namespace COMP72070_Section3_Group1.Controllers
{
    public class StartController : Controller
    {
        public async Task<IActionResult> Index()
        {
            Console.WriteLine("RUNNN");

            using var client = new TcpClient();
            var serverIp = "127.0.0.1";
            var serverPort = 27000;


            await client.ConnectAsync(serverIp, serverPort);

            if (client.Connected)
            {
                Console.WriteLine("Connected to server");
                var networkStream = client.GetStream();

                // send
                //byte[] data = new byte[5] { 'H', 'e', 'l', 'l', 'o'};
                //var data = Encoding.ASCII.GetBytes(msg);
                //await networkStream.WriteAsync(data, 0, data.Length);
                //Console.WriteLine("sent: " + msg);

                char[] data = ['H', 'e', 'l', 'l', 'o' ];
                Packet myPacket = new Packet();
                myPacket.header.msgLen = 5;
                myPacket.header.Source = 1;
                myPacket.header.messageType = Packet.Type.Post;
                myPacket.header.pictureFlag = false;
                myPacket.Body = System.Text.Encoding.UTF8.GetBytes(data);

                var TxBuffer = Packet.SerializePacket(myPacket);
                await networkStream.WriteAsync(TxBuffer, 0, TxBuffer.Length);
                Console.WriteLine("sent: " + TxBuffer);

                // recv
                var buffer = new byte[1024];
                var bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                var response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                Console.WriteLine("received: " + response);
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
