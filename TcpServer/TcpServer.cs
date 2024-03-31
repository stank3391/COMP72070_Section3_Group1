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
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Text.Json;
using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Presentation;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
        private string postsPath = "../../../placeholder_db/posts.json";
        private string accountsPath = "../../../placeholder_db/accounts.json";
        private List<Post> posts = new List<Post>(); // list of posts from the database
        private List<Account> accounts = new List<Account>(); // list of accounts from the database

        /// <summary>
        /// Constructor for the TcpServer class
        /// </summary>
        public TcpServer()
        {
            this.listener = new TcpListener(this.localAddress, this.port);
        }

        /// <summary>
        /// save all posts to palceholder database
        /// </summary>
        public void PlaceholderSavePosts()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(posts);

            File.WriteAllText(postsPath, json);

            Console.WriteLine("Posts list saved to placeholder database.");
        }

        /// <summary>
        /// loads all posts from placeholder database to posts list
        /// </summary>
        public void PlaceholderLoadPosts()
        {
            string json = File.ReadAllText(postsPath);

            posts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Post>>(json);

            Console.WriteLine("Posts list loaded from placeholder database.");
        }

        /// <summary>
        /// save all accounts to palceholder database
        /// </summary>
        public void PlaceholderSaveAccounts()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(accounts);

            File.WriteAllText(accountsPath, json);

            Console.WriteLine("Accounts list saved to placeholder database.");
        }

        /// <summary>
        /// loads all accounts from placeholder database to accounts list
        /// </summary>
        public void PlaceholderLoadAccounts()
        {
            string json = File.ReadAllText(accountsPath);

            accounts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Account>>(json);

            Console.WriteLine("Accounts list loaded from placeholder database.");
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
                    Log.CreateLog(Log.fileName, packet.header.sourceId, "Acknowledgement Signal Recieved");

                    break;
                case Packet.Type.Error:
                    Console.WriteLine("TcpServer.HandlePacket(): Error received");
                    Log.CreateLog(Log.fileName, packet.header.sourceId, "Error Signal Recieved");
                    break;
                case Packet.Type.ReadyPost:
                    Console.WriteLine("TcpServer.HandlePacket(): ReadyPost received");
                    Log.CreateLog(Log.fileName, packet.header.sourceId, "ReadyPost Signal Recieved");
                    HandleReadyPostPacket();
                    break;
                case Packet.Type.ReadyImage:
                    Console.WriteLine("TcpServer.HandlePacket(): ReadyImage received");
                    Log.CreateLog(Log.fileName, packet.header.sourceId, "ReadyImage Signal Recieved");
                    HandleReadyImagePacket();
                    break;
                case Packet.Type.Post:
                    Console.WriteLine("TcpServer.HandlePacket(): Post received");
                    Log.CreateLog(Log.fileName, packet.header.sourceId, "Post Signal Recieved");
                    HandlePostPacket(packet);
                    break;
                case Packet.Type.Image:
                    Console.WriteLine("TcpServer.HandlePacket(): Image received");
                    Log.CreateLog(Log.fileName, packet.header.sourceId, "Image Signal Recieved");
                    HandleImagePacket(packet);
                    break;
                case Packet.Type.Auth:
                    Console.WriteLine("TcpServer.HandlePacket(): Auth received");
                    HandleAuthPacket(packet);
                    Log.CreateLog(Log.fileName, packet.header.sourceId, "Auth Signal Recieved");
                    break;
                case Packet.Type.Acc:
                    Console.WriteLine("TcpServer.HandlePacket(): Acc received");
                    HandleAccPacket(packet);
                    Log.CreateLog(Log.fileName, packet.header.sourceId, "Acc Signal Recieved");
                    break;
                default:
                    Console.WriteLine("TcpServer.HandlePacket(): Unknown packet type received");
                    Log.CreateLog(Log.fileName, packet.header.sourceId, "Unknown Signal Recieved");
                    break;
            }

            Console.WriteLine("TcpServer.HandlePacket(): End");
        }

        /// <summary>
        /// Handles the account creation packet
        /// </summary>
        private void HandleAccPacket(Packet packet)
        {
            string bodystring = packet.ToString();
            Console.WriteLine(bodystring);

            // Parse the input string to extract username and password
            var userData = ParseLoginInputString(bodystring);

            // Check if the username already exists in the database
            if (accounts.Any(account => account.username == userData.username))
            {
                Console.WriteLine($"Account creation failed: Username already exists{userData.username}");

                // Send acc fail packet
                Packet response = new Packet("TCP_SERVER", Packet.Type.AccFail);
                byte[] serializedPacket = Packet.SerializePacket(response);
                stream.Write(serializedPacket, 0, serializedPacket.Length);
            }
            else
            {
                // Add the new account to the list
                accounts.Add(new Account(userData.username, userData.password));

                // Save the updated list to the database
                PlaceholderSaveAccounts();

                // Send acc success packet
                Packet response = new Packet("TCP_SERVER", Packet.Type.AccSuccess);
                byte[] serializedPacket = Packet.SerializePacket(response);
                stream.Write(serializedPacket, 0, serializedPacket.Length);

                Console.WriteLine($"Account created successfully: {userData.username}");
            }
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
            Packet packet = new Packet("TCP_SERVER", Packet.Type.Ack, body);
            byte[] serializedPacket = Packet.SerializePacket(packet);
            stream.Write(serializedPacket, 0, serializedPacket.Length);

            // then we start blasting
            for (int i = 0; i < postCount; i++)
            {
                Console.WriteLine($"TcpServer.HandleReadyPostPacket(): Sending post {i + 1} of {postCount}");
                body = posts[i].ToByte();
                packet = new Packet("TCP_SERVER", Packet.Type.Post, body);
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
            Packet packet = new Packet("TCP_SERVER", Packet.Type.Ack, body);
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

        /// <summary>
        /// Handles the post packet
        /// inserts the post to the top of the list
        /// and saves the updated posts list to the database
        /// </summary>
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

        /// <summary>
        /// Handles the image packet
        /// receives image packets and reconstructs the image
        /// saves it to the images folder
        /// </summary>
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

        /// <summary>
        /// Sends an ack packet to the client so client can send the next packet
        /// </summary>
        private void SendAck()
        {
            Packet ackPacket = new Packet("TCP_SERVER", Packet.Type.Ack);
            byte[] serializedPacket = Packet.SerializePacket(ackPacket);
            stream.Write(serializedPacket, 0, serializedPacket.Length);
        }

        /// <summary>
        /// Verifies the login credentials against the database
        /// </summary>
        private bool VerifyLogin(string username, string password)
        {
            // Check if the username and password match any account in the database
            return accounts.Any(account => account.username == username && account.password == password);
        }

        public void HandleAuthPacket(Packet packet)
        {
            //packet.Deserialize
            string bodystring = packet.ToString();
            Console.WriteLine(bodystring);

            // Parse the input string to extract username and password
            var userData = ParseLoginInputString(bodystring);

            if (VerifyLogin(userData.username, userData.password))
            {
                Console.WriteLine("LoginAction successful!");

                // send auth success packet
                Packet response = new Packet("TCP_SERVER", Packet.Type.AuthSuccess);
                byte[] serializedPacket = Packet.SerializePacket(response);
                stream.Write(serializedPacket, 0, serializedPacket.Length);
            }
            else
            {
                Console.WriteLine("Invalid username or password.");

                // send auth fail packet
                Packet reponse = new Packet("TCP_SERVER", Packet.Type.AuthFail);
                byte[] serializedPacket = Packet.SerializePacket(reponse);
                stream.Write(serializedPacket, 0, serializedPacket.Length);
            }
        }

        /// <summary>
        /// Parses the input string to extract username and password
        /// </summary>
        static (string username, string password) ParseLoginInputString(string input)
        {
            // Define a regular expression pattern to match "Username: <username>, Password: <password>"
            string pattern = @"Username:\s*(?<username>\w+),\s*Password:\s*(?<password>\w+)";

            // remove whitespace from the input string
            input = input.Replace(" ", "");

            // Match the input string against the pattern
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                // Extract username and password from the named groups in the match
                string username = match.Groups["username"].Value;
                string password = match.Groups["password"].Value;

                return (username, password);
            }
            else
            {
                // Handle invalid input format
                throw new ArgumentException("Invalid input format");
            }
        }
    }
}