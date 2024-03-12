using System;
using System.Text;
namespace COMP72070_Section3_Group1.Models
{
    public class Post
    {
        int id { set; get; }
        string content { set; get; }
        string author { set; get; }
        DateTime date { set; get; }

        public Post()
        {
            // nothing
        }

        public Post(int id, string content, string author, DateTime date)
        {
            this.id = id;
            this.content = content;
            this.author = author;
            this.date = date;
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
        }

        /// <summary>
        /// Serializes a post into a byte array
        /// </summary>
        public byte[] ToByte()
        {
            // create data string
            string body = $"{id},{content},{author},{date.ToString()}";

            // convert data to byte array
            byte[] bodyBytes = Encoding.ASCII.GetBytes(body);

            return bodyBytes;
        }

        
    }
}
