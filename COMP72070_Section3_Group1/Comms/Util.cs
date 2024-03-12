using Microsoft.CodeAnalysis.CSharp.Syntax;
using COMP72070_Section3_Group1.Models;
using System.Text;

namespace COMP72070_Section3_Group1.Comms
{
    public class Util
    {

        /// <summary>
        /// Send a ready packet to the server and get all posts to store in property
        /// param: client - the client object used to send and receive packets
        /// param: posts - the singleton list of posts to store the posts received from the server
        /// </summary>
        public void FetchPosts(Client client, List<Post> posts)
        {
            // send a ready packet to the server
            Packet readyPacket = new Packet("CLIENT", Packet.Type.Ready);
            client.SendPacket(readyPacket);

            // receive the ack packet
            Packet ackPacket = client.ReceivePacket();

            // get the number of posts from the ack packet
            string body = Encoding.ASCII.GetString(ackPacket.body);
            int postCount = int.Parse(body);

            // receive all the posts
            for (int i = 0; i < postCount; i++)
            {
                Console.WriteLine($"Receiving post {i + 1} of {postCount}");

                Packet postPacket = client.ReceivePacket();

                if (postPacket.header.messageType != Packet.Type.Post)
                {
                    Console.WriteLine($"Expecting POST packet, but this was received:\n{postPacket.ToString()}");
                    return;
                }
                Post post = new Post(postPacket.body);
                posts.Add(post);
            }
        }

    }
}
