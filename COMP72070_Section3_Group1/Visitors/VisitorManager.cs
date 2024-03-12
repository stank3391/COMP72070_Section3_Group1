﻿namespace COMP72070_Section3_Group1.Visitors
{
    /// <summary>
    /// Manages the visitors 
    /// </summary>
    public class VisitorManager
    {
        public Dictionary<string, Visitor> Visitors { get; set; } // dictionary of visitors mapped to their ids

        public VisitorManager()
        {
            this.Visitors = new Dictionary<string, Visitor>();
            Console.WriteLine("VisitorManager started");
        }

        /// <summary>
        /// add a visitoe
        /// <summary/>
        public void AddVisitor(Visitor visitor)
        {
            this.Visitors.Add(visitor.id, visitor);
            Console.WriteLine($"Visitor added: {visitor.id}");
        }

        /// <summary>
        /// remove a visitor by visitor obj
        /// <summary/>
        public void RemoveVisitor(Visitor visitor)
        {
            this.Visitors.Remove(visitor.id);
            Console.WriteLine($"Visitor removed: {visitor.id}");
        }

        /// <summary>
        /// remove a visitor by id
        /// <summary/>
        public void RemoveVisitor(string id)
        {
            this.Visitors.Remove(id);
            Console.WriteLine($"Visitor removed: {id}");
        }
        
        /// <summary>
        /// get a visitor by id
        /// </summary>
        public Visitor GetVisitor(string id)
        {
            return this.Visitors[id];
        }


        
    }
}
