using System;
using System.Text;
using System.Text.Json;
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

        public Post() { }

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
        /// Constructs a post from a byte array
        /// </summary>
        public Post(byte[] body)
        {
            // convert byte array to json then to field
            string json = Encoding.UTF8.GetString(body);
            Post post = JsonSerializer.Deserialize<Post>(json);

            this.id = post.id;
            this.content = post.content;
            this.author = post.author;
            this.date = post.date;
            this.imageName = post.imageName;
        }

        /// <summary>
        /// Constructor for the Post class
        /// imageName optional
        /// </summary>
        /// <param name="visitor"></param>
        /// <param name="content"></param>
        /// <param name="imageName"></param>
        public Post (Visitor visitor, string content, string imageName = "")
        {
            this.id = Guid.NewGuid().GetHashCode();
            this.content = content;
            this.author = visitor.username;
            this.date = DateTime.Now;
            this.imageName = imageName;
        }

        /// <summary>
        /// Serializes a post into a byte array
        /// </summary>
        public byte[] ToByte()
        {
            // convert field to json then to byte array
            string json = JsonSerializer.Serialize(this);
            byte[] bodyBytes = Encoding.UTF8.GetBytes(json);
            

            return bodyBytes;
        }


    }
}
