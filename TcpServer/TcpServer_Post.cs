using COMP72070_Section3_Group1.Models;
using System.Text;

namespace TcpServer
{
    /// <summary>
    /// Partial class for the TcpServer
    /// handles the post packets and image packets 
    /// </summary>
    public partial class TcpServer
    {
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
        /// <summary>
        /// Handles the post packet
        /// inserts the post to the top of the list
        /// and saves the updated posts list to the database
        /// </summary>
        public void HandlePostPacket(Packet packet)
        {
            Console.WriteLine("TcpServer.HandlePostPacket(): Start");

            // deserialize the packet body into a post
            Post post = new Post(packet.body);

            // add to top of list
            posts.Insert(0, post);
            
            // save the posts to the database
            this.PlaceholderSavePosts();

            // send ack packet
            this.SendAck();

            Console.WriteLine("TcpServer.HandlePostPacket(): End");
        }

        /// <summary>
        /// Handles the image packet
        /// receives image packets and reconstructs the image
        /// saves it to the images folder
        /// </summary>
        public void HandleImagePacket(Packet packet)
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
    }
}