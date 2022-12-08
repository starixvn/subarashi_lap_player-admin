

using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Repositories
{
    public class ThirdPartyBetLimitRepositoryTests
    {
        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb = Substitute.For<IObjectDb>();
        }

        [Test]
        public async Task GetSettingAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var custID = FakeHelper.Create<int>();
            var productId = FakeHelper.Create<int>();
            var expectedResult = FakeHelper.Create<ThirdPartyBetSetting>();
            _dbFactory.GetObjectDb("Lrf_3rdProductSelCustStatus").Returns(_objectDb);
            AssignReturnResult(expectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetSettingAsync(custID, productId);

            //Assert
            Assert.AreEqual(expectedResult.IsEnabled, result.IsEnabled);
            Assert.AreEqual(expectedResult.IsLineEnabled, result.IsLineEnabled);
            Assert.AreEqual(expectedResult.CurrencyId, result.CurrencyId);
        }

        [Test]
        public async Task UpdateStatusAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateStatusMetaModel>();
            var expectedMsg = FakeHelper.Create<string>();
            _dbFactory.GetObjectDb("Lrf_3rdProductUpdCustStatus").Returns(_objectDb);
            _objectDb.GetParameterValue("ErrMsg").Returns(expectedMsg);

            var repo = CreateRepo();

            //Act
            var result = await repo.UpdateStatusAsync(metaModel);

            //Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("custid", metaModel.CustId);
                _objectDb.Received(1).SetParameterValue("userid", metaModel.UserId);
                _objectDb.Received(1).SetParameterValue("productid", metaModel.ProductId);
                _objectDb.Received(1).SetParameterValue("isenabled", metaModel.Status);
                _objectDb.Received(1).SetParameterValue("ip", metaModel.Ip);
                _objectDb.Received(1).SetParameterValue("language", metaModel.Language);
                _objectDb.Received(1).SetParameterValue("ErrMsg", null);
                Assert.AreEqual(0, result.Code);
                Assert.AreEqual(expectedMsg, result.Message);
            });
        }

        private ThirdPartyBetLimitRepository CreateRepo()
            => new ThirdPartyBetLimitRepository(_dbFactory);

        private void AssignReturnResult(ThirdPartyBetSetting expectedResult)
        {
            _objectDb.GetParameterValue("IsEnabled").Returns(expectedResult.IsEnabled);
            _objectDb.GetParameterValue("IsLineEnabled").Returns(expectedResult.IsLineEnabled);
            _objectDb.GetParameterValue("Currency").Returns(expectedResult.CurrencyId);
        }

    }

}
