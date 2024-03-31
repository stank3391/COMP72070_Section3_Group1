using System.Text;
using System.Text.Json;
using COMP72070_Section3_Group1.Models;

namespace ModelsTest
{
    [TestClass]
    public class PostTest
    {
        [TestMethod]
        public void Test_Constructor_Vars()
        {
            // Arrange
            Post post = new Post(1, "content", "author", DateTime.Now, "imageName");
            int expectedId = 1;
            string expectedContent = "content";
            string expectedAuthor = "author";
            DateTime expectedDate = DateTime.Now;
            string expectedImageName = "imageName";

            // Act
            int actualId = post.id;
            string actualContent = post.content;
            string actualAuthor = post.author;
            DateTime actualDate = post.date;
            string actualImageName = post.imageName;

            // Assert
            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedContent, actualContent);
            Assert.AreEqual(expectedAuthor, actualAuthor);
            Assert.AreEqual(expectedDate.ToString(), actualDate.ToString());
            Assert.AreEqual(expectedImageName, actualImageName);
        }

        [TestMethod]
        public void Test_Constructor_Bytes()
        {
            // Arrange
            DateTime date = DateTime.Now;
            Post post = new Post(1, "content", "author", date, "imageName");
            byte[] body = post.ToByte();
            Post actualPost = new Post(body);
            int expectedId = 1;
            string expectedContent = "content";
            string expectedAuthor = "author";
            DateTime expectedDate = date;
            string expectedImageName = "imageName";

            // Act
            int actualId = actualPost.id;
            string actualContent = actualPost.content;
            string actualAuthor = actualPost.author;
            DateTime actualDate = actualPost.date;
            string actualImageName = actualPost.imageName;

            // Assert
            Assert.AreEqual(expectedId, actualId);
            Assert.AreEqual(expectedContent, actualContent);
            Assert.AreEqual(expectedAuthor, actualAuthor);
            Assert.AreEqual(expectedDate, actualDate);
            Assert.AreEqual(expectedImageName, actualImageName);
        }
        

        [TestMethod]
        public void Test_Get_id()
        {
            // Arrange
            Post post = new Post(1, "content", "author", DateTime.Now, "imageName");
            int expected = 1;

            // Act
            int actual = post.id;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Get_content()
        {
            // Arrange
            Post post = new Post(1, "content", "author", DateTime.Now, "imageName");
            string expected = "content";

            // Act
            string actual = post.content;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Get_author()
        {
            // Arrange
            Post post = new Post(1, "content", "author", DateTime.Now, "imageName");
            string expected = "author";

            // Act
            string actual = post.author;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Get_date()
        {
            // Arrange
            Post post = new Post(1, "content", "author", DateTime.Now, "imageName");
            DateTime expected = DateTime.Now;

            // Act
            DateTime actual = post.date;

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void Test_Get_imageName()
        {
            // Arrange
            Post post = new Post(1, "content", "author", DateTime.Now, "imageName");
            string expected = "imageName";

            // Act
            string actual = post.imageName;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Set_id()
        {
            // Arrange
            Post post = new Post(1, "content", "author", DateTime.Now, "imageName");
            int expected = 2;

            // Act
            post.id = 2;
            int actual = post.id;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Set_content()
        {
            // Arrange
            Post post = new Post(1, "content", "author", DateTime.Now, "imageName");
            string expected = "newContent";

            // Act
            post.content = "newContent";
            string actual = post.content;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Set_author()
        {
            // Arrange
            Post post = new Post(1, "content", "author", DateTime.Now, "imageName");
            string expected = "newAuthor";

            // Act
            post.author = "newAuthor";
            string actual = post.author;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Set_date()
        {
            // Arrange
            Post post = new Post(1, "content", "author", DateTime.Now, "imageName");
            DateTime expected = DateTime.Now.AddDays(1);

            // Act
            post.date = DateTime.Now.AddDays(1);
            DateTime actual = post.date;

            // Assert
            Assert.AreEqual(expected.ToString(), actual.ToString());
        }

        [TestMethod]
        public void Test_Set_imageName()
        {
            // Arrange
            Post post = new Post(1, "content", "author", DateTime.Now, "imageName");
            string expected = "newImageName";

            // Act
            post.imageName = "newImageName";
            string actual = post.imageName;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_ToByte()
        {
            // Arrange
            DateTime date = DateTime.Now;
            Post post1 = new Post(1, "content", "author", date, "imageName");
            Post post2 = post1;
            string expectedString = JsonSerializer.Serialize(post2);
            byte[] expected = Encoding.ASCII.GetBytes(expectedString);

            // Act
            byte[] actual = post1.ToByte();

            // Assert
            CollectionAssert.AreEqual(expected, actual);
        }
    }
}