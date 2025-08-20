namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Middlewares
{
    using Core.Middlewares;
    using Microsoft.AspNetCore.Http;
    using Moq;
    using ONEbook.UM.Models;
    using ONEbook.UM.Services;

    public class UnderMaintenanceHandlerTests
    {
        private readonly Mock<IMaintenanceService> _maintenanceServiceMock;
        private readonly Mock<RequestDelegate> _nextDelegateMock;
        private readonly UnderMaintenanceHandler _handler;

        public UnderMaintenanceHandlerTests()
        {
            _maintenanceServiceMock = new Mock<IMaintenanceService>();
            _nextDelegateMock = new Mock<RequestDelegate>();
            _handler = new UnderMaintenanceHandler(_nextDelegateMock.Object, _maintenanceServiceMock.Object);
        }

        [Fact]
        public async Task Invoke_ShouldCallNextDelegate_WhenNotUnderMaintenance()
        {
            _maintenanceServiceMock.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(false);
            
            await _handler.Invoke(new DefaultHttpContext());
            
            _nextDelegateMock.Verify(x => x(It.IsAny<HttpContext>()), Times.Once);
        }

        [Fact]
        public async Task Invoke_ShouldReturnMaintenanceMessage_WhenUnderMaintenance()
        {
            _maintenanceServiceMock.Setup(x => x.IsUnderMaintenanceAsync()).ReturnsAsync(true);
            _maintenanceServiceMock.Setup(x => x.GetUnderMaintenanceTimeAsync()).ReturnsAsync(new UnderMaintenanceTime());
            
            var context = new DefaultHttpContext();
            
            await _handler.Invoke(context);
            
            Assert.Equal("text/html", context.Response.ContentType);
        }
    }
}
