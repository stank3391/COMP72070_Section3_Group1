using System.Text;
using System.IO;



public class Packet
{
    // definitions: 
    public enum Type { Post, DM, Auth, Acc };

    public struct Header
    {
        public int bodyLen { get; set; }
        public int source { get; set; } // Change this datatype
        public Type messageType { get; set; }
        public bool pictureFlag { get; set; }
    } 

    // properties
    public Header header;

    private byte[] _body = null;

    public byte[] body
    {
        get
        {
            return _body;
        }

        set
        {
            _body = value;
            header.bodyLen = _body.Length;
        }
    }

    // constructor
    public Packet()
    {
      // nothing
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
                writer.Write(packet.header.bodyLen);
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
                packet.header.bodyLen = reader.ReadInt32();
                packet.header.source = reader.ReadInt32();
                packet.header.messageType = (Type)reader.ReadInt32();
                packet.header.pictureFlag = reader.ReadBoolean();

                // Deserialize the packet body
                packet.body = reader.ReadBytes(packet.header.bodyLen);
            }
        }
        return packet;
    }

    public override string ToString()
    {
        string str = "Packet:\n";
        str += "Message Length: " + header.bodyLen + "\n";
        str += "Source: " + header.source + "\n";
        str += "Message Type: " + header.messageType + "\n";
        str += "Picture Flag: " + header.pictureFlag + "\n";
        string bodyStr = Encoding.ASCII.GetString(body);
        str += "Body: " + bodyStr + "\n";
        return str;
    }
}

