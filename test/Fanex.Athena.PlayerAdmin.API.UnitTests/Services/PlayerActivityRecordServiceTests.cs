

using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    public class PlayerActivityRecordServiceTests
    {
        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb= Substitute.For<IObjectDb>();
        }

        [Test]
        public async Task GetActivityRecord_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var model = FakeHelper.Create<PlayerActivityRecordMetaModel>();
            _dbFactory.GetObjectDb("Lrf_CustomerLoginInfo").Returns(_objectDb);
            var expectedResult = FakeHelper.Create<PlayerActivityRecord>();
            _objectDb.QueryEntityAsync<PlayerActivityRecord>().Returns(expectedResult);
            var service = CreateService();

            //Act
            var result = await service.GetActivityRecord(model);

            //Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().SetParameterValue("custid", model.CustId);
                _objectDb.Received().SetParameterValue("siteName", model.SiteName);
                Assert.AreSame(expectedResult, result);
            });
        }

        private PlayerActivityRecordService CreateService()
            => new PlayerActivityRecordService(_dbFactory);
    }
}
