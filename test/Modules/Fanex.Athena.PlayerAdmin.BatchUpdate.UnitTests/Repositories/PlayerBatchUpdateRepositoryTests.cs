namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Repositories
{
    using System.Data;
    using System.Data.Common;
    using AutoFixture;
    using BatchUpdate.Criteria;
    using BatchUpdate.MetaModels;
    using BatchUpdate.Repositories;
    using Core.Repositories;
    using Data;
    using Moq;

    public class PlayerBatchUpdateRepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IObjectDb> _objectDbMock;
        private readonly Mock<IDataReader> _readerMock;
        private readonly PlayerBatchUpdateRepository _sut;

        public PlayerBatchUpdateRepositoryTests()
        {
            DbSettingProviderManager
                .StartTestingSession()
                .Run();

            _fixture = new Fixture();
            _readerMock = new Mock<IDataReader>();
            _objectDbMock = CreateObjectDb(_readerMock);

            var dbFactory = new DbFactory();
            _sut = new PlayerBatchUpdateRepository(dbFactory);
        }

        [Fact]
        public async Task GetOldValues_ShouldReturnDictionary_WhenReaderHasData()
        {
            // Arrange
            var custIds = _fixture.CreateMany<int>();
            var type = GetOldValuesCriteria.SpecialLevelType;
            var custId1 = "123";
            var custId2 = "456";
            var value1 = "level1";
            var value2 = "level2";

            _readerMock.SetupSequence(x => x.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            // Setup the indexer calls for custid column - first call returns custId1, second call returns custId2
            _readerMock.SetupSequence(x => x["custid"])
                .Returns(custId1)
                .Returns(custId2);

            // Setup the indexer calls for SpecialLevel column 
            _readerMock.SetupSequence(x => x["SpecialLevel"])
                .Returns(value1)
                .Returns(value2);

            // Act
            var result = await _sut.GetOldValues(custIds, type);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal(value1, result[custId1]);
            Assert.Equal(value2, result[custId2]);
            _objectDbMock.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public async Task GetOldValues_ShouldReturnEmptyDictionary_WhenReaderHasNoData()
        {
            // Arrange
            var custIds = _fixture.CreateMany<int>();
            var type = GetOldValuesCriteria.SpecialLevelType;

            _readerMock.Setup(x => x.Read()).Returns(false);

            // Act
            var result = await _sut.GetOldValues(custIds, type);

            // Assert
            Assert.Empty(result);
            _objectDbMock.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnResultInfo_WhenCalled()
        {
            // Arrange
            var metaModelMock = new Mock<BasePlayerBatchUpdateApiMetaModel>();
            var criteriaMock = new Mock<BasePlayerBatchUpdateCriteria>(metaModelMock.Object, _fixture.Create<string>());
            var expectedCode = _fixture.Create<int>();
            var expectedMessage = _fixture.Create<string>();

            _objectDbMock.Setup(x => x.GetParameterValue("ErrCode")).Returns(expectedCode);
            _objectDbMock.Setup(x => x.GetParameterValue("ErrMsg")).Returns(expectedMessage);

            // Act
            var result = await _sut.Update(criteriaMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCode, result.Code);
            Assert.Equal(expectedMessage, result.Message);
            _objectDbMock.Verify(x => x.Dispose(), Times.Once);
        }

        private static Mock<IObjectDb> CreateObjectDb(Mock<IDataReader> mockDataReader)
        {
            var mockObjectDB = new Mock<IObjectDb>();
            mockObjectDB.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(mockDataReader.Object));

            ObjectDbFactory.SwitchToTestingMode();
            ObjectDbFactory.Mock((x, y, z) => mockObjectDB.Object);

            return mockObjectDB;
        }
    }
}