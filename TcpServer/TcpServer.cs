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


    private bool IsStop = false; // flag to stop the server
    private List<Client> Connections { get; set; } = new List<Client>(); // list of connections for managing clients

    /// <summary>
    /// Constructor for the TcpServer class
    /// </summary>
    public TcpServer()
    {
        this.Listener = new TcpListener(this.LocalAddress, this.Port);
        this.Listener.Start();
        Console.WriteLine("Server started. Listenting for clients.\n");
    }

    /// <summary>
    /// Listens and connects to clients. 
    /// Adds the client to the list of connections.
    /// </summary>
    public void Start()
    {
        while (!this.IsStop)
        {
            TcpClient tcpClient = this.Listener.AcceptTcpClientAsync().Result;

            // add client to list of connections
            Client client = new Client(tcpClient);
            this.Connections.Add(client);
            Console.WriteLine($"Connected to: {client.ToString()}");

            Task.Run(() => HandleClient(client)); // becuase async version of AcceptTcpClientAsync() is used 

        }
    }

    /// <summary>
    /// MAIN FUNCTION TO COMMUNICATE WITH CLIENT!
    /// </summary>
    private void HandleClient(Client client)
    {

        byte[] bufferIn = new byte[1024]; // buffer for incoming data
        byte[] bufferOut = new byte[1024]; // buffer for outgoing data
        string data = string.Empty; // parsed data from buffer
        NetworkStream stream = client.tcpClient.GetStream(); // stream for reading and writing data

        bool isDisconnect = false;
        while (!isDisconnect)
        {
            // receive data from client
            stream.Read(bufferIn, 0, bufferIn.Length);

            // deserialize the packet
            Packet packetIn = Packet.DeserializePacket(bufferIn);

            // print the packet
            Console.WriteLine($"Packet received:\n{packetIn.ToString()}\n");

            client.Authenticate();

            // send packet to client
            Packet packetOut = new Packet(Packet.Type.Post, false, Encoding.ASCII.GetBytes("I got it!!!"));
            byte[] serializedPacket = Packet.SerializePacket(packetOut);
            stream.Write(serializedPacket, 0, serializedPacket.Length);
            Console.WriteLine($"Packet sent:\n{packetOut.ToString()}\n");

            //////////////////////////////end///////////////////////////////

            if (!client.tcpClient.Connected)
            {
                Console.WriteLine($"Client disconnected: {client.ToString()}\n");
                client.tcpClient.Close();
                this.Connections.Remove(client);
            }

        }

    }

}

