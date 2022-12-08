using Fanex.Athena.PlayerAdmin.Core.ConsoleClient;
using Fanex.Athena.PlayerAdmin.Core.DataDomains;
using Fanex.Athena.PlayerAdmin.Core.Mediator.Handlers;
using Fanex.Athena.PlayerAdmin.Core.Mediator;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using NSubstitute;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Mediator.Handlers
{
    public class UpdateMultipleBAToCTSClientHandlerTests
    {
        private const string EnableBA = "1";

        private ICTSApiClient _client;
        private CustomerClassApiSetting _setting;

        [SetUp]
        public void SetUp()
        {
            _setting = FakeHelper.Create<CustomerClassApiSetting>();
            _client = Substitute.For<ICTSApiClient>();

        }

        [Test]
        public async Task Handle_WhenSetingNotEnable_NotCallCTSApi()
        {
            //Arrange
            var @event = FakeHelper.Create<PlayerUpdatedMultipleBAToCTSEvent>();
            _setting = FakeHelper.Create<CustomerClassApiSetting>(_ => _.Enabled, false);

            var handler = CreateHandler();

            //Act
            await handler.Handle(@event, default);

            //Assert
            await _client.DidNotReceiveWithAnyArgs().PostAsync(default, default, default, default);
        }

        [TestCase(EnableBA)]
        [TestCase("1000")]
        public async Task Handle_WhenSetingEnable_CallCTSApi(string newValue)
        {
            //Arrange
            var @event = FakeHelper.Create<PlayerUpdatedMultipleBAToCTSEvent>(_ => _.NewValue, newValue);
            _setting = FakeHelper.Create<CustomerClassApiSetting>(_ => _.Enabled, true);

            var handler = CreateHandler();

            //Act
            await handler.Handle(@event, default);

            //Assert
            await _client.ReceivedWithAnyArgs(1).PostAsync(default, default, default, default);
        }

        private UpdateMultipleBAToCTSClientHandler CreateHandler()
        {
            var option = Substitute.For<IOptions<CustomerClassApiSetting>>();
            option.Value.Returns(_setting);
            return new UpdateMultipleBAToCTSClientHandler(_client, option);
        }
    }
}
