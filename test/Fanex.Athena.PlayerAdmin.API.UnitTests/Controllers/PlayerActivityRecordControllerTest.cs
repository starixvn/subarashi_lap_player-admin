namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    using API.Controllers;
    using API.DTO;
    using API.MetaModel;
    using API.Services.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using Xunit;

    public class PlayerActivityRecordControllerTest
    {
        private readonly PlayerActivityRecordController _controller;
        private readonly Mock<IPlayerActivityRecordService> _mockService;

        public PlayerActivityRecordControllerTest()
        {
            _mockService = new Mock<IPlayerActivityRecordService>();
            _controller = new PlayerActivityRecordController(_mockService.Object);
        }

        [Fact]
        public void Constructor_WithValidService_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public async Task Index_WithValidCustId_ReturnsPlayerActivityRecord()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 123,
                CurrencyId = 1,
                SiteName = "TestSite"
            };

            var expectedRecord = new PlayerActivityRecord
            {
                CreateDate = DateTime.Now,
                LastLogged = DateTime.Now,
                BAUpdatedBy = "TestUser",
                SpecialLevelUpdateBy = "AdminUser"
            };

            _mockService.Setup(x => x.GetActivityRecord(metaModel))
                .ReturnsAsync(expectedRecord);

            // Act
            var result = await _controller.Index(metaModel);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<PlayerActivityRecord>>(result);
            Assert.Equal(expectedRecord, actionResult.Value);
            _mockService.Verify(x => x.GetActivityRecord(metaModel), Times.Once);
        }

        [Fact]
        public async Task Index_WithZeroCustId_ReturnsNull()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 0,
                CurrencyId = 1,
                SiteName = "TestSite"
            };

            // Act
            var result = await _controller.Index(metaModel);

            // Assert
            Assert.Null(result);
            _mockService.Verify(x => x.GetActivityRecord(It.IsAny<PlayerActivityRecordMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Index_WithNegativeCustId_ReturnsNull()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = -1,
                CurrencyId = 1,
                SiteName = "TestSite"
            };

            // Act
            var result = await _controller.Index(metaModel);

            // Assert
            Assert.Null(result);
            _mockService.Verify(x => x.GetActivityRecord(It.IsAny<PlayerActivityRecordMetaModel>()), Times.Never);
        }

        [Fact]
        public async Task Index_WithServiceReturningNull_ReturnsNull()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 123,
                CurrencyId = 1,
                SiteName = "TestSite"
            };

            _mockService.Setup(x => x.GetActivityRecord(metaModel))
                .ReturnsAsync((PlayerActivityRecord)null);

            // Act
            var result = await _controller.Index(metaModel);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<PlayerActivityRecord>>(result);
            Assert.Null(actionResult.Value);
            _mockService.Verify(x => x.GetActivityRecord(metaModel), Times.Once);
        }

        [Fact]
        public async Task Index_WithServiceThrowingException_ThrowsException()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 123,
                CurrencyId = 1,
                SiteName = "TestSite"
            };

            _mockService.Setup(x => x.GetActivityRecord(metaModel))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Index(metaModel));
            _mockService.Verify(x => x.GetActivityRecord(metaModel), Times.Once);
        }

        [Fact]
        public async Task Index_WithDifferentMetaModelValues_CallsServiceWithCorrectParameters()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 456,
                CurrencyId = 2,
                SiteName = "AnotherSite"
            };

            var expectedRecord = new PlayerActivityRecord
            {
                CreateDate = new DateTime(2023, 1, 1),
                BAUpdatedBy = "AnotherUser"
            };

            _mockService.Setup(x => x.GetActivityRecord(metaModel))
                .ReturnsAsync(expectedRecord);

            // Act
            var result = await _controller.Index(metaModel);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<PlayerActivityRecord>>(result);
            Assert.Equal(expectedRecord, actionResult.Value);
            _mockService.Verify(
                x => x.GetActivityRecord(It.Is<PlayerActivityRecordMetaModel>(m => m.CustId == 456 && m.CurrencyId == 2 && m.SiteName == "AnotherSite")),
                Times.Once);
        }

        [Fact]
        public async Task Index_WithCompletePlayerActivityRecord_ReturnsAllProperties()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 789,
                CurrencyId = 3,
                SiteName = "CompleteSite"
            };

            var expectedRecord = new PlayerActivityRecord
            {
                CreateDate = new DateTime(2022, 1, 1),
                LastLogged = new DateTime(2023, 1, 1, 12, 0, 0),
                LastTransferDate = new DateTime(2023, 1, 2),
                LastUpdateBA = new DateTime(2023, 1, 3),
                BAUpdatedBy = "CompleteUser",
                SpecialLevelUpdateBy = "AdminUser",
                SpecialLevelUpdateDate = new DateTime(2023, 1, 4)
            };

            _mockService.Setup(x => x.GetActivityRecord(metaModel))
                .ReturnsAsync(expectedRecord);

            // Act
            var result = await _controller.Index(metaModel);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<PlayerActivityRecord>>(result);
            var actualRecord = actionResult.Value;


            Assert.Equal(expectedRecord.CreateDate, actualRecord.CreateDate);
            Assert.Equal(expectedRecord.LastLogged, actualRecord.LastLogged);
            Assert.Equal(expectedRecord.LastTransferDate, actualRecord.LastTransferDate);
            Assert.Equal(expectedRecord.LastUpdateBA, actualRecord.LastUpdateBA);
            Assert.Equal(expectedRecord.BAUpdatedBy, actualRecord.BAUpdatedBy);
            Assert.Equal(expectedRecord.SpecialLevelUpdateBy, actualRecord.SpecialLevelUpdateBy);
            Assert.Equal(expectedRecord.SpecialLevelUpdateDate, actualRecord.SpecialLevelUpdateDate);
        }
    }
}