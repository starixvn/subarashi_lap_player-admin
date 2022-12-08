

using Fanex.Athena.PlayerAdmin.BatchUpdate.Criteria;
using Fanex.Athena.PlayerAdmin.BatchUpdate.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Models;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using NSubstitute;
using NUnit.Framework;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Repositories
{
    public class PlayerBatchUpdateRepositoryTests
    {
        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;
        private IDataReader _dataReader;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb= Substitute.For<IObjectDb>();
            _dataReader = Substitute.For<IDataReader>();
        }

        [Test]
        public async Task GetOldValues_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var type = GetOldValuesCriteria.SpecialLevelType;
            var custId = FakeHelper.Create<string>();
            const string expectedColumnName = "SpecialLevel";
            const string spName = "Lrf_CustomerSpecialLevelSel";
            _dbFactory.GetObjectDb(spName).Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            _dataReader.Read().Returns(true, false);

            _dataReader["custid"].Returns(custId);
            _dataReader[expectedColumnName].Returns(FakeHelper.Create<string>());

            var repo = CreateRepository();

            //Act
            var result = await repo.GetOldValues(FakeHelper.CreateMany<int>(), type);

            //Assert
            Assert.Contains(custId, result.Keys.ToList());
        }

        [Test]
        public async Task Update_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _dbFactory.GetObjectDb(default).ReturnsForAnyArgs(_objectDb);
            _objectDb.GetParameterValue("ErrCode").Returns(expectedResult.Code.ToString());
            _objectDb.GetParameterValue("ErrMsg").Returns(expectedResult.Message);

            var repo = CreateRepository();

            //Act
            var result = await repo.Update(FakeHelper.Create<BasePlayerBatchUpdateCriteria>());

            //Assert
            Assert.AreEqual(expectedResult.Code, result.Code);
            Assert.AreEqual(expectedResult.Message, result.Message);
        }

        [Test]
        public async Task UpdateBAStatus_WhenCall_ReturnCorrectValue()
        {
            //Arrange
            var expectedResult = FakeHelper.Create<ResultInfo>();
            _dbFactory.GetObjectDb("Lrf_CustomerUpdMemberBAStatus").Returns(_objectDb);
            _objectDb.GetParameterValue("ErrCode").Returns(expectedResult.Code.ToString());
            _objectDb.GetParameterValue("ErrMsg").Returns(expectedResult.Message);
            _objectDb.GetParameterValue("OddsGroup").Returns(expectedResult.ExtendedData);

            var repo = CreateRepository();

            //Act
            var result = await repo.UpdateBAStatus(FakeHelper.Create<UpdateBAStatusCriteria>());

            //Assert
            Assert.AreEqual(expectedResult.Code, result.Code);
            Assert.AreEqual(expectedResult.Message, result.Message);
            Assert.AreEqual(expectedResult.ExtendedData, result.ExtendedData);
        }

        private PlayerBatchUpdateRepository CreateRepository()
            => new PlayerBatchUpdateRepository(_dbFactory);
    }
}
