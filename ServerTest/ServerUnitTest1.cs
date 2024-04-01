using System.Text;
using TcpServer;
using COMP72070_Section3_Group1.Models;
using Microsoft.EntityFrameworkCore.Query;
using System.Runtime.InteropServices;

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
    }
}