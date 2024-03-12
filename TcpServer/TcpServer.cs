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
        private TcpListener listener;
        private int port = 27000;
        private IPAddress localAddress = IPAddress.Any;

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
        /// </summary>
        public void Start()
        {
            Console.WriteLine("Server started.");
            while (!this.isStop)
            {
                Console.WriteLine("Listenting.\n");
                this.listener.Start();

                TcpClient tcpClient = this.listener.AcceptTcpClient();
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
                // receive data 
                stream.Read(bufferIn, 0, bufferIn.Length);
                Packet packetIn = Packet.DeserializePacket(bufferIn);
                Console.WriteLine($"Packet received:\n{packetIn.ToString()}\n");

                // send packet 
                Packet packetOut = new Packet("SERVER", Packet.Type.Ack, false, Encoding.ASCII.GetBytes("I got it!!"));
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


