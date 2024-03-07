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

    // Listens and connects to clients. Adds the client to the list of connections
    public void Start()
    {
        while (!this.IsStop)
        {
            TcpClient tcpClient = this.Listener.AcceptTcpClientAsync().Result;

            string clientInfo = tcpClient.Client.RemoteEndPoint.ToString(); // ignore warning 

            Console.WriteLine("Connected to: " + clientInfo);

            // add client to list of connections
            Client client = new Client(tcpClient);
            this.Connections.Add(client);

            Task.Run(() => HandleClient(client)); // becuase async version of AcceptTcpClientAsync() is used 

        }
    }

    // runs in a separate thread per client
    private void HandleClient(Client client)
    {


        byte[] bufferIn = new byte[1024]; // buffer for incoming data
        byte[] bufferOut = new byte[1024]; // buffer for outgoing data
        string data = string.Empty; // parsed data from buffer
        NetworkStream stream; // stream for reading and writing data

        stream = client.TcpClient.GetStream();

        // read incoming data
        stream.Read(bufferIn, 0, bufferIn.Length);
        data = Encoding.ASCII.GetString(bufferIn, 0, bufferIn.Length);
        Console.WriteLine("Received: " + data);


        // send response
        bufferOut = Encoding.ASCII.GetBytes("Hello from server");
        stream.Write(bufferOut, 0, bufferOut.Length);
        Console.WriteLine("Sent: Hello from server");




        //////////////////////////////end///////////////////////////////
        // end connection
        client.TcpClient.Close();

        // remove client from list of connections
        this.Connections.Remove(client);

    }

}

