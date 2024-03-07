using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

// Represents a client that connects to the server
public class Client
{
    public TcpClient TcpClient { get; private set; } // The TcpClient object 

    public bool IsAuthenticated { get; set; } // Indicates if the client is authenticated

    // Constructor
    public Client(TcpClient client)
    {
        this.TcpClient = client;
        this.IsAuthenticated = false;
    }

    // Authenticates the client, for now it just sets the IsAuthenticated property to true
    public bool Authenticate()
    {
        this.IsAuthenticated = true;
        return true;
    }
}
