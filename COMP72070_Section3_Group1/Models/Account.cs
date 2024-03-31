namespace COMP72070_Section3_Group1.Models
{
    public class Account
    {
        public string username { get; set; }
        public string password { get; set; }

        public Account(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
