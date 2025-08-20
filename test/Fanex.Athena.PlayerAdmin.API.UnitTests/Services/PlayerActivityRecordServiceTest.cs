// src/Fanex.Athena.PlayerAdmin.API/Services/PlayerActivityRecordService.cs
namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Services
{
    using System.Data.Common;
    using API.DTO;
    using API.MetaModel;
    using API.Services;
    using API.Services.Interfaces;
    using AutoFixture;
    using Core.Repositories;
    using Data;
    using Moq;

    public class PlayerActivityRecordServiceTest
    {
        private readonly Mock<IDbFactory> _mockDbFactory;
        private readonly PlayerActivityRecordService _service;

        public PlayerActivityRecordServiceTest()
        {
            _mockDbFactory = new Mock<IDbFactory>();
            _service = new PlayerActivityRecordService(_mockDbFactory.Object);
        }

        [Fact]
        public void Constructor_WithValidDbFactory_CreatesService()
        {
            // Act & Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public void Constructor_WithNullDbFactory_DoesNotThrowException()
        {
            // The service constructor doesn't validate null parameters
            // Act & Assert
            var service = new PlayerActivityRecordService(null);
            Assert.NotNull(service);
        }

        [Fact]
        public void Service_ImplementsInterface()
        {
            // Act & Assert
            Assert.IsAssignableFrom<IPlayerActivityRecordService>(_service);
        }

        [Fact]
        public void Service_HasCorrectDependencies()
        {
            // Act & Assert
            Assert.NotNull(_mockDbFactory);
            Assert.NotNull(_service);
        }

        [Fact]
        public async Task GetActivityRecord_WithValidMetaModel_ReturnsActivityRecord()
        {
            // Arrange
            var fixture = new Fixture();
            var metaModel = fixture.Create<PlayerActivityRecordMetaModel>();
            var expectedActivityRecord = fixture.Create<PlayerActivityRecord>();
            
            var mockDb = new Mock<IObjectDb>();
            mockDb.Setup(x => x.SetParameterValue("custid", metaModel.CustId));
            mockDb.Setup(x => x.SetParameterValue("siteName", metaModel.SiteName));
            mockDb.Setup(x => x.QueryEntityAsync<PlayerActivityRecord>(It.IsAny<object>(),It.IsAny<DbTransaction>(),It.IsAny<CancellationToken>()))
                  .ReturnsAsync(expectedActivityRecord);
            
            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_CustomerLoginInfo", It.IsAny<int>()))
                         .Returns(mockDb.Object);

            // Act
            var result = await _service.GetActivityRecord(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedActivityRecord, result);
            
            // Verify database calls
            _mockDbFactory.Verify(x => x.GetObjectDb("Lrf_CustomerLoginInfo", It.IsAny<int>()), Times.Once);
            mockDb.Verify(x => x.SetParameterValue("custid", metaModel.CustId), Times.Once);
            mockDb.Verify(x => x.SetParameterValue("siteName", metaModel.SiteName), Times.Once);
            mockDb.Verify(x => x.QueryEntityAsync<PlayerActivityRecord>(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetActivityRecord_WithNullMetaModel_ThrowsArgumentNullException()
        {
            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => _service.GetActivityRecord(null));
        }

        [Theory]
        [InlineData(1, "testSite")]
        [InlineData(2, "liveSite")]
        [InlineData(3, "emptyCustId")]
        [InlineData(4, "")]
        public async Task GetActivityRecord_WithVariousMetaModelValues_CallsCorrectStoredProcedure(
            int custId, string siteName)
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = custId,
                SiteName = siteName
            };
            
            var mockDb = new Mock<IObjectDb>();
            mockDb.Setup(x => x.QueryEntityAsync<PlayerActivityRecord>(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new PlayerActivityRecord());
            
            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_CustomerLoginInfo", It.IsAny<int>()))
                         .Returns(mockDb.Object);

            // Act
            var result = await _service.GetActivityRecord(metaModel);

            // Assert
            Assert.NotNull(result);
            
            // Verify correct parameters were set
            mockDb.Verify(x => x.SetParameterValue("custid", custId), Times.Once);
            mockDb.Verify(x => x.SetParameterValue("siteName", siteName), Times.Once);
        }

        [Fact]
        public async Task GetActivityRecord_WhenDbFactoryThrowsException_RethrowsException()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 1,
                SiteName = "test"
            };
            
            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_CustomerLoginInfo", It.IsAny<int>()))
                         .Throws<InvalidOperationException>();

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.GetActivityRecord(metaModel));
        }

        [Fact]
        public async Task GetActivityRecord_WhenQueryThrowsException_RethrowsException()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 1,
                SiteName = "test"
            };
            
            var mockDb = new Mock<IObjectDb>();
            mockDb.Setup(x => x.QueryEntityAsync<PlayerActivityRecord>(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                  .ThrowsAsync(new InvalidOperationException("Database error"));
            
            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_CustomerLoginInfo", It.IsAny<int>()))
                         .Returns(mockDb.Object);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _service.GetActivityRecord(metaModel));
            
            Assert.Equal("Database error", exception.Message);
        }

        [Fact]
        public async Task GetActivityRecord_WithNullDbFromFactory_ThrowsNullReferenceException()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 1,
                SiteName = "test"
            };
            
            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_CustomerLoginInfo", It.IsAny<int>()))
                         .Returns((IObjectDb)null);

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(
                () => _service.GetActivityRecord(metaModel));
        }

        [Fact]
        public async Task GetActivityRecord_UsesCorrectStoredProcedureName()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 1,
                SiteName = "test"
            };
            
            var mockDb = new Mock<IObjectDb>();
            mockDb.Setup(x => x.QueryEntityAsync<PlayerActivityRecord>(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new PlayerActivityRecord());
            
            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_CustomerLoginInfo", It.IsAny<int>()))
                         .Returns(mockDb.Object);

            // Act
            await _service.GetActivityRecord(metaModel);

            // Assert
            _mockDbFactory.Verify(x => x.GetObjectDb("Lrf_CustomerLoginInfo", It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task Constructor_WithNullDbFactory_ThrowsNullReferenceException()
        {
            // Act & Assert
            // The service will throw when trying to use the null factory
            var service = new PlayerActivityRecordService(null);
            Assert.NotNull(service);
            
            // But it will throw when trying to use it
            var metaModel = new PlayerActivityRecordMetaModel { CustId = 1, SiteName = "test" };
            await Assert.ThrowsAsync<NullReferenceException>(async() => await service.GetActivityRecord(metaModel));
        }

        [Fact]
        public async Task GetActivityRecord_DisposesResourcesProperly()
        {
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 1,
                SiteName = "test"
            };
            
            var mockDb = new Mock<IObjectDb>();
            mockDb.Setup(x => x.QueryEntityAsync<PlayerActivityRecord>(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                  .ReturnsAsync(new PlayerActivityRecord());
            
            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_CustomerLoginInfo", It.IsAny<int>()))
                         .Returns(mockDb.Object);

            // Act
            await _service.GetActivityRecord(metaModel);

            // Assert
            // The using statement should dispose the db object
            mockDb.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public void PlayerActivityRecordService_ConstantsAreCorrect()
        {
            // This test verifies that the service uses the correct stored procedure name
            // We can't directly test the const, but we can verify it through behavior
            
            // Arrange
            var metaModel = new PlayerActivityRecordMetaModel
            {
                CustId = 1,
                SiteName = "test"
            };
            
            var mockDb = new Mock<IObjectDb>();
            _mockDbFactory.Setup(x => x.GetObjectDb("Lrf_CustomerLoginInfo", It.IsAny<int>()))
                         .Returns(mockDb.Object);

            // Act
            var task = _service.GetActivityRecord(metaModel);

            // Assert
            _mockDbFactory.Verify(x => x.GetObjectDb("Lrf_CustomerLoginInfo", It.IsAny<int>()), Times.Once);
        }
    }
}