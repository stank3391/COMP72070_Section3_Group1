using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class Client
{
    private TcpClient TcpClient { set; get; }

    public bool IsAuthenticated { get; set; }

    public Client(TcpClient client)
    {
        this.TcpClient = client;
        this.IsAuthenticated = false;
    }
}

