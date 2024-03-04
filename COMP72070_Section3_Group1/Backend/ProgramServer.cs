namespace COMP72070_Section3_Group1
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;


    public class ProgramServer
    {
        const String LOCALHOSTADR ="127.0.0.1";
        const int LOCALPORT = 27000;
        public static async Task MainServer()
        {
            TcpListener server = new TcpListener(IPAddress.Parse(LOCALHOSTADR), LOCALPORT);
            server.Start();

            Console.WriteLine("Server listening...");

            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
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
    }
}