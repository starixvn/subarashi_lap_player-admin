namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Mediator
{
    using AutoFixture;
    using Core.ConsoleClient;
    using Core.Mediator;
    using Core.Mediator.Handlers;
    using Core.Settings;
    using Microsoft.Extensions.Options;
    using Moq;

    public class UpdateMultipleBAToCTSClientHandlerTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ICTSApiClient> _ctsApiClientMock;
        private readonly UpdateMultipleBAToCTSClientHandler _handler;

        public UpdateMultipleBAToCTSClientHandlerTests()
        {
            _fixture = new Fixture();
            _ctsApiClientMock = new Mock<ICTSApiClient>();
            var settingMock = new Mock<IOptions<CustomerClassApiSetting>>();

            var setting = new CustomerClassApiSetting
            {
                Enabled = true,
                EndPoints = new CustomerClassEndpoint
                {
                    AddLicBA = "addLicBA",
                    RemoveLicBA = "removeLicBA"
                }
            };

            settingMock.Setup(x => x.Value).Returns(setting);
            _handler = new UpdateMultipleBAToCTSClientHandler(_ctsApiClientMock.Object, settingMock.Object);
        }

        [Theory]
        [InlineData("1", "addLicBA")]
        [InlineData("0", "removeLicBA")]
        public async Task Handle_ShouldCallCorrectApi(string newValue, string expectedPath)
        {
            var notification = _fixture.Build<PlayerUpdatedMultipleBAToCTSEvent>()
                .With(x => x.NewValue, newValue)
                .Create();

            await _handler.Handle(notification, CancellationToken.None);
            
            _ctsApiClientMock.Verify(c => c.PostAsync(expectedPath, It.IsAny<object>(), It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }
    }
}
