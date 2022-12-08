using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    public class PlayerListServiceTests
    {
        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;
        private IDataReader _dataReader;
        private IContextService _contextService;

        [SetUp]
        public void Setup()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _contextService= Substitute.For<IContextService>();
            _objectDb = Substitute.For<IObjectDb>();
            _dataReader= Substitute.For<IDataReader>();
        }

        [Test]
        public async Task Search_WhenIsNotDeposited_ReturnResultWithEmptyDataList()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerSearchingMetaModel>();
            metaModel.IsDeposited = 0;

            var service = CreateService();

            //Act
            var result = await service.Search(metaModel);

            //Assert
            Assert.IsEmpty(result.DataList);
        }

        [Test]
        public async Task Search_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerSearchingMetaModel>();
            var expectedDataList = FakeHelper.CreateMany<Player>();
            var totalRow = expectedDataList.Count();
            metaModel.IsDeposited = 1;
            _dbFactory.GetObjectDb("Shard_Lrf_CustomerSelByUsername").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            FakeReturnFromReader(_dataReader, expectedDataList);
            _objectDb.GetParameterValue("TotalRows").Returns(totalRow);

            var service = CreateService();

            //Act
            var result = await service.Search(metaModel);

            //Assert
            Assert.Multiple(() =>
            {
                AssertAssignCommonData(metaModel);
                AssertAssignParamByUserName(metaModel);
                Assert.AreEqual(totalRow, result.TotalItem);
                Assert.AreEqual(expectedDataList.Count(), result.DataList.Count());
            });
        }

        [Test]
        public async Task GetPlayerListById_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerListByIdMetaModel>();
            var expectedDataList = FakeHelper.CreateMany<Player>();
            var totalRow = expectedDataList.Count();
            _dbFactory.GetObjectDb("Shard_Lrf_CustomerSelByCustid").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            FakeReturnFromReader(_dataReader, expectedDataList);
            _objectDb.GetParameterValue("TotalRows").Returns(totalRow);

            var service = CreateService();

            //Act
            var result = await service.GetPlayerListById(metaModel);

            //Assert
            Assert.Multiple(() =>
            {
                AssertAssignCommonData(metaModel);
                AssertAssignParamByCustId(metaModel);
                Assert.AreEqual(totalRow, result.TotalItem);
                Assert.AreEqual(expectedDataList.Count(), result.DataList.Count());
            });
        }

        [Test]
        public async Task ArchivedSearch_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var metaModel = FakeHelper.Create<PlayerArchivedSearchingMetaModel>();
            var expectedDataList = FakeHelper.CreateMany<Player>();
            var totalRow = expectedDataList.Count();
            _dbFactory.GetObjectDb("Lrf_DormantCustomerSel").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            FakeReturnFromReader(_dataReader, expectedDataList);
            _objectDb.GetParameterValue("TotalRows").Returns(totalRow);

            var service = CreateService();

            //Act
            var result = await service.ArchivedSearch(metaModel);

            //Assert
            Assert.Multiple(() =>
            {
                AssertAssignArchiedSearchParameter(metaModel);
                Assert.AreEqual(totalRow, result.TotalItem);
                Assert.AreEqual(expectedDataList.Count(), result.DataList.Count());
            });
        }

        private PlayerListService CreateService()
            => new PlayerListService(_dbFactory, _contextService);

        private static void FakeReturnFromReader(IDataReader dataReader, IEnumerable<Player> players)
        {
            dataReader.FakeNumberOfRead(players);
            var firstITem = players.First();
            var remaainItems = players.Skip(1);
            dataReader["CustId"].Returns(firstITem.IbcCustId, remaainItems.Select(_ => (object)_.IbcCustId).ToArray());
            dataReader["custid"].Returns(firstITem.IbcCustId, remaainItems.Select(_ => (object)_.IbcCustId).ToArray());
            dataReader["ExchangeId"].Returns(firstITem.CurrencyId, remaainItems.Select(_ => (object)_.CurrencyId).ToArray());
            dataReader["Currency"].Returns(firstITem.Currency, remaainItems.Select(_ => (object)_.Currency).ToArray());
            dataReader["UserName"].Returns(firstITem.SystemId, remaainItems.Select(_ => (object)_.SystemId).ToArray());
            dataReader["username"].Returns(firstITem.SystemId, remaainItems.Select(_ => (object)_.SystemId).ToArray());
            dataReader["NickName"].Returns(firstITem.UserName, remaainItems.Select(_ => (object)_.UserName).ToArray());
            dataReader["nickname"].Returns(firstITem.UserName, remaainItems.Select(_ => (object)_.UserName).ToArray());
            dataReader["BAStatus"].Returns(firstITem.BaStatus, remaainItems.Select(_ => (object)_.BaStatus).ToArray());
            dataReader["SpecialLevel"].Returns(firstITem.SpecialLevel, remaainItems.Select(_ => (object)_.SpecialLevel).ToArray());
            dataReader["OddsGroup"].Returns(firstITem.OddsGroup, remaainItems.Select(_ => (object)_.OddsGroup).ToArray());
            dataReader["TotalBalance"].Returns(firstITem.CurrentBalance, remaainItems.Select(_ => (object)_.CurrentBalance).ToArray());
            dataReader["Outstanding"].Returns(firstITem.OutstandingBalance, remaainItems.Select(_ => (object)_.OutstandingBalance).ToArray());
            dataReader["GroupName"].Returns(firstITem.CommissionGroup, remaainItems.Select(_ => (object)_.CommissionGroup).ToArray());
            dataReader["CustomerClass"].Returns(firstITem.CustomerClass, remaainItems.Select(_ => (object)_.CustomerClass).ToArray());
            dataReader["balance"].Returns(firstITem.CurrentBalance, remaainItems.Select(_ => (object)_.CurrentBalance).ToArray());
        }

        private void AssertAssignCommonData(PlayerListBaseMetaModel metaModel)
        {
            _objectDb.Received().SetParameterValue("LicenseeName", metaModel.SiteName);
            _objectDb.Received().SetParameterValue("AgentList", metaModel.AgentListXml);
            _objectDb.Received().SetParameterValue("CustomerClass", metaModel.CustomerClass);
            _objectDb.Received().SetParameterValue("CommGroups", metaModel.CommGroups);
        }

        private void AssertAssignParamByUserName(PlayerSearchingMetaModel metaModel)
        {
            _objectDb.Received().SetParameterValue("UserName", ToSqlTextSearch(metaModel.UserName));
            _objectDb.Received().SetParameterValue("SearchOption", metaModel.SearchOption);
            _objectDb.Received().SetParameterValue("BaFilter", metaModel.BaFilter);
            _objectDb.Received().SetParameterValue("SpecialLevel", metaModel.SpecialLevel);
            _objectDb.Received().SetParameterValue("OddsGroups", metaModel.OddsGroup);
            _objectDb.Received().SetParameterValue("PageSize", metaModel.PageSize);
            _objectDb.Received().SetParameterValue("PageIndex", metaModel.PageIndex);

            _objectDb.Received().SetParameterValue("TotalRows", 0);
        }

        private void AssertAssignParamByCustId(PlayerListByIdMetaModel metaModel)
        {
            _objectDb.Received().SetParameterValue("CustIds", metaModel.IdList);
            _objectDb.Received().SetParameterValue("BaFilter", metaModel.BaFilter);
            _objectDb.Received().SetParameterValue("SpecialLevel", metaModel.SpecialLevel);
            _objectDb.Received().SetParameterValue("OddsGroups", metaModel.OddsGroup);
            _objectDb.Received().SetParameterValue("PageSize", metaModel.PageSize);
            _objectDb.Received().SetParameterValue("PageIndex", metaModel.PageIndex);
        }

        private void AssertAssignArchiedSearchParameter(PlayerArchivedSearchingMetaModel metaModel)
        {
            var usernameTextSearch = ToSqlTextSearch(metaModel.UserName);
            _objectDb.Received().SetParameterValue("AgentList", metaModel.AgentListXml);
            _objectDb.Received().SetParameterValue("UserName", usernameTextSearch);
            _objectDb.Received().SetParameterValue("SearchOption", metaModel.SearchOption);
            _objectDb.Received().SetParameterValue("BaseCurrency", string.Empty);
            _objectDb.Received().SetParameterValue("PageSize", metaModel.PageSize);
            _objectDb.Received().SetParameterValue("PageIndex", metaModel.PageIndex);
        }

        private static string ToSqlTextSearch(string text)
            => string.IsNullOrEmpty(text) ? text : text.Trim().Replace("_", "[_]");
    }
}
