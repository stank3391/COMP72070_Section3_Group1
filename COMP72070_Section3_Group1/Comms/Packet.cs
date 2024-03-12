using System.Text;
using System.IO;

public class Packet
{
    // definitions: 
    public enum Type { Post, DM, Auth, Acc, Ack, Test };

    public struct Header
    {
        public int bodyLen { get; set; }
        public string sourceId { get; set; } // id of the visitor
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

    /// <summary>
    /// default constructor
    /// </summary>
    public Packet()
    {
        // nothing
    }

    /// <summary>
    /// constructs a packet with byte[] as body
    /// </summary>
    public Packet(string sourceId, Type messageType, bool pictureFlag, byte[] body)
    {
        this.header.sourceId = sourceId;
        this.header.messageType = messageType;
        this.header.pictureFlag = pictureFlag;
        this.body = body;
    }

    /// <summary>
    /// constructs a packet with string as body
    /// </summary>
    public Packet(string sourceId, Type messageType, bool pictureFlag, string body)
    {
        this.header.sourceId = sourceId;
        this.header.messageType = messageType;
        this.header.pictureFlag = pictureFlag;
        this.body = Encoding.ASCII.GetBytes(body);

    }

    /// <summary>
    /// Serializes a packet into a byte array
    /// </summary>
    public static byte[] SerializePacket(Packet packet)
    {
        using (MemoryStream memoryStream = new MemoryStream())
        {
            using (BinaryWriter writer = new BinaryWriter(memoryStream))
            {
                // Serialize the packet header
                writer.Write(packet.header.bodyLen);
                writer.Write(packet.header.sourceId);
                writer.Write((int)packet.header.messageType);
                writer.Write(packet.header.pictureFlag);

                // Serialize the packet body
                writer.Write(packet.body);

                return memoryStream.ToArray();
            }
        }
    }


    /// <summary>
    /// Deserializes a byte array into a packet
    /// </summary>
    public static Packet DeserializePacket(byte[] data)
    {
        Packet packet = new Packet();
        using (MemoryStream memoryStream = new MemoryStream(data))
        {
            using (BinaryReader reader = new BinaryReader(memoryStream))
            {
                // Deserialize the packet header
                packet.header.bodyLen = reader.ReadInt32();
                packet.header.sourceId = reader.ReadString();
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
        str += "Source: " + header.sourceId + "\n";
        str += "Message Type: " + header.messageType + "\n";
        str += "Picture Flag: " + header.pictureFlag + "\n";
        string bodyStr = Encoding.ASCII.GetString(body);
        str += "Body: " + bodyStr + "\n";
        return str;
    }
}

