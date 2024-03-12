namespace COMP72070_Section3_Group1.Users
{
    /// <summary>
    /// Manages the visitors 
    /// </summary>
    public class UserManager
    {
        public Dictionary<string, User> users { get; set; } // dictionary of visitors mapped to their ids

        public UserManager()
        {
            this.users = new Dictionary<string, User>();
            Console.WriteLine("VisitorManager started");
        }

        /// <summary>
        /// add a visitoe
        /// <summary/>
        public void AddUser(User visitor)
        {
            this.users.Add(visitor.id, visitor);
            Console.WriteLine($"Visitor added: {visitor.id}");
        }

        /// <summary>
        /// remove a visitor by visitor obj
        /// <summary/>
        public void RemoveVisitor(User visitor)
        {
            this.users.Remove(visitor.id);
            Console.WriteLine($"Visitor removed: {visitor.id}");
        }

        /// <summary>
        /// remove a visitor by id
        /// <summary/>
        public void RemoveVisitor(string id)
        {
            this.users.Remove(id);
            Console.WriteLine($"Visitor removed: {id}");
        }
        
        /// <summary>
        /// get a visitor by id
        /// </summary>
        public User GetVisitor(string id)
        {
            return this.users[id];
        }


        
    }
}
