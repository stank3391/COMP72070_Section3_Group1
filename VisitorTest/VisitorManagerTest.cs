using COMP72070_Section3_Group1.Models;

namespace VisitorManagerTest
{
    [TestClass]
    public class VisitorManagerTest
    {
        [TestMethod]
        public void Test_Constructor()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            int expected = 0;

            // Act
            int actual = visitorManager.visitors.Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_AddVisitor()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor = new Visitor("123abc");
            int expected = 1;

            // Act
            visitorManager.AddVisitor(visitor);
            int actual = visitorManager.visitors.Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_RemoveVisitor_Visitor()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor = new Visitor("123abc");
            visitorManager.AddVisitor(visitor);
            int expected = 0;

            // Act
            visitorManager.RemoveVisitor(visitor);
            int actual = visitorManager.visitors.Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_RemoveVisitor_Id()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor = new Visitor("123abc");
            visitorManager.AddVisitor(visitor);
            int expected = 0;

            // Act
            visitorManager.RemoveVisitor("123abc");
            int actual = visitorManager.visitors.Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_GetVisitor()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor = new Visitor("123abc");
            visitorManager.AddVisitor(visitor);
            Visitor expected = visitor;

            // Act
            Visitor actual = visitorManager.GetVisitor("123abc");

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_UpdateVisitor()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor = new Visitor("123abc");
            visitorManager.AddVisitor(visitor);
            visitor.isAuthenicated = true;
            visitor.username = "test";
            Visitor expected = visitor;

            // Act
            visitorManager.UpdateVisitor(visitor);
            Visitor actual = visitorManager.GetVisitor("123abc");

            // Assert
            Assert.AreEqual(expected, actual);
        }
        [TestMethod]
        public void Test_UpdateVisitor_Multiple()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor1 = new Visitor("123abc");
            Visitor visitor2 = new Visitor("456def");
            visitorManager.AddVisitor(visitor1);
            visitorManager.AddVisitor(visitor2);
            visitor1.isAuthenicated = true;
            visitor1.username = "test";
            visitor2.isAuthenicated = true;
            visitor2.username = "test";
            Visitor expected1 = visitor1;
            Visitor expected2 = visitor2;

            // Act
            visitorManager.UpdateVisitor(visitor1);
            visitorManager.UpdateVisitor(visitor2);
            Visitor actual1 = visitorManager.GetVisitor("123abc");
            Visitor actual2 = visitorManager.GetVisitor("456def");

            // Assert
            Assert.AreEqual(expected1, actual1);
            Assert.AreEqual(expected2, actual2);
        }

        [TestMethod]
        public void Test_VisitorCount()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor = new Visitor("123abc");
            visitorManager.AddVisitor(visitor);
            int expected = 1;

            // Act
            int actual = visitorManager.visitors.Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_VisitorCount_Multiple()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor1 = new Visitor("123abc");
            Visitor visitor2 = new Visitor("456def");
            visitorManager.AddVisitor(visitor1);
            visitorManager.AddVisitor(visitor2);
            int expected = 2;

            // Act
            int actual = visitorManager.visitors.Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_VisitorCount_Remove()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor = new Visitor("123abc");
            visitorManager.AddVisitor(visitor);
            visitorManager.RemoveVisitor(visitor);
            int expected = 0;

            // Act
            int actual = visitorManager.visitors.Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_VisitorCount_Remove_Multiple()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor1 = new Visitor("123abc");
            Visitor visitor2 = new Visitor("456def");
            visitorManager.AddVisitor(visitor1);
            visitorManager.AddVisitor(visitor2);
            visitorManager.RemoveVisitor(visitor1);
            visitorManager.RemoveVisitor(visitor2);
            int expected = 0;

            // Act
            int actual = visitorManager.visitors.Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_VisitorCount_Remove_Id()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor = new Visitor("123abc");
            visitorManager.AddVisitor(visitor);
            visitorManager.RemoveVisitor("123abc");
            int expected = 0;

            // Act
            int actual = visitorManager.visitors.Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Test_VisitorCount_Remove_Id_Multiple()
        {
            // Arrange
            VisitorManager visitorManager = new VisitorManager();
            Visitor visitor1 = new Visitor("123abc");
            Visitor visitor2 = new Visitor("456def");
            visitorManager.AddVisitor(visitor1);
            visitorManager.AddVisitor(visitor2);
            visitorManager.RemoveVisitor("123abc");
            visitorManager.RemoveVisitor("456def");
            int expected = 0;

            // Act
            int actual = visitorManager.visitors.Count;

            // Assert
            Assert.AreEqual(expected, actual);
        }
    }
}