namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Mediator
{
    using Core.Mediator;
    using Core.Repositories;
    using Models;
    using Moq;

    public class PlayerUpdatedHandlerTests
    {
        private readonly Mock<ILogRepository> _logRepository;
        private readonly PlayerUpdatedHandler<TestPlayerUpdatedEvent> _playerUpdatedHandler;

        public PlayerUpdatedHandlerTests()
        {
            _logRepository = new Mock<ILogRepository>();
            _playerUpdatedHandler = new PlayerUpdatedHandler<TestPlayerUpdatedEvent>(_logRepository.Object);
        }

        [Fact]
        public async Task Handle_Always_ShouldWriteLog()
        {
            var notification = new TestPlayerUpdatedEvent
            {
                CustID = 1,
                UserId = 2,
            };

            await _playerUpdatedHandler.Handle(notification, CancellationToken.None);

            _logRepository.Verify(x => x.Write(It.Is<LogData>(
                p => p.CustID == notification.CustID &&
                p.UserId == notification.UserId &&
                p.ActionID == notification.ActionID &&
                p.TypeId == notification.TypeId &&
                p.Doing == notification.Doing)), Times.Once);
        }
    }

    public class TestPlayerUpdatedEvent : PlayerUpdatedEvent
    {
        public override int ActionID => 1;

        public override string Doing => "Test Doing";
    }
}
