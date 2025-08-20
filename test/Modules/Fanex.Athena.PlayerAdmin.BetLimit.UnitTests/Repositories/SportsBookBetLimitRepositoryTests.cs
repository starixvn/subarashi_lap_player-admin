namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Repositories
{
    using System.Data;
    using System.Data.Common;
    using AutoFixture;
    using BetLimit.Repositories;
    using Core.Repositories;
    using Data;
    using MetaModel;
    using Moq;

    public class SportsBookBetLimitRepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IDbFactory> _dbFactoryMock;
        private readonly SportsBookBetLimitRepository _sportsBookBetLimitRepository;

        public SportsBookBetLimitRepositoryTests()
        {
            _fixture = new Fixture();
            _dbFactoryMock = new Mock<IDbFactory>();
            _sportsBookBetLimitRepository = new SportsBookBetLimitRepository(_dbFactoryMock.Object);
        }

        [Fact]
        public async Task GetSettingAsync_ShouldReturnCorrectData()
        {
            var custId = _fixture.Create<int>();
            
            var readerMock = new Mock<IDataReader>();
            var objectDbMock = new Mock<IObjectDb>();

            readerMock.SetupSequence(r => r.Read())
                .Returns(true)
                .Returns(false);
            
            objectDbMock.Setup(db => db.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>())).ReturnsAsync(readerMock.Object);
            _dbFactoryMock.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(objectDbMock.Object);
            
            var result = await _sportsBookBetLimitRepository.GetSettingAsync(custId, true);
            
            Assert.Equal(10, result.Count());
        }

        [Fact]
        public async Task UpdateSettingAsync_ShouldReturnCorrectResult()
        {
            var metaModel = _fixture.Create<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>();

            var objectDbMock = new Mock<IObjectDb>();
            var expectedErrorCode = _fixture.Create<int>();
            var expectedErrorMessage = _fixture.Create<string>();
            
            objectDbMock.Setup(db => db.GetParameterValue("ErrCode")).Returns(expectedErrorCode);
            objectDbMock.Setup(db => db.GetParameterValue("ErrMsg")).Returns(expectedErrorMessage);
            
            _dbFactoryMock.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(objectDbMock.Object);
            
            var result = await _sportsBookBetLimitRepository.UpdateSettingAsync(metaModel, true);
            
            Assert.Equal(expectedErrorCode, result.Code);
            Assert.Equal(expectedErrorMessage, result.Message);
        }
    }
}
