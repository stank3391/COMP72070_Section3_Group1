using COMP72070_Section3_Group1.Models;

namespace ModelsTest
{
    [TestClass]
    public class VisitorTest
    {
        [TestMethod]
        public void Test_Set_id()
        {
            // Arrange
            Visitor visitor = new Visitor("123abc");
            string expected = "222ccc";

            // Act
            visitor.id = "222ccc";

            // Assert
            Assert.AreEqual(expected, visitor.id);
        }

        [TestMethod]
        public void Test_Set_isAuthenticated_True()
        {
            // Arrange
            Visitor visitor = new Visitor("123abc");
            bool expected = true;

            // Act
            visitor.isAuthenicated = true;

            // Assert
            Assert.AreEqual(expected, visitor.isAuthenicated);
        }

        [TestMethod]
        public void Test_Set_isAuthenticated_False()
        {
            // Arrange
            Visitor visitor = new Visitor("123abc");
            bool expected = false;

            // Act
            visitor.isAuthenicated = false;

            // Assert
            Assert.AreEqual(expected, visitor.isAuthenicated);
        }

        [TestMethod]
        public void Test_Set_username()
        {
            // Arrange
            Visitor visitor = new Visitor("123abc");
            string expected = "testUser";

            // Act
            visitor.username = "testUser";

            // Assert
            Assert.AreEqual(expected, visitor.username);
        }

        [TestMethod]
        public void Test_Constructor()
        {
            // Arrange
            string expected = "123abc";

            // Act
            Visitor visitor = new Visitor("123abc");

            // Assert
            Assert.AreEqual(expected, visitor.id);
        }

        [TestMethod]
        public void Test_Get_id()
        {
            // Arrange
            Visitor visitor = new Visitor("123abc");
            string expected = "123abc";

            // Act
            string actual = visitor.id;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Get_isAuthenticated_False()
        {
            // Arrange
            Visitor visitor = new Visitor("123abc");
            bool expected = false;

            // Act
            bool actual = visitor.isAuthenicated;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Get_isAuthenticated_True()
        {
            // Arrange
            Visitor visitor = new Visitor("123abc");
            visitor.isAuthenicated = true;
            bool expected = true;

            // Act
            bool actual = visitor.isAuthenicated;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_Get_username()
        {
            // Arrange
            Visitor visitor = new Visitor("123abc");
            string expected = "";

            // Act
            string actual = visitor.username;

            // Assert
            Assert.AreEqual(expected, actual);
        }
       
    }
}