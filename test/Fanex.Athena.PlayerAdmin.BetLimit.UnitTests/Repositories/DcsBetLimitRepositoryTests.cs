using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories;
using Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.TestHelper;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Repositories
{
    public class DcsBetLimitRepositoryTests
    {
        private const string InvalidErrMsg = "<Root>";
        private const string ValidErrMsg = "<Root Msg=\"ErrorMsg\"></Root>";
        private const int MemberRoleId = 1;
        private const int ProductStatusSettingType = 4;

        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;
        private IDataReader _dataReader;
        private ILogger<DcsBetLimitRepository> _logger;

        [SetUp]
        public void Setup()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb= Substitute.For<IObjectDb>();
            _dataReader= Substitute.For<IDataReader>();
            _logger = Substitute.For<ILogger<DcsBetLimitRepository>>();
        }

        [Test]
        public async Task GetSettingAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var productId = FakeHelper.Create<int>();
            var expectedResult = FakeHelper.CreateMany<PlayerBetLimit>();
            _dbFactory.GetObjectDb("Acc_Dep_CustSetting_BetLimitSettingSelect").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            _dataReader.FakeNumberOfRead(expectedResult);
            AssignPlayerBetLimitField(_dataReader, expectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetSettingAsync(custId, productId);

            //Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("custid", custId);
                _objectDb.Received(1).SetParameterValue("productid", productId);
                Assert.AreEqual(expectedResult.Count(), result.Count());
            });
        }

        [Test]
        public async Task UpdateSettingAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();
            metaModel.ProductId = ProductTypes.VirtualSports;
            metaModel.Setting.First().SportType = "180";
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _dbFactory.GetObjectDb("Acc_Dep_CustSetting_BetLimitUpd").Returns(_objectDb);
            _objectDb.AssignResultToParameter(expectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.UpdateSettingAsync(metaModel);

            //Assert
            Assert.Multiple(() =>
            {
                AssertReceiveUpdateParameter(metaModel);
                Assert.AreEqual(expectedResult.Code, result.Code);
                Assert.AreEqual(expectedResult.Message, result.Message);
            });
        }

        [TestCase(1, false)]
        [TestCase(0, true)]
        public async Task GetStatusAsync_WhenHaveDataReturnFromDB_ReturnCorrectResult(int dbResult, bool expectedResult)
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var productId = FakeHelper.Create<int>();
            _dbFactory.GetObjectDb("Acc_Dep_CustSetting_StatusSettingSelect").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            _dataReader.Read().Returns(true, false);
            _dataReader["IsDisabled"].Returns(dbResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetStatusAsync(custId, productId);

            //Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("CustID", custId);
                _objectDb.Received(1).SetParameterValue("ProductId", productId);
                _objectDb.Received(1).SetParameterValue("SettingType", ProductStatusSettingType);
                _objectDb.Received(1).SetParameterValue("RoleID", MemberRoleId);
                Assert.AreEqual(expectedResult, result);
            });
        }

        [Test]
        public async Task GetStatusAsync_WhenNotHaveDataReturnFromDb_ReturnFalse()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var productId = FakeHelper.Create<int>();
            _dbFactory.GetObjectDb("Acc_Dep_CustSetting_StatusSettingSelect").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            _dataReader.Read().Returns(false);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetStatusAsync(custId, productId);

            //Assert
            Assert.IsFalse(result);
        }

        [Test]
        public async Task UpdateStatusAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerBetLimitUpdateStatusMetaModel>();
            metaModel.ProductId = ProductTypes.CG;
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _dbFactory.GetObjectDb("Acc_Dep_CustSetting_DisableStatUpd").Returns(_objectDb);
            _objectDb.AssignResultToParameter(expectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.UpdateStatusAsync(metaModel);

            //Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("custid", metaModel.CustId);
                _objectDb.Received(1).SetParameterValue("userid", metaModel.UserId);
                _objectDb.Received(1).SetParameterValue("IsDisabled", Arg.Is<string>(_ => _.Contains("ProductStatus")));
                _objectDb.Received(1).SetParameterValue("ip", metaModel.Ip);
                _objectDb.Received(1).SetParameterValue("ErrCode", null);
                _objectDb.Received(1).SetParameterValue("ErrMsg", null);
                Assert.AreEqual(expectedResult.Code, result.Code);
                Assert.AreEqual(expectedResult.Message, result.Message);
            });
        }

        [Test]
        public void GetErrorMessage_WhenCallWithInvalidMsg_ReturnInvalidMsg()
        {
            //Arrange
            var repo = CreateRepo();

            //Act
            var result = repo.InvokeProtected("GetErrorMessage", InvalidErrMsg);

            //Assert
            Assert.AreEqual(InvalidErrMsg, result);
        }

        [Test]
        public void GetErrorMessage_WhenCallWithValidMsg_ReturnCorrectMessage()
        {
            //Arrange
            var repo = CreateRepo();

            //Act
            var result = repo.InvokeProtected("GetErrorMessage", ValidErrMsg);

            //Assert
            Assert.AreEqual("ErrorMsg", result);
        }

        private DcsBetLimitRepository CreateRepo()
            => new DcsBetLimitRepository(_dbFactory, _logger);

        public void AssignPlayerBetLimitField(IDataReader reader, IEnumerable<PlayerBetLimit> playerBetLimits)
        {
            reader.AssignCommonBetLimitField(playerBetLimits);
            var firstItem = playerBetLimits.First();
            var remainItems = playerBetLimits.Skip(1);

            reader["maxpermatch"].Returns(firstItem.MaxPerMatch, remainItems.Select(_ => (object)_.MaxPerMatch).ToArray());
            reader["maxpermatchmax"].Returns(firstItem.MaxPerMatchMax, remainItems.Select(_ => (object)_.MaxPerMatchMax).ToArray());
        }

        private void AssertReceiveUpdateParameter(PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>> metaModel)
        {
            _objectDb.Received(1).SetParameterValue("custid", metaModel.CustId);
            _objectDb.Received(1).SetParameterValue("userid", metaModel.UserId);
            _objectDb.Received(1).SetParameterValue("ip", metaModel.Ip);
            _objectDb.Received(1).SetParameterValue("errcode", null);
            _objectDb.Received(1).SetParameterValue("errmsg", null);
            _objectDb.Received(1).SetParameterValue("betlimit", Arg.Is<string>(_ => _.Contains("BetLimit")));
        }
    }
}
