namespace COMP72070_Section3_Group1.Models
{
    /// <summary>
    /// Manages the visitors 
    /// </summary>
    public class VisitorManager
    {
        private int visitorCount; // count of visitors for testing
        public Dictionary<string, Visitor> visitors { get; set; } // dictionary of visitors mapped to their ids

        public VisitorManager()
        {
            visitors = new Dictionary<string, Visitor>();
            Console.WriteLine("VisitorManager started");
            visitorCount = 0;
        }

        /// <summary>
        /// add a visitoe
        /// <summary/>
        public void AddVisitor(Visitor visitor)
        {
            visitors.Add(visitor.id, visitor);
            Console.WriteLine($"Visitor added: {visitor.id} ({visitorCount})");
            visitorCount++;
        }

        /// <summary>
        /// remove a visitor by visitor obj
        /// <summary/>
        public void RemoveVisitor(Visitor visitor)
        {
            visitors.Remove(visitor.id);
            Console.WriteLine($"Visitor removed: {visitor.id}");
        }

        /// <summary>
        /// remove a visitor by id
        /// <summary/>
        public void RemoveVisitor(string id)
        {
            visitors.Remove(id);
            Console.WriteLine($"Visitor removed: {id}");
        }

        /// <summary>
        /// get a visitor by id
        /// </summary>
        public Visitor GetVisitor(string id)
        {
            return visitors[id];
        }

        /// <summary>
        /// update a visitor
        /// </summary>
        public void UpdateVisitor(Visitor visitor)
        {
            visitors[visitor.id] = visitor;
        }


    }
}
