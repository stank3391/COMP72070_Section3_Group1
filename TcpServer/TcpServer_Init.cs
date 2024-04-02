using COMP72070_Section3_Group1.Models;
using System.Text;

namespace TcpServer
{
    /// <summary>
    /// Partial class for the TcpServer
    /// Handles the initialization of the server and the database
    /// Sends the and updates the list of posts and images available to the client
    /// </summary>
    public partial class TcpServer
    {
        // database props
        public string postsPath = "../../../placeholder_db/posts.json"; // path to the posts database
        public string accountsPath = "../../../placeholder_db/accounts.json"; // path to the accounts database
        public List<Post> posts = new List<Post>(); // list of posts from the database
        public List<Account> accounts = new List<Account>(); // list of accounts from the database

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

            if (posts == null)
            {
                posts = new List<Post>();
            }

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

            if (accounts == null)
            {
                accounts = new List<Account>();
            }

            Console.WriteLine("Accounts list loaded from placeholder database.");
        }
        /// <summary>
        /// Handles the ready post packet
        /// send all posts to the client
        /// </summary>
        public void HandleReadyPostPacket()
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
            this.SendPacket(packet);

            // then we start blasting
            for (int i = 0; i < postCount; i++)
            {
                Console.WriteLine($"TcpServer.HandleReadyPostPacket(): Sending post {i + 1} of {postCount}");
                body = posts[i].ToByte();
                packet = new Packet("TCP_SERVER", Packet.Type.Post, body);
                this.SendPacket(packet);

                // wait for ack packet
                this.WaitForAck();
            }

            Console.WriteLine("TcpServer.HandleReadyPostPacket(): End");
            Log.CreateLog(Log.ServerLogName, "TCP_SERVER", $"Posts sent to client. Total count: {postCount}");
        }

        public void HandleReadyImagePacket()
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
    }
}