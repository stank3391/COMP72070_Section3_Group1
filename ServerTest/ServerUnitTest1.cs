using System.Text;
using TcpServer;
using COMP72070_Section3_Group1.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace ServerTest
{
    [TestClass]
    public class ServerUnitTest1
    {
        [TestMethod]
        public void Test_HandleAuthPacket()
        {
            // Arrange
            TcpServer.TcpServer server = new TcpServer.TcpServer();
            string username = "username";
            string password = "password";
            server.accounts.Add(new Account(username, password));
            Packet packet = new Packet("TCP_CLIENT", Packet.Type.Auth, Encoding.ASCII.GetBytes($"{username},{password}"));

            // Act
            bool actual = server.VerifyLogin(username, password);

            // Assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void Test_VerifyLogin_True()
        {
            // Arrange
            TcpServer.TcpServer server = new TcpServer.TcpServer();
            server.PlaceholderLoadAccounts();
            string username = "dsa";
            string password = "psa";

            // Act
            bool result = server.VerifyLogin(username, password);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Test_VerifyLogin_False()
        {
            // Arrange
            TcpServer.TcpServer server = new TcpServer.TcpServer();
            server.PlaceholderLoadAccounts();
            string username = "test";
            string password = "aaaa";

            // Act
            bool result = server.VerifyLogin(username, password);

            // Assert
            Assert.IsFalse(result);
        }

            

        [TestMethod]
        public void Test_PlaceholderSavePosts()
        {
            // Arrange
            TcpServer.TcpServer server = new TcpServer.TcpServer();
            server.posts.Add(new Post(1, "testcontent", "testauthor", DateTime.Now, "testimageName"));

            // Act
            server.PlaceholderSavePosts();

            // Assert
            string json = File.ReadAllText(server.postsPath);
            Assert.IsTrue(json.Contains("testcontent"));
            Assert.IsTrue(json.Contains("testauthor"));
            Assert.IsTrue(json.Contains("testimageName"));
            
        }

        [TestMethod]
        public void Test_PlaceholderLoadPosts()
        {
            // Arrange
            TcpServer.TcpServer server = new TcpServer.TcpServer();
            string json = "[{\r\n    \"id\": 1,\r\n    \"content\": \"Hello World \\\\o/ I am Akrem\",\r\n    \"author\": \"Akrem\",\r\n    \"date\": \"2024-03-18T07:02:47\",\r\n    \"imageName\": \"\"\r\n  }]";
            File.WriteAllText(server.postsPath, json);

            // Act
            server.PlaceholderLoadPosts();

            // Assert
            Assert.AreEqual(1, server.posts.Count);
            Assert.AreEqual(1, server.posts[0].id);
            Assert.AreEqual("Hello World \\o/ I am Akrem", server.posts[0].content);
            Assert.AreEqual("Akrem", server.posts[0].author);
            Assert.AreEqual("", server.posts[0].imageName);
        }

        [TestMethod]
        public void Test_PlaceholderSaveAccounts()
        {
            // Arrange
            TcpServer.TcpServer server = new TcpServer.TcpServer();
            server.accounts.Add(new Account ("dsa", "psa"));

            // Act
            server.PlaceholderSaveAccounts();

            // Assert
            string json = File.ReadAllText(server.accountsPath);
            Assert.IsTrue(json.Contains("dsa"));
            Assert.IsTrue(json.Contains("psa"));
        }

        [TestMethod]
        public void Test_PlaceholderLoadAccounts()
        {
            // Arrange
            TcpServer.TcpServer server = new TcpServer.TcpServer();

            string json = "[{\"username\":\"testUser\",\"password\":\"testPassword\"}]";
            File.WriteAllText(server.accountsPath, json);

            // Act
            server.PlaceholderLoadAccounts();

            // Assert
            Assert.AreEqual(1, server.accounts.Count);
            Assert.AreEqual("testUser", server.accounts[0].username);
            Assert.AreEqual("testPassword", server.accounts[0].password);
        }   
    }
}