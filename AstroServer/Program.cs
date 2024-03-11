using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AstroServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //example of how to use database
            // Here is the initialization
            using (Entity db = new Entity())
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
        }
    }
}
