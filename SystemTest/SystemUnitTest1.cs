using System;
using System.Text;
using TcpServer;

namespace SystemTest
{
    [TestClass]
    public class SystemUnitTest1
    {
        [TestMethod]
        public void SYS_015_a_PacketHeaderBody_empty()
        {
            //This test verifies that the packet consists of a header and body
            //Arange
            Packet p;
            //Act
            p = new Packet();
            //Assert
            Assert.IsNotNull(p.body);
            Assert.IsNotNull(p.header);
        }

        [TestMethod]
        public void SYS_015_b_PacketHeaderBody_notEmpty()
        {
            //This test verifies that the packet consists of a header and body with a constructed packet
            char[] words = { 'H', 'E', 'L', 'L', 'O', };
            Packet p = new Packet("69", Packet.Type.Post, Encoding.ASCII.GetBytes(words));
            //Assert
            Assert.IsNotNull(p.body);
            Assert.IsNotNull(p.header);
        }
        [TestMethod]
        public void SerializationOfPackets()
        {
            char[] words = { 'H', 'E', 'L', 'L', 'O', };
            Packet p = new Packet("69", Packet.Type.Post, Encoding.ASCII.GetBytes(words));
            byte[] serializedPacket = Packet.SerializePacket(p);
            Packet packetIn = Packet.DeserializePacket(serializedPacket);
            Assert.AreEqual(p.ToString(), packetIn.ToString());
        }
        [TestMethod]
        public void PacketNullValue()
        {
            char[] words = { 'H', 'E', 'L', 'L', 'O', };
            Packet p = new Packet("69", Packet.Type.Post, Encoding.ASCII.GetBytes(words));
            p.body = null;
            //byte[] serializedPacket = Packet.SerializePacket(p);
            //Packet packetIn = Packet.DeserializePacket(serializedPacket);
            Assert.AreEqual(p.header.bodyLen, 0);
        }
    }
}