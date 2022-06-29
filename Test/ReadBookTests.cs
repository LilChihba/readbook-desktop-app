using ReadBook;

namespace Test
{
    [TestClass]
    public class ReadBookTests
    {
        [TestMethod]
        public void Test_Open_Connection()
        {
            Database connection = new Database();

            connection.Open();
            string state = connection.GetState().ToString();

            Assert.AreEqual("Open", state);
        }

        [TestMethod]
        public void Test_Close_Connection()
        {
            Database connection = new Database();

            connection.Close();
            string state = connection.GetState().ToString();

            Assert.AreEqual("Closed", state);
        }
    }
}