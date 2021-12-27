using NUnit.Framework;
using Service;

namespace BiscoitoUnitTest
{

    [TestFixture]
    public class UserTest
    {

        private UserService _serv;

        [SetUp]
        public void Setup()
        {
            _serv = new UserService();
        }

        [Test]
        public void Test1()
        {
            var value = _serv.UnitTest(false);
            Assert.IsTrue(value);
        }


    }
}