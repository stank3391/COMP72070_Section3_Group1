using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AstroServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            dbHandler handler = new dbHandler();

            Console.WriteLine(handler.CreateUser("nic", "password", "email@mail.com",
                "bdqkjwd2@!DQWDasd", DateTime.Now, "user"));

            Console.WriteLine(handler.getUser(1).ToString());
            Console.WriteLine(handler.getUser(2).ToString());

            // Added so that the program doesn't automatically quit
            Console.WriteLine("press any key to exit");
            Console.ReadKey();
        }
    }
}
