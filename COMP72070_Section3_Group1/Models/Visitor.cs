using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;

namespace COMP72070_Section3_Group1.Models
{
    /// <summary>
    /// Visitor class to store the visitor's id and authentication status
    /// </summary>
    public class Visitor
    {
        public string id { get; set; } // used to identify the visitor/session

        public bool isAuthenicated { get; set; } = false; // placeholder until authentication is implemented

        public string? username { get; set; } = ""; // placeholder until authentication is implemented

        public string? imageName { get; set; } = ""; // image name of the profile picture

        /// <summary>
        /// Creates a new visitor with the given id
        /// </summary>
        /// <param name="id"></param>
        public Visitor(string id)
        {
            this.id = id;
            Console.WriteLine($"Visitor created: {id}");
        }
    }
}
