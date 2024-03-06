using COMP72070_Section3_Group1.Models;
using Microsoft.AspNetCore.Mvc;
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
            try
            {

                await client.ConnectAsync(serverIp, serverPort);

                if (client.Connected)
                {
                    Console.WriteLine("Connected to server");
                    var networkStream = client.GetStream();

                    // send
                    var msg = "Hello";
                    var data = Encoding.ASCII.GetBytes(msg);
                    await networkStream.WriteAsync(data, 0, data.Length);
                    Console.WriteLine("sent: " + msg);

                    // recv
                    var buffer = new byte[1024];
                    var bytesRead = await networkStream.ReadAsync(buffer, 0, buffer.Length);
                    var response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    Console.WriteLine("received: " + response);

                    return View();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed..." + ex.Message);
            }
            finally
            {
                // client.Close(); 
            }
            

            return View();
        }

    }
}
