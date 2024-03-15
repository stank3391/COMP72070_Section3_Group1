using System;
using System.Text;
namespace COMP72070_Section3_Group1.Models
{
    /// <summary>
    /// Represents a post
    /// </summary>
    public class Post
    {
        public int id { set; get; } // id of the post (in the database)
        public string content { set; get; } 
        public string author { set; get; }
        public DateTime date { set; get; }
        public string imageName { set; get; } // name of the image file in ./wwwroot/images

        public Post(){}

        /// <summary>
        /// Constructor for the Post class
        /// imageName optional
        /// </summary>
        public Post(int id, string content, string author, DateTime date, string imageName = "")
        {
            this.id = id;
            this.content = content;
            this.author = author;
            this.date = date;
            this.imageName = imageName;
        }

        /// <summary>
        /// Deserializes a byte array into a post
        /// </summary>
        public Post(byte[] body)
        {
            // convert byte array to string
            string data = Encoding.ASCII.GetString(body);

            // split the string into fields
            string[] fields = data.Split(',');

            // set the fields
            id = int.Parse(fields[0]);
            content = fields[1];
            author = fields[2];
            date = DateTime.Parse(fields[3]);
            imageName = fields[4];
        }

        /// <summary>
        /// Serializes a post into a byte array
        /// </summary>
        public byte[] ToByte()
        {
            // create data string
            string body = $"{id},{content},{author},{date.ToString()},{imageName}";

            // convert data to byte array
            byte[] bodyBytes = Encoding.ASCII.GetBytes(body);

            return bodyBytes;
        }

        
    }
}
