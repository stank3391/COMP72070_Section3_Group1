namespace COMP72070_Section3_Group1.Models
{
    public class Account
    {
        public string username { get; set; }
        public string password { get; set; }

        public string imageName { get; set; }

        public Account(){}

        public Account(string username, string password)
        {
            this.username = username;
            this.password = password;
            this.imageName = "";
        }

        public Account(string username, string password, string imageName)
        {
            this.username = username;
            this.password = password;
            this.imageName = imageName;
        }
    }
}
