using Fanex.Athena.PlayerAdmin.Core.Mediator;
using Fanex.Athena.PlayerAdmin.Core.Repositories;
using Fanex.Athena.PlayerAdmin.UnitTest.Common.Helpers;
using MediatR;
using NSubstitute;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Mediator.Handlers
{
    public class PlayerUpdatedHandlerTests
    {
        private ILogRepository _logRepository;

        [SetUp]
        public void SetUp()
        {
            _logRepository = Substitute.For<ILogRepository>();
        }

        [TestCaseSource(nameof(Events))]
        public async Task Handle_WhenCall_CallRelevantMethod(PlayerUpdatedEvent updatedEvent)
        {
            //Arrange
            var handler = Create<PlayerUpdatedEvent>();

            //Act
            await handler.Handle(updatedEvent, default);

            //Assert
            await _logRepository.ReceivedWithAnyArgs(1).Write(default);
        }

        private PlayerUpdatedHandler<T> Create<T>()
            where T : PlayerUpdatedEvent, INotification
            => new PlayerUpdatedHandler<T>(_logRepository);

        private static PlayerUpdatedEvent[] Events =
        {
            FakeHelper.Create<PlayerUpdatedBAEvent>(),
            FakeHelper.Create<PlayerUpdatedOddsGroupEvent>(),
            FakeHelper.Create<PlayerUpdatedSpecialLevelEvent>(),
        };

    }
}
