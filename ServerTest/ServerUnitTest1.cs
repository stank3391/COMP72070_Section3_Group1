using System.Text;

namespace ServerTest
{
    [TestClass]
    public class ServerUnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {

            char[] words = { 'H', 'E', 'L', 'L', 'O', };
            //Packet p = new Packet(Packet.Type.Post, false, Encoding.ASCII.GetBytes(words));
            Packet p = new Packet("69", Packet.Type.Post, Encoding.ASCII.GetBytes(words));
            byte[] expected;

            //Client c = new Client();
            //c.connect();
            byte[] actual = Packet.SerializePacket(p);
            Assert.AreEqual((byte) 'c', actual[0]);
        }
    }
}