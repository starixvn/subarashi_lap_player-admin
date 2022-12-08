

using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.Core.Mediator;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    public class PlayerBaStatusServiceTests
    {
        private IDbFactory _dbFactory;
        private IPublisher _publisher;
        private IObjectDb _objectDb;

        [SetUp]
        public void Setup()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb= Substitute.For<IObjectDb>();
            _publisher= Substitute.For<IPublisher>();
        }

        [Test]
        public async Task Update_WhenCall_CallRelevantMethodCorrectly()
        {
            //Arrange
            var model = FakeHelper.Create<BaStatusUpdatingMetaModel>();
            _dbFactory.GetObjectDb("Lrf_CustomerUpdMemberBAStatus").Returns(_objectDb);

            var service = CreateService();

            //Act
            await service.Update(model);

            //Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().SetParameterValue("LicenseeName", model.SiteName);
                _objectDb.Received().SetParameterValue("CustID", model.CustId);
                _objectDb.Received().SetParameterValue("ModifyBy", model.UserId);
                _objectDb.Received().SetParameterValue("IsBA", model.IsBa);
                _objectDb.Received().SetParameterValue("langkey", model.Language);
                _objectDb.Received().SetParameterValue("IP", model.Ip);
                _objectDb.Received().SetParameterValue("OddsGroup", null);
                _objectDb.Received().SetParameterValue("ErrCode", null);
                _objectDb.Received().SetParameterValue("ErrMsg", null);
                _publisher.Received().Publish(Arg.Any<PlayerUpdatedBAEvent>());
            });
        }

        private PlayerBaStatusService CreateService()
            => new PlayerBaStatusService(_dbFactory, _publisher);
    }
}
