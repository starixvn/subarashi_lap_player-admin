using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories;
using Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.TestHelper;
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
    public class AGCasinoBetLimitRepositoryTests
    {
        private IDbFactory _db;
        private IObjectDb _objectDb;

        [SetUp]
        public void Setup()
        {
            _db = Substitute.For<IDbFactory>();
            _objectDb = Substitute.For<IObjectDb>();
        }

        [Test]
        public async Task GetBetSettingAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var expectedResult = FakeHelper.Create<AgCasinoBetSetting>();
            _db.GetObjectDb("Lrf_AGC_SelCustSetting").Returns(_objectDb);
            AssignResultToParameter(expectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetBetSettingAsync(custId);

            //Assert
            Assert.Multiple(() =>
            {
                AssertAssignBetSettingParameters(_objectDb, custId);
                AssertCorrectResult(expectedResult, result);
            });
        }

        [Test]
        public async Task UpdateSettingAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel>>();
            _db.GetObjectDb("Lrf_AGC_UpdCustSetting").Returns(_objectDb);
            var expectedResult = FakeHelper.Create<ResultInfo>();
            AssignResultToParameter(expectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.UpdateSettingAsync(metaModel);

            //Assert
            Assert.Multiple(() =>
            {
                AssertAssignUpdateParameter(_objectDb, metaModel);
                Assert.AreEqual(expectedResult.Code, result.Code);
                Assert.AreEqual(expectedResult.Message, result.Message);
            });
        }

        [Test]
        public async Task UpdateStatusAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateStatusMetaModel>();
            _db.GetObjectDb("Lrf_AGC_UpdCustStatus").Returns(_objectDb);
            var expectedResult = FakeHelper.Create<ResultInfo>();
            AssignResultToParameter(expectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.UpdateStatusAsync(metaModel);

            //Assert
            Assert.Multiple(() =>
            {
                AssertAssignUpdateStatusParameter(_objectDb, metaModel);
                Assert.AreEqual(expectedResult.Code, result.Code);
                Assert.AreEqual(expectedResult.Message, result.Message);
            });
        }

        private AGCasinoBetLimitRepository CreateRepo()
            => new AGCasinoBetLimitRepository(_db);

        private void AssignResultToParameter(AgCasinoBetSetting expectedResult)
        {
            _objectDb.GetParameterValue("Currency").Returns(expectedResult.CurrencyId);
            _objectDb.GetParameterValue("AGGroup").Returns(expectedResult.AgGroup);
            _objectDb.GetParameterValue("IsEnabled").Returns(expectedResult.IsEnabled);
            _objectDb.GetParameterValue("IsLineEnabled").Returns(expectedResult.IsLineEnabled);
            _objectDb.GetParameterValue("CheckedGroups").Returns(string.Join(",",expectedResult.CheckedGroups));
        }

        private void AssignResultToParameter(ResultInfo expectedResult)
            => _objectDb.AssignResultToParameter(expectedResult);

        private void AssertAssignBetSettingParameters(IObjectDb objectDb, int custId)
        {
            objectDb.Received(1).SetParameterValue("CustID", custId);
            objectDb.Received(1).SetParameterValue("Currency", null);
            objectDb.Received(1).SetParameterValue("AGGroup", null);
            objectDb.Received(1).SetParameterValue("IsEnabled", null);
            objectDb.Received(1).SetParameterValue("IsLineEnabled", null);
            objectDb.Received(1).SetParameterValue("CheckedGroups", null);
        }

        private void AssertAssignUpdateParameter(IObjectDb objectDb, PlayerBetLimitUpdateMetaModel<AgCasinoBetLimitMetaModel> metaModel)
        {
            objectDb.Received(1).SetParameterValue("aggroup", metaModel.Setting.Group);
            objectDb.Received(1).SetParameterValue("ip", metaModel.Ip);
            objectDb.Received(1).SetParameterValue("language", metaModel.Language);
            objectDb.Received(1).SetParameterValue("userid", metaModel.UserId);
            objectDb.Received(1).SetParameterValue("custid", metaModel.CustId);

            objectDb.Received(1).SetParameterValue("ErrCode", null);
            objectDb.Received(1).SetParameterValue("ErrMsg", null);
        }

        private void AssertAssignUpdateStatusParameter(IObjectDb objectDb, PlayerBetLimitUpdateStatusMetaModel metaModel)
        {
            objectDb.Received(1).SetParameterValue("isenabled", metaModel.Status);
            objectDb.Received(1).SetParameterValue("ip", metaModel.Ip);
            objectDb.Received(1).SetParameterValue("language", metaModel.Language);
            objectDb.Received(1).SetParameterValue("userid", metaModel.UserId);
            objectDb.Received(1).SetParameterValue("custid", metaModel.CustId);

            objectDb.Received(1).SetParameterValue("ErrCode", null);
            objectDb.Received(1).SetParameterValue("ErrMsg", null);
        }

        private void AssertCorrectResult(AgCasinoBetSetting expectedResult, AgCasinoBetSetting result)
        {
            Assert.AreEqual(expectedResult.CurrencyId, result.CurrencyId);
            Assert.AreEqual(expectedResult.AgGroup, result.AgGroup);
            Assert.AreEqual(expectedResult.IsEnabled, result.IsEnabled);
            Assert.AreEqual(expectedResult.IsLineEnabled, result.IsLineEnabled);
            Assert.AreEqual(expectedResult.CheckedGroups, result.CheckedGroups);
        }
    }
}
