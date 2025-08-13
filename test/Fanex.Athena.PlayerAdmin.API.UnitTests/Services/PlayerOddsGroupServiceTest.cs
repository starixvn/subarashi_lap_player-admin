namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    using API.MetaModel;
    using API.Services;
    using AutoFixture;
    using Core.DataDomains;
    using Core.Mediator;
    using Core.Repositories;
    using Data;
    using MediatR;
    using Moq;

    public class PlayerOddsGroupServiceTest
    {
        private readonly Fixture _fixture;
        private readonly Mock<IDbFactory> _dbFactoryMock;
        private readonly Mock<IPublisher> _publisherMock;
        private readonly PlayerOddsGroupService _playerOddsGroupService;

        public PlayerOddsGroupServiceTest()
        {
            _fixture = new Fixture();
            _dbFactoryMock = new Mock<IDbFactory>();
            _publisherMock = new Mock<IPublisher>();
            _playerOddsGroupService = new PlayerOddsGroupService(_dbFactoryMock.Object, _publisherMock.Object);
        }

        [Theory]
        [InlineData(0, true, LogText.Success)]
        [InlineData(1, false, LogText.False)]
        public async Task Update_ShouldReturnCorrectResultAndPublishEvent(int errCode, bool expectedSuccess, string expectedStatus)
        {
            var metaModel = _fixture.Create<OddsGroupUpdatingMetaModel>();

            var objectDbMock = new Mock<IObjectDb>();

            objectDbMock.Setup(db => db.GetParameterValue("ErrCode")).Returns(errCode);
            objectDbMock.Setup(db => db.GetParameterValue("ErrMsg")).Returns(_fixture.Create<string>());

            _dbFactoryMock.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(objectDbMock.Object);
            
            var result = await _playerOddsGroupService.Update(metaModel);
            
            Assert.Equal(expectedSuccess, result.Success);
            Assert.Equal(errCode, result.ErrorCode);

            _publisherMock.Verify(p => p.Publish(It.Is<PlayerUpdatedOddsGroupEvent>(
                e => e.CustID == metaModel.CustId &&
                     e.Status == expectedStatus &&
                     e.NewValue == metaModel.OddsGroup &&
                     e.OldValue == metaModel.OldValue
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
