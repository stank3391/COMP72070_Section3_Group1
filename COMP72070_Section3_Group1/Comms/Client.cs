using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using COMP72070_Section3_Group1.Models;

namespace COMP72070_Section3_Group1.Comms
{
    // Represents a client that connects to the server
    public class Client
    {
        public TcpClient tcpClient { get; private set; } // The TcpClient object 
        public NetworkStream stream { get; private set; } // The NetworkStream object

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

        public void Connect()
        {
            Console.WriteLine("Client.Connect(): Start");
            this.tcpClient.Connect(serverIp, serverPort);
            this.stream = this.tcpClient.GetStream();

            if (this.tcpClient.Connected)
            {
                Console.WriteLine("Client.Connect(): Connected to server");
            }
            else
            {
                Console.WriteLine("Client.Connect(): ERROR: Failed to Connect to server");
            }
            Console.WriteLine("Client.Connect(): End");
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
        /// Sends a packet
        /// </summary>
        public void SendPacket(Packet packet)
        {
            //Console.WriteLine("Client.SendPacket(): Start");
            // serialize the packet
            byte[] serializedPacket = Packet.SerializePacket(packet);

            // send the packet
            this.stream.Write(serializedPacket, 0, serializedPacket.Length);

            Console.WriteLine($"Client.SendPacket(): Packet sent:\n{packet.ToString()}");
            //Console.WriteLine("Client.SendPacket(): End");
        }

        /// <summary>
        /// Receives a packet
        /// </summary>
        public Packet ReceivePacket()
        {
            //Console.WriteLine("Client.ReceivePacket(): Start");

            // receive the packet from the client
            byte[] buffer = new byte[Packet.MAX_PACKET_SIZE];
            this.stream.Read(buffer, 0, buffer.Length);

            // deserialize the packet
            Packet packet = Packet.DeserializePacket(buffer);

            //Console.WriteLine($"Client.SendPacket(): Packet received:\n{packet.ToString()}");
            //Console.WriteLine("Client.ReceivePacket(): End");

            return packet;
        }

        /// <summary>
        /// sends a simple ack packet to the server
        /// used to acknowledge the receipt of a packet so no need to wait between sending and receiving
        /// </summary>
        public void SendAck()
        {
            Packet ackPacket = new Packet("CLIENT", Packet.Type.Ack);
            SendPacket(ackPacket);
        }

        /// <summary>
        /// send a post to the server
        /// </summary>
        public void SendPost(int sourceId, Post post)
        {
            //Console.WriteLine("Client.SendPost(): Start");
            Packet packet = new Packet(sourceId.ToString(), Packet.Type.Post, post.ToByte());
            SendPacket(packet);

            //Console.WriteLine($"Client.SendPost(): Post sent: {post.content}");
            //Console.WriteLine("Client.SendPost(): End");
        }

        /// <summary>
        /// receive a post from the server and return it
        /// </summary>
        public Post ReceivePost()
        {
            //Console.WriteLine("Client.ReceivePost(): Start");
            Packet postPacket = ReceivePacket();

            if (postPacket.header.packetType != Packet.Type.Post)
            {
                Console.WriteLine($"Client.ReceivePost(): ERROR: Expecting POST packet, but '{postPacket.header.packetType}' received");
                return null;
            }

            Post post = new Post(postPacket.body);

            Console.WriteLine($"Post received: {post.content}");

            //Console.WriteLine("Client.ReceivePost(): End");
            return post;
        }

        /// <summary>
        /// Send a ready packet to the server and get all posts to store in list
        /// param: posts - the singleton list of posts to store the posts received from the server
        /// </summary>
        public void FetchPosts(List<Post> posts)
        {
            Console.WriteLine("Client.FetchPosts(): Start");
            // send a ready packet to the server
            Packet readyPacket = new Packet("CLIENT", Packet.Type.ReadyPost);
            this.SendPacket(readyPacket);

            // receive the ack packet
            Packet ackPacket = this.ReceivePacket();

            // get the number of posts from the ack packet
            string body = Encoding.ASCII.GetString(ackPacket.body);
            int postCount = int.Parse(body);

            // receive all the posts
            for (int i = 0; i < postCount; i++)
            {
                Console.WriteLine($"Client.FetchPosts(): Receiving post {i + 1} of {postCount}");

                Post post = this.ReceivePost();

                posts.Add(post);

                // send ack packet
                this.SendAck();
            }

            Console.WriteLine("Client.FetchPosts(): End");
        }

        public void FetchImages()
        {
            Console.WriteLine("Client.FetchImages(): Start");

            // send a ready packet to the server
            Packet readyPacket = new Packet("CLIENT", Packet.Type.ReadyImage);
            this.SendPacket(readyPacket);

            // receive the ack packet
            Packet packetIn = this.ReceivePacket();

            // get the number of images from the ack packet
            string body = Encoding.ASCII.GetString(packetIn.body);
            int imageCount = int.Parse(body);

            // receive all the images
            List<Packet> imagePackets = new List<Packet>();
  
            Packet ackPacket = new Packet("CLIENT", Packet.Type.Ack);
            for (int i = 0; i < imageCount; i++)
            {
                Console.WriteLine($"Client.FetchImages(): Receiving image {i + 1} of {imageCount}");
                
                // receive first packet
                Packet imagePacket = this.ReceivePacket();

                imagePackets.Add(imagePacket); // add to list

                // send ack packet
                this.SendPacket(ackPacket);

                while(imagePacket.header.packetNumber + 1 < imagePacket.header.totalPackets)
                {
                    // receive next packet
                    imagePacket = this.ReceivePacket();

                    imagePackets.Add(imagePacket); // add to list

                    // send ack packet
                    this.SendPacket(ackPacket);
                }

                byte[] imageData = Packet.ReconstructImage(imagePackets.ToArray());

                string imagePath = Path.Combine("./wwwroot/images/", imagePacket.header.sourceId);
                File.WriteAllBytes(imagePath, imageData);

                // clear list
                imagePackets.Clear();
            }   


            Console.WriteLine("Client.FetchImages(): End");
        }
    }
}