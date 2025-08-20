namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Repositories
{
    using AutoFixture;
    using BetLimit.Repositories;
    using Core.Repositories;
    using Data;
    using MetaModel;
    using Moq;

    public class ThirdPartyBetLimitRepositoryTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<IDbFactory> _dbFactoryMock;
        private readonly ThirdPartyBetLimitRepository _thirdPartyBetLimitRepository;

        public ThirdPartyBetLimitRepositoryTests()
        {
            _fixture = new Fixture();
            _dbFactoryMock = new Mock<IDbFactory>();
            _thirdPartyBetLimitRepository = new ThirdPartyBetLimitRepository(_dbFactoryMock.Object);
        }

        [Fact]
        public async Task GetSettingAsync_ShouldReturnCorrectData()
        {
            var custId = _fixture.Create<int>();
            var productId = _fixture.Create<int>();

            var objectDbMock = new Mock<IObjectDb>();
            var expectedIsEnabled = _fixture.Create<bool>();
            var expectedIsLineEnabled = _fixture.Create<bool>();
            var expectedCurrencyId = _fixture.Create<int>();

            objectDbMock.Setup(db => db.GetParameterValue("IsEnabled")).Returns(expectedIsEnabled);
            objectDbMock.Setup(db => db.GetParameterValue("IsLineEnabled")).Returns(expectedIsLineEnabled);
            objectDbMock.Setup(db => db.GetParameterValue("Currency")).Returns(expectedCurrencyId);

            _dbFactoryMock.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(objectDbMock.Object);
            
            var result = await _thirdPartyBetLimitRepository.GetSettingAsync(custId, productId);
            
            Assert.Equal(expectedIsEnabled, result.IsEnabled);
            Assert.Equal(expectedIsLineEnabled, result.IsLineEnabled);
            Assert.Equal(expectedCurrencyId, result.CurrencyId);
        }

        [Fact]
        public async Task UpdateStatusAsync_ShouldReturnCorrectResult()
        {
            var metaModel = _fixture.Create<PlayerBetLimitUpdateStatusMetaModel>();

            var objectDbMock = new Mock<IObjectDb>();
            var expectedErrorMessage = _fixture.Create<string>();

            objectDbMock.Setup(db => db.GetParameterValue("ErrMsg")).Returns(expectedErrorMessage);
            _dbFactoryMock.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(objectDbMock.Object);
            
            var result = await _thirdPartyBetLimitRepository.UpdateStatusAsync(metaModel);
            
            Assert.Equal(0, result.Code);
            Assert.Equal(expectedErrorMessage, result.Message);
        }
    }
}
