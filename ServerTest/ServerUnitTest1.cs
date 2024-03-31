using System.Text;

namespace ServerTest
{
    [TestClass]
    public class ServerUnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            //char[] words = { 'H', 'E', 'L', 'L', 'O', };
            //Packet p = new Packet("69", Packet.Type.Post, Encoding.ASCII.GetBytes(words));
            //byte[] expected;

            ////Client c = new Client();
            ////c.connect();
            //byte[] actual = Packet.SerializePacket(p);
            //for (int i = 0; i < words.Length; i++)
            //{
            //    Assert.AreEqual((byte) words[i], actual[i]);
            //}
        }
        [TestMethod]
        public void ServerMainTest()
        {
            //TcpServer.Program.Main();
            //TcpServer.TcpServer tcpServer = new TcpServer.TcpServer();
        }
        //[TestMethod]
        //public void TcpServerTestPlaceholderSavePosts()
        //{
        //    TcpServer.TcpServer myServ = new TcpServer.TcpServer();
        //    myServ.PlaceholderSavePosts();
        //}
        //[TestMethod]
        //public void TcpServerTestPlaceholderLoadPosts()
        //{
        //    TcpServer.TcpServer myServ = new TcpServer.TcpServer();
        //    myServ.PlaceholderLoadPosts();
        //}
    }
}