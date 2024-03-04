using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    internal class ProgramServer
    {
        const String LOCALHOSTADR = "127.0.0.1";
        const int LOCALPORT = 27000;

        public static async Task MainServer()
        {
            TcpListener server = new TcpListener(IPAddress.Parse(LOCALHOSTADR), LOCALPORT);
            server.Start();

            Console.WriteLine("Server listening...");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Thread.Sleep(1000);
                _ = HandleClientAsync(client);
            }
        }

        static async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                using NetworkStream stream = client.GetStream();
                var buffer = new byte[1_024];

                int received = await stream.ReadAsync(buffer);
                var message = Encoding.UTF8.GetString(buffer, 0, received);
                Console.WriteLine($"Message received from {client.Client.RemoteEndPoint}: \"{message}\"");

                // Process or respond to the message as needed...

                // Example response
                byte[] response = Encoding.UTF8.GetBytes("Server response: Message received successfully!");
                await stream.WriteAsync(response, 0, response.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error handling client: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }

        static async Task MainTest()
        {
            try
            {
                TcpClient client = new TcpClient();
                await client.ConnectAsync("127.0.01", 27000);
                NetworkStream stream = client.GetStream();

                // Send messages
                await SendMessageAsync(stream, "Hello from Client A");
                await SendMessageAsync(stream, "Hello from Client B");

                // Receive messages
                await ReceiveMessageAsync(stream);

                stream.Close();
                client.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        static async Task SendMessageAsync(NetworkStream stream, string message)
        {
            byte[] data = Encoding.UTF8.GetBytes(message);
            await stream.WriteAsync(data, 0, data.Length);
            Console.WriteLine($"Sent message: \"{message}\"");
        }

        static async Task ReceiveMessageAsync(NetworkStream stream)
        {
            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
            string receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);
            Console.WriteLine($"Received message: \"{receivedMessage}\"");
        }
    }
}