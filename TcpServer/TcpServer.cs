using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TcpServer
{
    public class TcpServer
    {
        private TcpListener Listener;
        private int Port = 27000;
        private IPAddress LocalAddress = IPAddress.Any;

        private bool IsStop = false; // flag to stop the server

        /// <summary>
        /// Constructor for the TcpServer class
        /// </summary>
        public TcpServer()
        {
            this.Listener = new TcpListener(this.LocalAddress, this.Port);
        }

        /// <summary>
        /// Listens and connects to clients. 
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Server started.");
            while (!this.IsStop)
            {
                Console.WriteLine("Listenting.\n");
                this.Listener.Start();

                TcpClient tcpClient = this.Listener.AcceptTcpClient();
                Console.WriteLine("Connected\n");

                HandleClient(tcpClient);
            }
        }

        /// <summary>
        /// MAIN FUNCTION TO COMMUNICATE WITH CLIENT!
        /// </summary>
        private void HandleClient(TcpClient tcpClient)
        {

            byte[] bufferIn = new byte[1024]; // buffer for incoming data
            byte[] bufferOut = new byte[1024]; // buffer for outgoing data
            string data = string.Empty; // parsed data from buffer
            NetworkStream stream = tcpClient.GetStream(); // stream for reading and writing data

            bool isDisconnect = false;

            while (!isDisconnect)
            {
                // receive data from client
                stream.Read(bufferIn, 0, bufferIn.Length);
                Packet packetIn = Packet.DeserializePacket(bufferIn);
                Console.WriteLine($"Packet received:\n{packetIn.ToString()}\n");


                // send packet to client
                Packet packetOut = new Packet(Packet.Type.Ack, false, Encoding.ASCII.GetBytes("ACK"));
                byte[] serializedPacket = Packet.SerializePacket(packetOut);
                stream.Write(serializedPacket, 0, serializedPacket.Length);
                Console.WriteLine($"Packet sent:\n{packetOut.ToString()}\n");

                if (!tcpClient.Connected)
                {
                    Console.WriteLine($"Disconnected\n");
                    tcpClient.Close();
                }

            }

        }

    }

}


