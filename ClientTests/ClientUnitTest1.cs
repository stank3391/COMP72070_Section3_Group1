using System.Net.Sockets;
using COMP72070_Section3_Group1.Models;
using Microsoft.Extensions.Hosting;

namespace ClientTests
{
    [TestClass]
    public class ClientUnitTest1
    {
        [TestMethod]
        public void CLT_001_Create_Posts_Dynamic_Test_Assert_By_Viewing()
        {
            //==================ARRANGE===============================
            StreamReader sr = new StreamReader("C:\\Users\\Sean Tank\\Desktop\\PostCreateTests.txt");
            //Read the first line of text
            String line = sr.ReadLine();
            //Start up obj for server. doesn't need to connect to client for tear
            TcpServer.TcpServer tcpServer = new TcpServer.TcpServer();
            //Required for appending. Instead of replacing
            tcpServer.PlaceholderLoadPosts();

            //=====================ACT=============================
            //Read my file line-by-line
            while (line != null) {
                String[] words = line.Split(",");
                Post post;
                //Null check only required in my test stub. front end will handle this normally
                if (words[2] == "null")
                {
                    post = new Post(1, words[0], words[1], DateTime.Now);
                }
                else { 
                    post = new Post(1, words[0], words[1], DateTime.Now, words[2]);
                }
                Packet packet = new Packet("CLT_002", Packet.Type.Post, post.ToByte());
                // HandlePacket(packet);
                tcpServer.HandlePacket(packet);
                //===============ASSERT BY VEIWING WEBSITE THIS IS A POST============
                line = sr.ReadLine();
            }
        }
    }
}