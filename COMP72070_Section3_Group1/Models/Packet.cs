using COMP72070_Section3_Group1.Controllers;
using static System.Runtime.InteropServices.JavaScript.JSType;



public class Packet
{
    // definitions: 
    public enum Type { Post, DM, Auth, Acc };

    public struct Header
    {
        public int msgLen { get; set; }
        public int source { get; set; };// Change this datatype
        public Type messageType { get; set; }
        public bool pictureFlag { get; set; }
    } 

    // properties
    public Header header;

    public byte[] body 
    {
        get
        {
            return body;
        }
        set
        {
            body = value;
            header.msgLen = body.Length;
        }
    }

    // constructor
    public Packet()
    {
      
    }
    public Packet(Type messageType,bool pictureFlag, byte[] body)
    {
        this.header.source = 69;
        this.header.messageType = messageType;
        this.header.pictureFlag = pictureFlag;
        this.body = body;
    }

    public static byte[] SerializePacket(Packet packet)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(memoryStream))
            {
                // Serialize the packet header
                writer.Write(packet.header.msgLen);
                writer.Write(packet.header.source);
                writer.Write((int)packet.header.messageType);
                writer.Write(packet.header.pictureFlag);

                // Serialize the packet body
                writer.Write(packet.body);

                return memoryStream.ToArray();
            }
        }
    }

    public static Packet DeserializePacket(byte[] data)
    {
        Packet packet = new Packet();
        using (MemoryStream memoryStream = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(memoryStream))
            {
                // Deserialize the packet header
                packet.header.msgLen = reader.ReadInt32();
                packet.header.source = reader.ReadInt32();
                packet.header.messageType = (Type)reader.ReadInt32();
                packet.header.pictureFlag = reader.ReadBoolean();

                // Deserialize the packet body
                packet.body = reader.ReadBytes(packet.header.msgLen);
            }
        }
        return packet;
    }

    public override string ToString()
    {
        string str = "Packet:\n";
        str += "Message Length: " + header.msgLen + "\n";
        str += "Source: " + header.source + "\n";
        str += "Message Type: " + header.messageType + "\n";
        str += "Picture Flag: " + header.pictureFlag + "\n";
        str += "Body: " + body + "\n";
        return str;
    }
}

