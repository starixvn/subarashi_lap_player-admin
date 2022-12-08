using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.Core.Mediator;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    public class PlayerSpecialLevelServiceTests
    {
        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;
        private IPublisher _publisher;

        [SetUp]
        public void Setup()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb = Substitute.For<IObjectDb>();
            _publisher = Substitute.For<IPublisher>();
        }

        [Test]
        public async Task Update_WhenCall_CallRelevantMethod()
        {
            //Arrange
            var model = FakeHelper.Create<SpecialLevelUpdatingMetaModel>();
            _dbFactory.GetObjectDb("Lrf_CustomerUpdSpecialLevel").Returns(_objectDb);

            var service = CreateService();

            //Act
            await service.Update(model);

            //Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().SetParameterValue("custid", model.CustId);
                _objectDb.Received().SetParameterValue("userid", model.UserId);
                _objectDb.Received().SetParameterValue("speciallevel", model.SpecialLevel);
                _objectDb.Received().SetParameterValue("ip", model.Ip);
                _objectDb.Received().SetParameterValue("langkey", model.Language);
                _objectDb.Received().SetParameterValue("ErrCode", null);
                _objectDb.Received().SetParameterValue("ErrMsg", null);
                _publisher.Received().Publish(Arg.Any<PlayerUpdatedSpecialLevelEvent>());
            });
        }

        private PlayerSpecialLevelService CreateService()
            => new PlayerSpecialLevelService(_dbFactory, _publisher);
    }
}
