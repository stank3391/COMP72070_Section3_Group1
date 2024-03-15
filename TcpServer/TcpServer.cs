using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using COMP72070_Section3_Group1.Models;
using COMP72070_Section3_Group1.Visitors;
using Microsoft.AspNetCore.Routing.Constraints;

namespace TcpServer
{
    public class TcpServer
    {
        // networking props
        private TcpListener listener;
        private int port = 27000;
        private IPAddress localAddress = IPAddress.Any;
        private TcpClient tcpClient;
        private NetworkStream stream;

        private bool isStop = false; // flag to stop the server

        // database props
        private List<Post> posts = new List<Post>(); // list of posts from the database



        /// <summary>
        /// Constructor for the TcpServer class
        /// </summary>
        public TcpServer()
        {
            this.listener = new TcpListener(this.localAddress, this.port);
        }

        /// <summary>
        /// Listens and connects to clients. 
        /// load the posts from the database
        /// </summary>
        public void Start()
        {
            Console.WriteLine("TcpServer.Start(): Start");
            while (!this.isStop)
            {
                Console.WriteLine("TcpServer.Start(): Listenting");
                this.listener.Start();

                this.tcpClient = this.listener.AcceptTcpClient();
                Console.WriteLine("TcpServer.Start(): Connected");

                this.stream = this.tcpClient.GetStream();

                HandleClient();
            }
            Console.WriteLine("TcpServer.Start(): End");
        }

        /// <summary>
        /// MAIN FUNCTION TO COMMUNICATE WITH CLIENT!
        /// </summary>
        private void HandleClient()
        {
            Console.WriteLine("TcpServer.HandleClient(): Start");
            byte[] bufferIn = new byte[1024]; // buffer for incoming data

            bool isDisconnect = false;

            while (!isDisconnect)
            {
                // receive data 
                this.stream.Read(bufferIn, 0, bufferIn.Length);
                Packet packetIn = Packet.DeserializePacket(bufferIn);
                HandlePacket(packetIn);

                if (!tcpClient.Connected)
                {
                    Console.WriteLine($"TcpServer.HandleClient(): Disconnected");
                    tcpClient.Close();
                    isDisconnect = true;
                }
            }
            Console.WriteLine("TcpServer.HandleClient(): End");
        }

        /// <summary>
        /// Handles the packet received from the client
        /// determines the type of packet and do action
        /// </summary>
        private void HandlePacket(Packet packet)
        {
            Console.WriteLine("TcpServer.HandlePacket(): Start");

            Packet.Type type = packet.header.packetType;
            
            switch (type)
            {
                case Packet.Type.Ack:
                    Console.WriteLine("TcpServer.HandlePacket(): Ack received");
                    break;
                case Packet.Type.Error:
                    Console.WriteLine("TcpServer.HandlePacket(): Error received");
                    break;
                case Packet.Type.ReadyPost:
                    Console.WriteLine("TcpServer.HandlePacket(): ReadyPost received");
                    HandleReadyPostPacket();
                    break;
                case Packet.Type.ReadyImage:
                    Console.WriteLine("TcpServer.HandlePacket(): ReadyImage received");
                    HandleReadyImagePacket();
                    break;
                case Packet.Type.Auth:
                    Console.WriteLine("TcpServer.HandlePacket(): Auth received");
                    break;
                default:
                    Console.WriteLine("TcpServer.HandlePacket(): Unknown packet type received");
                    break;
            }

            Console.WriteLine("TcpServer.HandlePacket(): End");
        }

        /// <summary>
        /// Handles the ready post packet
        /// send all posts to the client
        /// </summary>
        private void HandleReadyPostPacket()
        {
            Console.WriteLine("TcpServer.HandleReadyPostPacket(): Start");
            // send ack packet with the total number of posts as the body
            int postCount = posts.Count;
            byte[] body = Encoding.ASCII.GetBytes(postCount.ToString());
            Packet packet = new Packet("SERVER", Packet.Type.Ack, body);
            byte[] serializedPacket = Packet.SerializePacket(packet);
            stream.Write(serializedPacket, 0, serializedPacket.Length);

            // then we start blasting
            for (int i = 0; i < postCount; i++)
            {
                Console.WriteLine($"TcpServer.HandleReadyPostPacket(): Sending post {i + 1} of {postCount}");
                body = posts[i].ToByte();
                packet = new Packet("SERVER", Packet.Type.Post, body);
                serializedPacket = Packet.SerializePacket(packet);
                stream.Write(serializedPacket, 0, serializedPacket.Length);

                // wait for 10 ms
                System.Threading.Thread.Sleep(100); // MUST BE HERE OR ELSE ASP BREAKS (wont recevie all packets)
            }

            Console.WriteLine("TcpServer.HandleReadyPostPacket(): End");
        }

        public void HandleReadyImagePacket()
        {
            Console.WriteLine("TcpServer.HandleReadyImagePacket(): Start");



            Console.WriteLine("TcpServer.HandleReadyImagePacket(): End");
        }


        /// <summary>
        /// Fetches all the posts from the database and stores them in 'posts' property
        /// will replace with database connection later
        /// </summary>
        public void UpdatePosts()
        {
            // update the posts from the database

            // just return some dummy posts for now
            posts.Add(new Post(1, "HEELOO!!! I am a post1", "user1", DateTime.Now,""));

            posts.Add(new Post(2, "HEELOO!!! I am a post2", "user2", DateTime.Now,""));

            posts.Add(new Post(3, "HEELOO!!! I am a post3", "user3", DateTime.Now, ""));

            posts.Add(new Post(4, "HEELOO!!! I am a post4", "user4", DateTime.Now, ""));

            posts.Add(new Post(5, "HEELOO!!! I am a post5", "user5", DateTime.Now, ""));

            Console.WriteLine("Posts list updated from 'Database'.");

        }
    }
}


