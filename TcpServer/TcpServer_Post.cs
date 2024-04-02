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
            Log.CreateLog(Log.ServerLogName, packet.header.sourceId, $"Post received from client. Author: {post.author}");
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
                packet = this.ReceivePacket();
                imagePackets.Add(packet);

                // send ack packet
                this.SendAck();
            }

            byte[] imageData = Packet.ReconstructImage(imagePackets.ToArray());
            string imagePath = Path.Combine("../../../images/", packet.header.sourceId);
            File.WriteAllBytes(imagePath, imageData);
            
            Console.WriteLine("TcpServer.HandleImagePacket(): End");

            Log.CreateLog(Log.ServerLogName, packet.header.sourceId, $"Image received from client. Path: {imagePath}");
        }
    }
}