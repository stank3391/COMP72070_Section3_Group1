using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using COMP72070_Section3_Group1.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
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
            byte[] bufferIn = new byte[Packet.MAX_PACKET_SIZE]; // buffer for incoming data

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
        public void HandlePacket(Packet packet)
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
                case Packet.Type.Post:
                    Console.WriteLine("TcpServer.HandlePacket(): Post received");
                    HandlePostPacket(packet);
                    break;
                case Packet.Type.Image:
                    Console.WriteLine("TcpServer.HandlePacket(): Image received");
                    HandleImagePacket(packet);
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
            int postCount = 0;
            if (posts != null)
            {
                postCount = posts.Count;
            }
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

                // wait for ack packet
                this.WaitForAck();
            }

            Console.WriteLine("TcpServer.HandleReadyPostPacket(): End");
        }

        private void HandleReadyImagePacket()
        {
            Console.WriteLine("TcpServer.HandleReadyImagePacket(): Start");
            string[] files = Directory.GetFiles("../../../images/");

            int imageCount = files.Length;
            byte[] body = Encoding.ASCII.GetBytes(imageCount.ToString());
            Packet packet = new Packet("SERVER", Packet.Type.Ack, body);
            byte[] serializedPacket = Packet.SerializePacket(packet);
            stream.Write(serializedPacket, 0, serializedPacket.Length);

            // then we start blasting
            for (int i = 0; i < imageCount; i++)
            {
                Console.WriteLine($"TcpServer.HandleReadyImagePacket(): Sending image {i + 1} of {imageCount}");
                byte[] imageData = File.ReadAllBytes(files[i]);
                string fileName = Path.GetFileName(files[i]);
                Console.WriteLine($"TcpServer.HandleReadyImagePacket(): Sending image {fileName}");
                Packet[] packets = Packet.CreateImagePackets(fileName, imageData);
                foreach (Packet p in packets)
                {
                    serializedPacket = Packet.SerializePacket(p);
                    stream.Write(serializedPacket, 0, serializedPacket.Length);

                    // wait for ack packet
                    this.WaitForAck();
                }
            }
            Console.WriteLine("TcpServer.HandleReadyImagePacket(): End");
        }


        private void HandlePostPacket(Packet packet)
        {
            Console.WriteLine("TcpServer.HandlePostPacket(): Start");

            // deserialize the packet body into a post
            Post post = new Post(packet.body);

            // add to top of list
            posts.Insert(0, post);
            
            // save the posts to the database
            this.PlaceholderSavePosts();

            Console.WriteLine("TcpServer.HandlePostPacket(): End");
        }

        private void HandleImagePacket(Packet packet)
        {
            Console.WriteLine("TcpServer.HandleImagePacket(): Start");

            List<Packet> imagePackets = new List<Packet>();
            imagePackets.Add(packet);

            // send ack packet
            this.SendAck();

            while(packet.header.packetNumber + 1 < packet.header.totalPackets) 
            {
                // receive next packet
                byte[] bufferIn = new byte[Packet.MAX_PACKET_SIZE];
                this.stream.Read(bufferIn, 0, bufferIn.Length);

                packet = Packet.DeserializePacket(bufferIn);
                imagePackets.Add(packet);

                // send ack packet
                this.SendAck();
            }

            byte[] imageData = Packet.ReconstructImage(imagePackets.ToArray());
            string imagePath = Path.Combine("../../../images/", packet.header.sourceId);
            File.WriteAllBytes(imagePath, imageData);
            
            Console.WriteLine("TcpServer.HandleImagePacket(): End");
        }

        /// <summary>
        /// Waits for an ack packet from the client
        /// so that we can send the next packet
        /// </summary>
        private void WaitForAck()
        {
            byte[] bufferIn = new byte[Packet.MAX_PACKET_SIZE];
            this.stream.Read(bufferIn, 0, bufferIn.Length);
            Packet ackPacket = Packet.DeserializePacket(bufferIn);
            if (ackPacket.header.packetType != Packet.Type.Ack)
            {
                Console.WriteLine("TcpServer.WaitForAck(): Error receiving ack packet");
            }
        }

        private void SendAck()
        {
            Packet ackPacket = new Packet("SERVER", Packet.Type.Ack);
            byte[] serializedPacket = Packet.SerializePacket(ackPacket);
            stream.Write(serializedPacket, 0, serializedPacket.Length);
        }


        /// <summary>
        /// Fetches all the posts from the database and stores them in 'posts' property
        /// will replace with database connection later
        /// NOT USED ANYMORE
        /// </summary>
        public void UpdatePosts()
        {
            // update the posts from the database

            // just return some dummy posts for now
            posts.Add(new Post(1, "HEELOO!!! This should display placeholder image", "user1", DateTime.Now, "testimg1.jpg"));

            posts.Add(new Post(2, "HEELOO!!! I am a post2", "user2", DateTime.Now, "testimg2.jpg"));

            posts.Add(new Post(3, "HEELOO!!! I am a post3", "user3", DateTime.Now, "testimg3.jpg"));

            posts.Add(new Post(4, "HEELOO!!! I am a post4", "user4", DateTime.Now, "testimg4.jpg"));

            posts.Add(new Post(5, "HEELOO!!! I am a post5", "user5", DateTime.Now, "testimg5.jpg"));

            posts.Add(new Post(6, "HEELOO!!! I am a post6", "user6", DateTime.Now, "testimg6.jpg"));

            posts.Add(new Post(7, "HEELOO!!! I am a post7", "user7", DateTime.Now, "testimg7.jpg"));

            posts.Add(new Post(8, "HEELOO!!! I am a post8", "user8", DateTime.Now, "testimg8.jpg"));

            posts.Add(new Post(9, "HEELOO!!! I am a post9", "user9", DateTime.Now, "testimg9.jpg"));

            Console.WriteLine("Posts list updated from 'Database'.");

        }

        /// <summary>
        /// save all posts to palceholder database
        /// </summary>
        public void PlaceholderSavePosts()
        {
            string path = "../../../../TcpServer/placeholder_db/posts.json";

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(posts);

            File.WriteAllText(path, json);

            Console.WriteLine("Posts list saved to placeholder database.");
        }

        /// <summary>
        /// loads all posts from placeholder database
        /// </summary>
        public void PlaceholderLoadPosts()
        {
            string path = "../../../../TcpServer/placeholder_db/posts.json";

            string json = File.ReadAllText(path);

            posts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Post>>(json);

            Console.WriteLine("Posts list loaded from placeholder database.");
        }


        public void HandleAuthPacket(Packet packet)
        {
            //packet.Deserialize
            string path = "../../../placeholder_db/accounts.json";
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(posts);
        }
    }
}


