using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class Program
{
    public static void Main()
    {
        TcpServer server = new TcpServer();
        server.Start();

        
    }
}
