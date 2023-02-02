using Fanex.Athena.PlayerAdmin.Commission.Criteria;
using Fanex.Athena.PlayerAdmin.Commission.Repositories;
using Fanex.Athena.PlayerAdmin.Commission.ViewModels;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Repositories
{
    public class PlayerCommissionRepositoryTests
    {
        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;
        private IDataReader _dataReader;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb = Substitute.For<IObjectDb>();
            _dataReader= Substitute.For<IDataReader>();
        }

        [Test]
        public async Task GetAllCommGroups_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var criteria = FakeHelper.Create<GetAllCommGroupsCriteria>();
            criteria.Permissions = Permissions;
            _dbFactory.GetObjectDb(criteria.SPName).Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            SetUpDataTable();

            var expectedResult = new List<PlayerCommissionViewModel>
            {
                FakeHelper.Create<PlayerCommissionViewModel>(),
            };
            var firstExpectedResult = expectedResult.FirstOrDefault();
            _dataReader.FakeNumberOfRead(expectedResult);
            FakeDataComGroupsForDataReader(_dataReader, firstExpectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetAllCommGroups(criteria);
            var firstItem = result.First();

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedResult.Count, result.Count());
                Assert.AreEqual(firstExpectedResult.GroupName, firstItem.GroupName);
                Assert.AreEqual(firstExpectedResult.GroupCommId, firstItem.GroupCommId);
                Assert.AreEqual(100, firstItem.CommValues.First().Value);
            });
        }

        [Test]
        public async Task GetCommissionGroupById_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var criteria = FakeHelper.Create<GetCommissionGroupByIdCriteria>();
            criteria.Permissions = Permissions;
            _dbFactory.GetObjectDb(criteria.SPName).Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            SetUpDataTable();

            var expectedResult = FakeHelper.Create<PlayerCommissionViewModel>();
            _dataReader.Read().Returns(true, false);
            FakeDataComGroupsForDataReader(_dataReader, expectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetCommissionGroupById(criteria);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedResult.GroupName, result.GroupName);
                Assert.AreEqual(expectedResult.GroupCommId, result.GroupCommId);
                Assert.AreEqual(100, result.CommValues.First().Value);
            });
        }

        [Test]
        public async Task GetPlayerCommissionGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var criteria = FakeHelper.Create<GetPlayerCommissionGroupCriteria>();
            criteria.Permissions = Permissions;
            _dbFactory.GetObjectDb(criteria.SPName).Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            SetUpDataTable();

            var expectedResult = FakeHelper.Create<PlayerCommissionViewModel>();
            _dataReader.Read().Returns(true, false);
            FakeDataComGroupsForDataReader(_dataReader, expectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetPlayerCommissionGroup(criteria);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedResult.GroupCommId, result.GroupCommId);
                Assert.AreEqual(100, result.CommValues.First().Value);
            });
        }

        [Test]
        public async Task MultipleMoveGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var criteria = FakeHelper.Create<MultipleMoveGroupCriteria>();
            _dbFactory.GetObjectDb(criteria.GetSPName()).Returns(_objectDb);
            var exptecdResult = FakeHelper.Create<ResultInfo>();
            SetUpReturnResultInfo(exptecdResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.MultipleMoveGroup(criteria);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(exptecdResult.Code, result.Code);
                Assert.AreEqual(exptecdResult.Message, result.Message);
                Assert.AreEqual(exptecdResult.ExtendedData, result.ExtendedData);
            });
        }

        [Test]
        public async Task MultipleRemoveFromGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var criteria = FakeHelper.Create<MultipleRemoveFromGroupCriteria>();
            _dbFactory.GetObjectDb(criteria.GetSPName()).Returns(_objectDb);
            var exptecdResult = FakeHelper.Create<ResultInfo>();
            SetUpReturnResultInfo(exptecdResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.MultipleRemoveFromGroup(criteria);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(exptecdResult.Code, result.Code);
                Assert.AreEqual(exptecdResult.Message, result.Message);
                Assert.AreEqual(exptecdResult.ExtendedData, result.ExtendedData);
            });
        }

        [Test]
        public async Task MultipleUpdateGroup_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var criteria = FakeHelper.Create<MultipleUpdateGroupCriteria>();
            criteria.Permissions = Permissions;
            criteria.Products = Permissions.Keys.ToArray();
            _dbFactory.GetObjectDb(criteria.GetSPName()).Returns(_objectDb);
            var exptecdResult = FakeHelper.Create<ResultInfo>();
            SetUpReturnResultInfo(exptecdResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.MultipleUpdateGroup(criteria);

            //Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(exptecdResult.Code, result.Code);
                Assert.AreEqual(exptecdResult.Message, result.Message);
                Assert.AreEqual(exptecdResult.ExtendedData, result.ExtendedData);
            });
        }

        private void SetUpDataTable()
        {
            var table = new DataTable();
            table.Columns.Add("ColumnName");
            table.Rows.Add("TestComm");
            _dataReader.GetSchemaTable().Returns(table);
        }

        private IDictionary<string, bool> Permissions => new Dictionary<string, bool>()
        {
            {"Test", true }
        };

        private void FakeDataComGroupsForDataReader(IDataReader dataReader, PlayerCommissionViewModel commissions)
        {
            dataReader["GroupName"].Returns(commissions.GroupName);
            dataReader["GroupCommId"].Returns(commissions.GroupCommId);
            dataReader["TestComm"].Returns(100);
        }

        private void SetUpReturnResultInfo(ResultInfo resultInfo)
        {
            _objectDb.GetParameterValue("ErrCode").Returns(resultInfo.Code);
            _objectDb.GetParameterValue("ErrMsg").Returns(resultInfo.Message);
            _objectDb.GetParameterValue("strCustId").Returns(resultInfo.ExtendedData);
        }

        private PlayerCommissionRepository CreateRepo()
            => new PlayerCommissionRepository(_dbFactory);
    }
}
