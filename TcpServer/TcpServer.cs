using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


public class TcpServer
{
    private TcpListener listener;
    private int port;

    private IPAddress localAddr = IPAddress.Any;
    // private IPAddress localAddr = IPAddress.Parse("127.0.0.1");

    public TcpServer()
    {
        this.port = 27000;
        this.listener = new TcpListener(this.localAddr, this.port);
        this.listener.Start();
    }

    public void Start()
    {
        byte[] bytes = new byte[256];
        String data;

        while (true)
        {
            Console.WriteLine("waiting... ");
            TcpClient client = this.listener.AcceptTcpClient();
            Console.WriteLine("connected!");

            data = "";

            NetworkStream stream = client.GetStream();

            int i;
            while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            {
                data = Encoding.ASCII.GetString(bytes, 0, i);
                Console.WriteLine("received: " + data);

                data = "i have it";
                byte[] msg = Encoding.ASCII.GetBytes(data);

                stream.Write(msg, 0, msg.Length);
                Console.WriteLine("sent: " + data);
            }

            client.Close();
        }
    }

}

