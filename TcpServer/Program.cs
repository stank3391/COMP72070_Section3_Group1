using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class TcpServer
{
    public static void Main()
    {
        TcpListener server;
        int port = 27000;
        IPAddress localAddr = IPAddress.Any;
        // IPAddress localAddr = IPAddress.Parse("127.0.0.1");

        server = new TcpListener(localAddr, port);

        server.Start();

        byte[] bytes = new byte[256];
        String data;

        while (true)
        {
            Console.WriteLine("waiting... ");
            TcpClient client = server.AcceptTcpClient();
            Console.WriteLine("connected!");

            data = "";

            NetworkStream stream = client.GetStream();

            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("received: " +  data);

                data = "i have it";
                byte[] msg = Encoding.ASCII.GetBytes(data);

                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("sent: " + data);
            }

            client.Close();
        }
        
    }
}
