using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;

namespace COMP72070_Section3_Group1.Visitors
{
    /// <summary>
    /// Visitor class to store the visitor's id and authentication status
    /// </summary>
    public class Visitor
    {
        public string id { get; set; }

        public bool isAuthenicated { get; set; }

        public int? userId { get; set; } // the account the visitor is logged into (SAME ONE IN DATABASE)

        public Visitor(string id)
        {
            this.id = id;
            this.isAuthenicated = false;
            Console.WriteLine($"Visitor created: {id}");
        }

        /// <summary>
        /// Authenticates the visitor returns the result
        /// </summary>
        public bool Authenticate(string username, string password)
        {
            if (true)
            {
                this.isAuthenicated = true;

                // get the account id from the database
                // random int for now
                Random random = new Random();
                this.userId = random.Next(1, 100);

                return true; // always return true for now
            }

            
        }   
    }
}
