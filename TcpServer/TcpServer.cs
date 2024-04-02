using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace TcpServer
{
    /// <summary>
    /// Main partial class for the tcpserver
    /// handles the networking and general packet handling
    /// </summary>
    public partial class TcpServer
    {
        // networking props
        private TcpListener listener;
        private int port = 27000;
        private IPAddress localAddress = IPAddress.Any;
        private TcpClient tcpClient;
        private NetworkStream stream;

        private bool isStop = false; // flag to stop the server

        

        /// <summary>
        /// Constructor for the TcpServer class
        /// </summary>
        public TcpServer()
        {
            this.listener = new TcpListener(this.localAddress, this.port);
        }

        /// <summary>
        /// Listens and connects to clients. 
        /// load the posts from the database
        /// </summary>
        public void Start()
        {
            Console.WriteLine("TcpServer.Start(): Start");
            while (!this.isStop)
            {
                Console.WriteLine("TcpServer.Start(): Listenting");
                this.listener.Start();

                this.tcpClient = this.listener.AcceptTcpClient();
                Console.WriteLine("TcpServer.Start(): Connected");

                this.stream = this.tcpClient.GetStream();

                HandleClient();
            }
            Console.WriteLine("TcpServer.Start(): End");
        }

        /// <summary>
        /// MAIN FUNCTION TO COMMUNICATE WITH CLIENT!
        /// </summary>
        private void HandleClient()
        {
            Console.WriteLine("TcpServer.HandleClient(): Start");

            bool isDisconnect = false;

            while (!isDisconnect)
            {
                Packet packetIn = this.ReceivePacket();
                HandlePacket(packetIn);

                if (!tcpClient.Connected)
                {
                    Console.WriteLine($"TcpServer.HandleClient(): Disconnected");
                    tcpClient.Close();
                    isDisconnect = true;
                }
            }
            Console.WriteLine("TcpServer.HandleClient(): End");
        }

        /// <summary>
        /// Handles the packet received from the client
        /// determines the type of packet and do action
        /// </summary>
        public void HandlePacket(Packet packet)
        {
            Console.WriteLine("TcpServer.HandlePacket(): Start");
            Log.CreateLog(Log.ServerLogName, packet.header.sourceId, "Packet received. Type: " + packet.header.packetType);
            Packet.Type type = packet.header.packetType;

            switch (type)
            {
                case Packet.Type.Ack:
                    Console.WriteLine("TcpServer.HandlePacket(): Ack received");
                    break;
                case Packet.Type.Error:
                    Console.WriteLine("TcpServer.HandlePacket(): Error received");
                    break;
                case Packet.Type.ReadyPost:
                    Console.WriteLine("TcpServer.HandlePacket(): ReadyPost received");
                    HandleReadyPostPacket();
                    break;
                case Packet.Type.ReadyImage:
                    Console.WriteLine("TcpServer.HandlePacket(): ReadyImage received");
                    HandleReadyImagePacket();
                    break;
                case Packet.Type.Post:
                    Console.WriteLine("TcpServer.HandlePacket(): Post received");
                    HandlePostPacket(packet);
                    break;
                case Packet.Type.Image:
                    Console.WriteLine("TcpServer.HandlePacket(): Image received");
                    HandleImagePacket(packet);
                    break;
                case Packet.Type.Auth:
                    Console.WriteLine("TcpServer.HandlePacket(): Auth received");
                    HandleAuthPacket(packet);
                    break;
                case Packet.Type.Acc:
                    Console.WriteLine("TcpServer.HandlePacket(): Acc received");
                    HandleAccPacket(packet);
                    break;
                case Packet.Type.UpdateAcc:
                    Console.WriteLine("TcpServer.HandlePacket(): UpdateAccImage received");
                    HandleUpdateAccPacket(packet);
                    break;
                default:
                    Console.WriteLine("TcpServer.HandlePacket(): Unknown packet type received");
                    break;
            }

            Console.WriteLine("TcpServer.HandlePacket(): End");
        }

        /// <summary>
        /// Waits for an ack packet from the client
        /// so that we can send the next packet
        /// </summary>
        public void WaitForAck()
        {
            Packet ackPacket = this.ReceivePacket();
            if (ackPacket.header.packetType != Packet.Type.Ack)
            {
                Console.WriteLine("TcpServer.WaitForAck(): Error receiving ack packet");
            }
        }

        /// <summary>
        /// Sends an ack packet to the client so client can send the next packet
        /// </summary>
        public void SendAck()
        {
            Packet ackPacket = new Packet("TCP_SERVER", Packet.Type.Ack);
            this.SendPacket(ackPacket); 
        }
        
        /// <summary>
        /// Sends a packet to the client
        /// </summary>
        public void SendPacket(Packet packet)
        {
            byte[] serializedPacket = Packet.SerializePacket(packet);
            stream.Write(serializedPacket, 0, serializedPacket.Length);
            Console.WriteLine($"TcpServer.SendPacket(): Packet sent. Type: {packet.header.packetType}. Count {packet.header.packetNumber + 1} of {packet.header.totalPackets}");     
        }

        /// <summary>
        /// Receives a packet from the client
        /// </summary>
        public Packet ReceivePacket()
        {
            byte[] bufferIn = new byte[Packet.MAX_PACKET_SIZE];
            this.stream.Read(bufferIn, 0, bufferIn.Length);
            Packet packet = Packet.DeserializePacket(bufferIn);
            return packet;
        }
    }
}