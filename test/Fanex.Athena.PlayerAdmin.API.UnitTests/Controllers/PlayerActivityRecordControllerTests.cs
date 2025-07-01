using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class PlayerActivityRecordControllerTests
    {
        private readonly Mock<IPlayerActivityRecordService> _serviceMock;
        private readonly PlayerActivityRecordController _controller;

        public PlayerActivityRecordControllerTests()
        {
            _serviceMock = new Mock<IPlayerActivityRecordService>();
            _controller = new PlayerActivityRecordController(_serviceMock.Object);
        }

        [Fact]
        public async Task Index_WithInvalidCustId_ReturnsNull()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel { CustId = 0 };

            // Act
            var result = await _controller.Index(metaModel);

            // Assert
            Assert.Null(result);
            _serviceMock.Verify(x => x.GetActivityRecord(It.IsAny<PlayerActivityRecordMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Index_WithValidCustId_ReturnsPlayerActivityRecord()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel { CustId = 123, SiteName = "site" };
            var expectedRecord = new PlayerActivityRecord { BAUpdatedBy = "admin" };
            _serviceMock.Setup(x => x.GetActivityRecord(metaModel)).ReturnsAsync(expectedRecord);

            // Act
            var result = await _controller.Index(metaModel);

            // Assert
            var okResult = Assert.IsType<PlayerActivityRecord>(result.Value);
            Assert.Equal("admin", okResult.BAUpdatedBy);
            _serviceMock.Verify(x => x.GetActivityRecord(metaModel), Times.Once);
        }
    }
} 