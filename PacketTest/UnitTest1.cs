using COMP72070_Section3_Group1.Comms;
using Microsoft.AspNetCore.Routing;

namespace PacketTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void SerializePacket_ReturnsByteArray()
        {
            // Arrange
            Packet packet = new Packet("sourceId", Packet.Type.Post, new byte[] { 1, 2, 3 });

            // Act
            byte[] serializedPacket = Packet.SerializePacket(packet);

            // Assert
            Assert.IsNotNull(serializedPacket);
            Assert.AreEqual(28, serializedPacket.Length);
        }

        [TestMethod]
        public void DeserializePacket_ReturnsPacket()
        {
            // Arrange
            Packet packet = new Packet("sourceId", Packet.Type.Post, new byte[] { 1, 2, 3 });
            byte[] serializedPacket = Packet.SerializePacket(packet);

            // Act
            Packet deserializedPacket = Packet.DeserializePacket(serializedPacket);

            // Assert
            Assert.IsNotNull(deserializedPacket);
            Assert.AreEqual(packet.header.sourceId, deserializedPacket.header.sourceId);
            Assert.AreEqual(packet.header.packetType, deserializedPacket.header.packetType);
            Assert.AreEqual(packet.header.packetNumber, deserializedPacket.header.packetNumber);
            Assert.AreEqual(packet.header.totalPackets, deserializedPacket.header.totalPackets);
            CollectionAssert.AreEqual(packet.body, deserializedPacket.body);

        }

        [TestMethod]
        public void CreateImagePackets_ReturnsPackets()
        {
            // Arrange
            string sourceId = "image.jpg";
            byte[] imageData = new byte[8192];
            int maxImageSize = 4096;

            // Act
            Packet[] packets = Packet.CreateImagePackets(sourceId, imageData, maxImageSize);

            // Assert
            Assert.IsNotNull(packets);
            Assert.AreEqual(2, packets.Length);
            Assert.AreEqual(sourceId, packets[0].header.sourceId);
            Assert.AreEqual(Packet.Type.Image, packets[0].header.packetType);
            Assert.AreEqual(0, packets[0].header.packetNumber);
            Assert.AreEqual(2, packets[0].header.totalPackets);
        }

        [TestMethod]
        public void ReconstructImage_ReturnsImageData()
        {
            // Arrange
            Packet[] packets = new Packet[]
            {
            new Packet("image.jpg", Packet.Type.Image, new byte[] { 1, 2, 3 }, 0, 3),
            new Packet("image.jpg", Packet.Type.Image, new byte[] { 4, 5, 6 }, 1, 3),
            new Packet("image.jpg", Packet.Type.Image, new byte[] { 7, 8, 9 }, 2, 3)
            };

            // Act
            byte[] reconstructedImage = Packet.ReconstructImage(packets);

            // Assert
            Assert.IsNotNull(reconstructedImage);
            Assert.AreEqual(9, reconstructedImage.Length);
        }
    }
}