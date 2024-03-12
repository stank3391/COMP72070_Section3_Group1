using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

// Represents a client that connects to the server
public class Client
{
    public TcpClient tcpClient { get; private set; } // The TcpClient object 

    private string serverIp = "127.0.0.1";
    private int serverPort = 27000;

    public Client()
    {
        this.tcpClient = new TcpClient();
    }

    public Client(TcpClient tcpClient)
    {
        this.tcpClient = tcpClient;
    }

    public void connect()
    {
        this.tcpClient.Connect(serverIp, serverPort);

        if (this.tcpClient.Connected)
        {
            Console.WriteLine("Connected to server");
        }
        else
        {
            Console.WriteLine("Failed to connect to server");
        }
    }

    /// <summary>
    /// Returns a string of the client information
    /// </summary>
    public override string ToString()
    {
        // return the client's IP address, port, authenication status

        string str = $"IP Address: {this.tcpClient.Client.RemoteEndPoint.ToString()} \n";

        return str;
    }

    /// <summary>
    /// Sends a packet to the client
    /// </summary>
    public void SendPacket(Packet packet)
    {
        // serialize the packet
        byte[] serializedPacket = Packet.SerializePacket(packet);

        // send the packet to the client
        NetworkStream stream = this.tcpClient.GetStream();
        stream.Write(serializedPacket, 0, serializedPacket.Length);

        Console.WriteLine($"Packet sent:\n{packet.ToString()}\n");
    }

    /// <summary>
    /// Receives a packet from the client
    /// </summary>
    public Packet ReceivePacket()
    {
        // receive the packet from the client
        NetworkStream stream = this.tcpClient.GetStream();
        byte[] buffer = new byte[1024];
        stream.Read(buffer, 0, buffer.Length);

        // deserialize the packet
        Packet packet = Packet.DeserializePacket(buffer);

        Console.WriteLine($"Packet received:\n{packet.ToString()}\n");

        return packet;
    }
}
