

using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories;
using Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.TestHelper;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Repositories
{
    public class NewDcsBetLimitRepositoryTests
    {
        private const string InvalidErrMsg = "<Root>";
        private const string ValidErrMsg = "<Root Msg=\"ErrorMsg\"></Root>";

        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;
        private IDataReader _dataReader;
        private ILogger<DcsBetLimitRepository> _logger;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb = Substitute.For<IObjectDb>();
            _dataReader = Substitute.For<IDataReader>();
            _logger = Substitute.For<ILogger<DcsBetLimitRepository>>();
        }

        [Test]
        public async Task GetSettingAsync_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var custId = FakeHelper.Create<int>();
            var productId = ProductTypes.VS3;
            var expectedResult = FakeHelper.CreateMany<PlayerBetLimit>();
            _dbFactory.GetObjectDb("Acc_Dep_CustSetting_NewBetLimitSettingSelect").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);

            _dataReader.FakeNumberOfRead(expectedResult);
            _dataReader.AssignCommonBetLimitField(expectedResult);

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
            var expectedResult = FakeHelper.Create<ResultInfo>();
            metaModel.ProductId = ProductTypes.VS3;
            metaModel.Setting.First().SportType = "231";
            _dbFactory.GetObjectDb("Acc_Dep_CustSetting_NewBetLimitUpd").Returns(_objectDb);
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

        private NewDcsBetLimitRepository CreateRepo()
            => new NewDcsBetLimitRepository(_dbFactory, _logger);

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
