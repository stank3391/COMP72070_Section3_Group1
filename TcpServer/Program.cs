using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpServer
{
    /// <summary>
    /// Main class for running the tcpserver
    /// </summary>
    class Program
    {
        public static void Main()
        {
            TcpServer server = new TcpServer();
            //server.UpdatePosts();
            server.PlaceholderLoadPosts();
            server.PlaceholderLoadAccounts();
            server.Start();
        }
    }
}
