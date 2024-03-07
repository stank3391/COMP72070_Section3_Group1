using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class TcpServer
{
    private TcpListener Listener;
    private int Port = 27000;
    private IPAddress LocalAddress = IPAddress.Any;
    private bool IsStop = false;
    private List<Client> Connections { get; set; } = new List<Client>();

    public TcpServer()
    {
        this.Listener = new TcpListener(this.LocalAddress, this.Port);
        this.Listener.Start();
        Console.WriteLine("Server started. Listenting for clients.\n");
    }

    // starts the server and listens for clients
    public void Start()
    {
        while (!this.IsStop)
        {
            TcpClient client = this.Listener.AcceptTcpClientAsync().Result;
            this.Connections.Add(new Client(client));

            Console.WriteLine("Connected to: " + client.ToString());

            Task.Run(() => HandleClient(client));

        }
    }

    // handles the client
    private void HandleClient(TcpClient client)
    {
        byte[] bufferIn = new byte[1024]; // buffer for incoming data
        byte[] bufferOut = new byte[1024]; // buffer for outgoing data
        string data = string.Empty; // parsed data from buffer
        NetworkStream stream; // stream for reading and writing data

        stream = client.GetStream();

        // read incoming data
        stream.Read(bufferIn, 0, bufferIn.Length);
        data = Encoding.ASCII.GetString(bufferIn, 0, bufferIn.Length);
        Console.WriteLine("Received: " + data);


        // send response
        bufferOut = Encoding.ASCII.GetBytes("Hello from server");
        stream.Write(bufferOut, 0, bufferOut.Length);
        Console.WriteLine("Sent: Hello from server");
       

        //client.Close();
    }

}

