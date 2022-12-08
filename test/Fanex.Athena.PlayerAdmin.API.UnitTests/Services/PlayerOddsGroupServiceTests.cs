using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.Core.Mediator;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using System.Data;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    public class PlayerOddsGroupServiceTests
    {
        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;
        private IPublisher _publisher;

        [SetUp]
        public void Setup() {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb = Substitute.For<IObjectDb>();
            _publisher = Substitute.For<IPublisher>();
        }

        [Test]
        public async Task Update_WhenCall_CallRelevantMethod()
        {
            //Arrange
            var model = FakeHelper.Create<OddsGroupUpdatingMetaModel>();
            _dbFactory.GetObjectDb("Lrf_CustomerUpdUgroup").Returns(_objectDb);

            var service = CreateService();

            //Act
            await service.Update(model);

            //Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().SetParameterValue("UserId", model.UserId);
                _objectDb.Received().SetParameterValue("CustId", model.CustId);
                _objectDb.Received().SetParameterValue("OddsGroup", model.OddsGroup);
                _objectDb.Received().SetParameterValue("Langkey", model.Language);
                _objectDb.Received().SetParameterValue("IP", model.Ip);
                _objectDb.Received().SetParameterValue("ErrCode", null);
                _objectDb.Received().SetParameterValue("ErrMsg", null);
                _publisher.Received().Publish(Arg.Any<PlayerUpdatedOddsGroupEvent>());
            });
        }

        private PlayerOddsGroupService CreateService()
            => new PlayerOddsGroupService(_dbFactory, _publisher);
    }
}
