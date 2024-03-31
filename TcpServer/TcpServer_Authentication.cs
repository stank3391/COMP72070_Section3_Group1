using System.Text;
using System.Text.RegularExpressions;
using COMP72070_Section3_Group1.Models;
namespace TcpServer
{
    /// <summary>
    /// Partial class for the TcpServer
    /// Contains the methods for handling account creation and authentication packets
    /// </summary>
    public partial class TcpServer
    {
        /// <summary>
        /// Handles the account creation packet
        /// </summary>
        public void HandleAccPacket(Packet packet)
        {
            string bodystring = packet.ToString();
            Console.WriteLine(bodystring);

            // Parse the input string to extract username and password
            var userData = ParseLoginInputString(bodystring);

            // Check if the username already exists in the database
            if (accounts.Any(account => account.username == userData.username))
            {
                Console.WriteLine($"Account creation failed: Username already exists{userData.username}");

                // Send acc fail packet
                Packet response = new Packet("TCP_SERVER", Packet.Type.AccFail);
                byte[] serializedPacket = Packet.SerializePacket(response);
                stream.Write(serializedPacket, 0, serializedPacket.Length);
            }
            else
            {
                // Add the new account to the list
                accounts.Add(new Account(userData.username, userData.password));

                // Save the updated list to the database
                PlaceholderSaveAccounts();

                // Send acc success packet
                Packet response = new Packet("TCP_SERVER", Packet.Type.AccSuccess);
                byte[] serializedPacket = Packet.SerializePacket(response);
                stream.Write(serializedPacket, 0, serializedPacket.Length);

                Console.WriteLine($"Account created successfully: {userData.username}");
            }
        }

        public void HandleUpdateAccPacket(Packet packet)
        {
            // convert the packet body to a string
            string bodyString = Encoding.ASCII.GetString(packet.body);

            // parse the input string to extract username, field, and value
            // body is in the format "username,field,value"
            string[] bodyParts = bodyString.Split(',');
            string username = bodyParts[0];
            string field = bodyParts[1];
            string value = bodyParts[2];

            // find the account in the list
            Account account = accounts.Find(account => account.username == username);
            if (account == null)
            {
                Console.WriteLine($"Account not found: {username}");
                return;
            }

            // update the account field
            switch (field)
            {
                case "password":
                    account.password = value;
                    break;
                case "imageName":
                    account.imageName = value;
                    break;
                default:
                    Console.WriteLine($"Invalid field: {field}");
                    return;
            }

            // update the account in the list
            accounts[accounts.FindIndex(account => account.username == username)] = account;

            // save the updated list to the database
            PlaceholderSaveAccounts();
            
        }

        /// <summary>
        /// Verifies the login credentials against the database
        /// </summary>
        public bool VerifyLogin(string username, string password)
        {
            // Check if the username and password match any account in the database
            return accounts.Any(account => account.username == username && account.password == password);
        }

        /// <summary>
        /// Handles the authentication packet
        /// Sends an auth success packet if the login is successful
        /// Sends an auth fail packet if the login is unsuccessful
        /// The body of the auth success packet is the profile image name
        /// </summary>
        /// <param name="packet"></param>
        public void HandleAuthPacket(Packet packet)
        {
            //packet.Deserialize
            string bodystring = packet.ToString();
            Console.WriteLine(bodystring);

            // Parse the input string to extract username and password
            var userData = ParseLoginInputString(bodystring);

            if (VerifyLogin(userData.username, userData.password))
            {
                Console.WriteLine("LoginAction successful!");

                // get profile image name
                string imageName = accounts.Find(account => account.username == userData.username).imageName;

                // send auth success packet with the image name as the body
                Packet response = new Packet("TCP_SERVER", Packet.Type.AuthSuccess, Encoding.ASCII.GetBytes(imageName));
                byte[] serializedPacket = Packet.SerializePacket(response);
                stream.Write(serializedPacket, 0, serializedPacket.Length);
            }
            else
            {
                Console.WriteLine("Invalid username or password.");

                // send auth fail packet
                Packet reponse = new Packet("TCP_SERVER", Packet.Type.AuthFail);
                byte[] serializedPacket = Packet.SerializePacket(reponse);
                stream.Write(serializedPacket, 0, serializedPacket.Length);
            }
        }

        /// <summary>
        /// Parses the input string to extract username and password
        /// </summary>
        public static (string username, string password) ParseLoginInputString(string input)
        {
            // Define a regular expression pattern to match "Username: <username>, Password: <password>"
            string pattern = @"Username:\s*(?<username>\w+),\s*Password:\s*(?<password>\w+)";

            // remove whitespace from the input string
            input = input.Replace(" ", "");

            // Match the input string against the pattern
            Match match = Regex.Match(input, pattern);

            if (match.Success)
            {
                // Extract username and password from the named groups in the match
                string username = match.Groups["username"].Value;
                string password = match.Groups["password"].Value;

                return (username, password);
            }
            else
            {
                // Handle invalid input format
                throw new ArgumentException("Invalid input format");
            }
        }
    }
}