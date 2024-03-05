using System.Diagnostics;
using COMP72070_Section3_Group1.Models;
using Microsoft.AspNetCore.Mvc;

// tcp stuff
using System.Net.Sockets;
using System.Text;


namespace COMP72070_Section3_Group1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Account _account;

        public HomeController(ILogger<HomeController> logger, Account account)
        {
            _logger = logger;
            _account = account;
        }

        public async Task<IActionResult> Run()
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

                    Console.WriteLine("recevd: " + response);
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

        public IActionResult Index()
        {
            return View(_account);
        }

        public IActionResult Privacy()
        {
            return View(_account);
        }
        public IActionResult AstroPost()
        {
            return View(_account);
        }

        public IActionResult AstroFans()
        {
            return View(_account);
        }

        public IActionResult AstroMessage()
        {
            return View(_account);
        }

        public IActionResult AstroSpace()
        {
            return View(_account);
        }

		public IActionResult loginwgoogle()
		{
			return View(_account);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
