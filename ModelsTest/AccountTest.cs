using COMP72070_Section3_Group1.Models;
using DocumentFormat.OpenXml.Presentation;

namespace ModelsTest
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            // Arrange
            Account account = new Account("test", "password");
            string expectedUsername = "test";
            string expectedPassword = "password";

            // Act
            string actualUsername = account.username;
            string actualPassword = account.password;

            // Assert
            Assert.AreEqual(expectedUsername, actualUsername);
            Assert.AreEqual(expectedPassword, actualPassword);
        }

        [TestMethod]
        public void Test_SetUsername()
        {
            // Arrange
            Account account = new Account("test", "password");
            string expected = "newUsername";

            // Act
            account.username = "newUsername";
            string actual = account.username;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_SetPassword()
        {
            // Arrange
            Account account = new Account("test", "password");
            string expected = "newPassword";

            // Act
            account.password = "newPassword";
            string actual = account.password;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_SetUsernameAndPassword()
        {
            // Arrange
            Account account = new Account("test", "password");
            string expectedUsername = "newUsername";
            string expectedPassword = "newPassword";

            // Act
            account.username = "newUsername";
            account.password = "newPassword";
            string actualUsername = account.username;
            string actualPassword = account.password;

            // Assert
            Assert.AreEqual(expectedUsername, actualUsername);
            Assert.AreEqual(expectedPassword, actualPassword);
        }

        [TestMethod]
        public void Test_GetUsername()
        {
            // Arrange
            Account account = new Account("test", "password");
            string expected = "test";

            // Act
            string actual = account.username;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_GetPassword()
        {
            // Arrange
            Account account = new Account("test", "password");
            string expected = "password";

            // Act
            string actual = account.password;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_GetUsernameAndPassword()
        {
            // Arrange
            Account account = new Account("test", "password");
            string expectedUsername = "test";
            string expectedPassword = "password";

            // Act
            string actualUsername = account.username;
            string actualPassword = account.password;

            // Assert
            Assert.AreEqual(expectedUsername, actualUsername);
            Assert.AreEqual(expectedPassword, actualPassword);
        }
    }
}