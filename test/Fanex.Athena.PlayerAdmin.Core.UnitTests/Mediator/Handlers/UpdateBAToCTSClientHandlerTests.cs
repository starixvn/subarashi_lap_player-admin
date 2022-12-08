using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.DataDomains;
using Fanex.Athena.PlayerAdmin.Core.Mediator;
using Fanex.Athena.PlayerAdmin.Core.Mediator.Handlers;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.Extensions.Options;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Mediator.Handlers
{
    public class UpdateBAToCTSClientHandlerTests
    {
        private ICTSApiClient _client;
        private CustomerClassApiSetting _setting;

        [SetUp]
        public void SetUp()
        {
            _setting = FakeHelper.Create<CustomerClassApiSetting>();
            _client = Substitute.For<ICTSApiClient>();

        }

        [TestCase(false, LogText.Success)]
        [TestCase(true, LogText.False)]
        public async Task Handle_WhenSetingNotEnableOrEventStatusNotSuccess_NotCallCTSApi(bool enabled, string statusText)
        {
            //Arrange
            var @event = FakeHelper.Create<PlayerUpdatedBAEvent>(_ => _.Status, statusText);
            _setting = FakeHelper.Create<CustomerClassApiSetting>(_ => _.Enabled, enabled);

            var handler = CreateHandler();

            //Act
            await handler.Handle(@event, default);

            //Assert
            await _client.DidNotReceiveWithAnyArgs().PostAsync(default, default, default, default);
        }

        [Test]
        public async Task Handle_WhenSetingEnableAndEventStatusSuccess_CallCTSApi()
        {
            //Arrange
            var @event = FakeHelper.Create<PlayerUpdatedBAEvent>(_ => _.Status, LogText.Success);
            @event.IgnoreCTSEvent = false;
            _setting = FakeHelper.Create<CustomerClassApiSetting>(_ => _.Enabled, true);

            var handler = CreateHandler();

            //Act
            await handler.Handle(@event, default);

            //Assert
            await _client.ReceivedWithAnyArgs(1).PostAsync(default, default, default, default);
        }

        private UpdateBAToCTSClientHandler CreateHandler()
        {
            var option = Substitute.For<IOptions<CustomerClassApiSetting>>();
            option.Value.Returns(_setting);
            return new UpdateBAToCTSClientHandler(_client, option);
        }
    }
}
