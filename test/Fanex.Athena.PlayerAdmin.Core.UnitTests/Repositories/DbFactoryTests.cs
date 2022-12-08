using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Repositories
{
    public class DbFactoryTests
    {
        private IObjectDb _objectDb;

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
        }

        [Test]
        public void GetObjectDb_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var factory = new DbFactory();
            CreateObjectDbFunc handler = CreateFunctionMethod;
            DbSettingProviderManager.StartTestingSession();
            ObjectDbFactory.SwitchToTestingMode();
            ObjectDbFactory.Mock(handler);

            //Act
            var result = factory.GetObjectDb("test");

            //Assert
            Assert.That(result, Is.SameAs(_objectDb));
        }

        private IObjectDb CreateFunctionMethod(string key, int? commandTimeout = null)
            => _objectDb;
    }
}
