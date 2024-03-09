using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace AstroServer
{
    internal class TcpServer
    {
        public static void Main()
        {
            //example of how to use database
            // Here is the initialization
            using(Entity db = new Entity())
            {
                // To add a user first create a tbl_Users object
                tbl_Users newUser = new tbl_Users();
                // Then fill in its parameters
                newUser.UserId = 10;
                newUser.Username = "John";
                // Then add it to the database
                db.tbl_Users.Add(newUser);
                // Lastly, save the database
                db.SaveChanges();

                // using this you can iterate through every item
                // in a table.
                foreach (var user in db.tbl_Users)
                    Console.WriteLine(user.Username);
            }

            // Added so that the program doesn't automatically quit
            Console.WriteLine("press any key to exit");
            Console.ReadKey();

            //TcpListener server;
            //int port = 27000;
            //IPAddress localAddr = IPAddress.Any;
            //// IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            //server = new TcpListener(localAddr, port);

            //server.Start();

            //byte[] bytes = new byte[256];
            //String data;

            //while (true)
            //{
            //    Console.WriteLine("waiting... ");
            //    TcpClient client = server.AcceptTcpClient();
            //    Console.WriteLine("connected!");

            //    data = "";

            //    NetworkStream stream = client.GetStream();

            //    int i;
            //    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
            //    {
            //        data = Encoding.ASCII.GetString(bytes, 0, i);
            //        Console.WriteLine("received: " + data);

            //        data = "i have it";
            //        byte[] msg = Encoding.ASCII.GetBytes(data);

            //        stream.Write(msg, 0, msg.Length);
            //        Console.WriteLine("sent: " + data);
            //    }

            //    client.Close();
            //}
        }
    }
}
