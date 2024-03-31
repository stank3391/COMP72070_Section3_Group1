using COMP72070_Section3_Group1.Models;

namespace TcpServer
{
    /// <summary>
    /// Partial class for the TcpServer
    /// Hnadles the database operations
    /// </summary>
    public partial class TcpServer
    {
        // database props
        public string postsPath = "../../../../../COMP72070_Section3_Group1/TcpServer/placeholder_db/posts.json"; // path to the posts database
        public string accountsPath = "../../../../../COMP72070_Section3_Group1/TcpServer/placeholder_db/accounts.json"; // path to the accounts database
        public List<Post> posts = new List<Post>(); // list of posts from the database
        public List<Account> accounts = new List<Account>(); // list of accounts from the database

        /// <summary>
        /// save all posts to palceholder database
        /// </summary>
        public void PlaceholderSavePosts()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(posts);

            File.WriteAllText(postsPath, json);

            Console.WriteLine("Posts list saved to placeholder database.");
        }

        /// <summary>
        /// loads all posts from placeholder database to posts list
        /// </summary>
        public void PlaceholderLoadPosts()
        {
            string json = File.ReadAllText(postsPath);

            posts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Post>>(json);

            Console.WriteLine("Posts list loaded from placeholder database.");
        }

        /// <summary>
        /// save all accounts to palceholder database
        /// </summary>
        public void PlaceholderSaveAccounts()
        {
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(accounts);

            File.WriteAllText(accountsPath, json);

            Console.WriteLine("Accounts list saved to placeholder database.");
        }

        /// <summary>
        /// loads all accounts from placeholder database to accounts list
        /// </summary>
        public void PlaceholderLoadAccounts()
        {
            string json = File.ReadAllText(accountsPath);

            accounts = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Account>>(json);

            Console.WriteLine("Accounts list loaded from placeholder database.");
        }

    }
}