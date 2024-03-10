using COMP72070_Section3_Group1.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace COMP72070_Section3_Group1.Controllers
{
    public class Packet
    {
        public enum Type { Post, DM, Auth, Acc };

        public struct Header
        {
            public int msgLen { get; set; }
            public int Source { get; set; } // Change this datatype
            public Type messageType { get; set; }
            public bool pictureFlag { get; set; }
        } 

        public Header header;
        public byte[] Body {
            get { return Body; }
            set {
                this.Body = value;
                this.header.msgLen = value.Length;
            }
        }

        // Constructor
        public Packet()
        {
            Body = null;
        }

        public static byte[] SerializePacket(Packet packet)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(memoryStream))
                {
                    // Serialize the packet header
                    writer.Write(packet.header.msgLen);
                    writer.Write(packet.header.Source);
                    writer.Write((int)packet.header.messageType); // Convert enum to int
                    writer.Write(packet.header.pictureFlag);

                    // Serialize the packet body
                    writer.Write(packet.Body);

                    // Convert the MemoryStream to a byte array
                    return memoryStream.ToArray();
                }
            }
        }
    }
}
