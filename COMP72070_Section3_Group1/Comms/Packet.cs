using System.Text;
using System.IO;

public class Packet
{
    // definitions: 
    public enum Type // signifies the type of message
    { 
        Post,   // post packet
        DM,     // direct message packet
        Auth,   // authentication packet
        Acc,    // ???
        Ack,    // acknowledgement packet
        Test,   // test packet
        Error,  // error packet
        ReadyPost,   // ready packet - ready to receive posts
        ReadyImage, // ready packet - ready to receive images
        ReadyDM, // ready packet - ready to receive direct messages
        Image, // image packet
    };

    public struct Header
    {
        public int bodyLen { get; set; }
        public string sourceId { get; set; } // id of the visitor
        public Type packetType { get; set; }
        public int packetNumber { get; set; }
        public int totalPackets { get; set; }
    } 

    // properties
    public Header header;

    private byte[] _body;

    public byte[] body
    {
        get
        {
            if(_body != null)
                return _body;
            else
                return new byte[0];
        }

        set
        {
            _body = value;
            if (_body != null)
                header.bodyLen = _body.Length;
            else
                header.bodyLen = 0;
        }
    }

    /// <summary>
    /// default constructor
    /// </summary>
    public Packet(){}

    /// <summary>
    /// constructs a packet with byte[] as body
    /// </summary>
    public Packet(string sourceId, Type messageType, byte[]? body = null,  int PacketNumber = 0, int TotalPackets = 1)
    {
        this.header.sourceId = sourceId;
        this.header.packetType = messageType;
        this.header.packetNumber = PacketNumber;
        this.header.totalPackets = TotalPackets;
        this.body = body;
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
                // ToByte the packet header
                writer.Write(packet.header.bodyLen);
                writer.Write(packet.header.sourceId);
                writer.Write((int)packet.header.packetType);
                writer.Write(packet.header.packetNumber);
                writer.Write(packet.header.totalPackets);

                // ToByte the packet body
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
                packet.header.packetType = (Type)reader.ReadInt32();
                packet.header.packetNumber = reader.ReadInt32();
                packet.header.totalPackets = reader.ReadInt32();

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
        str += "Message Type: " + header.packetType + "\n";
        str += "Packet Number: " + header.packetNumber + "\n";
        str += "Total Packets: " + header.totalPackets + "\n";
        string bodyStr = Encoding.ASCII.GetString(body);
        str += "Body: " + bodyStr;
        return str;
    }

    /// <summary>
    /// split image packets into multiple packets
    /// </summary>
    public static Packet[] CreateImagePacket(string sourceId, byte[] imageData, int maxSize = 1024)
    {
        Console.WriteLine("Packet.CreateImagePacket(): Start");
        // calc num ofpackets needed
        int totalPackets = (int)Math.Ceiling((double)imageData.Length / maxSize);

        // create packets
        Packet[] packets = new Packet[totalPackets];
        for (int i = 0; i < totalPackets; i++)
        {
            Console.WriteLine($"Packet.CreateImagePacket(): Creating packet {i + 1} of {totalPackets}");
            int offset = i * maxSize;
            int size = Math.Min(maxSize, imageData.Length - offset);
            byte[] body = new byte[size];
            Array.Copy(imageData, offset, body, 0, size); // DEEP COPY!!!!
            packets[i] = new Packet(sourceId, Type.Image, body, i, totalPackets);
        }
        Console.WriteLine("Packet.CreateImagePacket(): End");
        return packets;
    }

    /// <summary>
    /// reconstruct image from packets
    /// </summary>
    public static byte[] ReconstructImage(Packet[] packets)
    {
        Console.WriteLine("Packet.ReconstructImage(): Start");

        if (packets.Length == 0)
        {
            Console.WriteLine("Packet.ReconstructImage(): No packets to reconstruct");
        }
        // calc total size of image
        int totalSize = 0;
        foreach (Packet packet in packets)
        {
            totalSize += packet.body.Length;
        }

        byte[] imageData = new byte[totalSize];

        // copy data from packets to imageData
        int offset = 0;
        foreach (Packet packet in packets)
        {
            Console.WriteLine($"Packet.ReconstructImage(): Reconstructing {packet.header.packetNumber + 1} of {packet.header.totalPackets}");
            Array.Copy(packet.body, 0, imageData, offset, packet.body.Length);
            offset += packet.body.Length;
        }
        Console.WriteLine("Packet.ReconstructImage(): End");
        return imageData;
    }
}

