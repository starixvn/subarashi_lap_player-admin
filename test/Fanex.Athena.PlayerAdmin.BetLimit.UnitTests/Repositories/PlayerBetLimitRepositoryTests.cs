using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Fanex.Data;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Repositories
{
    public class PlayerBetLimitRepositoryTests
    {
        private IDbFactory _dbFactory;
        private IObjectDb _objectDb;
        private IDataReader _dataReader;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IDbFactory>();
            _objectDb = Substitute.For<IObjectDb>();
            _dataReader = Substitute.For<IDataReader>();
        }

        [Test]
        public async Task GetEnableBetLimitProducts_WhenCall_ReturnCorrectResult()
        {
            //Arrange
            var siteName = FakeHelper.Create<string>();
            var expectedResult = FakeHelper.CreateMany<PlayerBetLimitProduct>();
            _dbFactory.GetObjectDb("Lrf_LicenseeProductByBetLimitSel").Returns(_objectDb);
            _objectDb.ExecuteReaderAsync().Returns(_dataReader);
            _dataReader.FakeNumberOfRead(expectedResult);
            AssignProductsToReader(expectedResult);

            var repo = CreateRepo();

            //Act
            var result = await repo.GetEnableBetLimitProducts(siteName);

            //Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received(1).SetParameterValue("LicenseeName", siteName);
                Assert.AreEqual(expectedResult.Count(), result.Count());
                Assert.AreEqual(expectedResult.Last().ProductId, result.Last().ProductId);
            });
        }

        private PlayerBetLimitRepository CreateRepo()
            => new PlayerBetLimitRepository(_dbFactory);

        private void AssignProductsToReader(IEnumerable<PlayerBetLimitProduct> expectedResult)
        {
            var firstItem = expectedResult.First();
            var remainItems = expectedResult.Skip(1);
            _dataReader["IsEnable"].Returns(true, remainItems.Select(_ => (object)true).ToArray());
            _dataReader["ProductID"].Returns(firstItem.ProductId, remainItems.Select(_ => (object)_.ProductId).ToArray());
            _dataReader["ProductCode"].Returns(firstItem.ProductCode, remainItems.Select(_ => (object)_.ProductCode).ToArray());
        }
    }
}
