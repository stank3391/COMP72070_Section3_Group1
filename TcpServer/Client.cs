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

    public Client(TcpClient client)
    {
        this.TcpClient = client;
        this.IsAuthenticated = false;
    }

    /// <summary>
    /// Authenticates the client, (for now it just sets the IsAuthenticated property to true)
    /// </summary>
    /// <returns>
    /// bool: true if the client is authenticated, false otherwise
    /// </returns>
    public bool Authenticate()
    {
        this.IsAuthenticated = true;
        return true;
    }

    /// <summary>
    /// Returns a string of the client information
    /// </summary>
    public override string ToString()
    {
        // return the client's IP address, port, authenication status

        string str = $"IP Address: {this.TcpClient.Client.RemoteEndPoint.ToString()} \n";

        return str;
    }
}
