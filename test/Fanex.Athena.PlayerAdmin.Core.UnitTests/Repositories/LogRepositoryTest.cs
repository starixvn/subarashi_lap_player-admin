namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Repositories
{
    using System.Data.Common;
    using AutoFixture;
    using Core.Helpers;
    using Core.Repositories;
    using Data;
    using Microsoft.Extensions.Logging;
    using Models;
    using Moq;

    public class LogRepositoryTest
    {
        private readonly Fixture _fixture;
        private readonly LogRepository _logRepository;
        private readonly Mock<IObjectDb> _mockDatabase;
        private readonly Mock<IDbFactory> _mockDbFactory;
        private readonly Mock<ILogger<LogRepository>> _mockLogger;

        public LogRepositoryTest()
        {
            _mockDbFactory = new Mock<IDbFactory>();
            _mockLogger = new Mock<ILogger<LogRepository>>();
            _mockDatabase = new Mock<IObjectDb>();
            _fixture = new Fixture();

            _mockDbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), 120))
                .Returns(_mockDatabase.Object);

            _logRepository = new LogRepository(_mockDbFactory.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Write_WithValidLogData_ExecutesStoredProcedure()
        {
            // Arrange
            var logData = _fixture.Create<LogData>();

            // Act
            await _logRepository.Write(logData);

            // Assert
            _mockDatabase.Verify(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Write_CallsCorrectStoredProcedure()
        {
            // Arrange
            var logData = _fixture.Create<LogData>();
            const string expectedSpName = "WriteLog_UserLog_UserControl";

            // Act
            await _logRepository.Write(logData);

            // Assert
            _mockDbFactory.Verify(x => x.GetObjectDb(expectedSpName, 120), Times.Once);
        }

        [Fact]
        public async Task Write_PassesCorrectParameters()
        {
            // Arrange
            var logData = new LogData
            {
                UserId = 123,
                CustID = 456,
                ActionID = 789,
                Doing = "Test Action",
                Duration = 5,
                TypeId = 1
            };

            // Act
            await _logRepository.Write(logData);

            // Assert
            _mockDatabase.Verify(x => x.ExecuteNonQueryAsync(logData, It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Write_DisposesDatabase()
        {
            // Arrange
            var logData = _fixture.Create<LogData>();

            // Act
            await _logRepository.Write(logData);

            // Assert
            _mockDatabase.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public async Task Write_WhenDatabaseThrowsException_LogsError()
        {
            // Arrange
            var logData = _fixture.Create<LogData>();
            var expectedException = new InvalidOperationException("Database error");

            _mockDatabase.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(expectedException);

            // Act
            await _logRepository.Write(logData);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(ErrorMessageHelper.GenerateMessage(expectedException, null))),
                    expectedException,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Write_WhenDatabaseThrowsException_DoesNotRethrow()
        {
            // Arrange
            var logData = _fixture.Create<LogData>();
            var expectedException = new InvalidOperationException("Database error");

            _mockDatabase.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(expectedException);

            // Act & Assert - Should not throw
            await _logRepository.Write(logData);
        }

        [Fact]
        public async Task Write_WhenDatabaseThrowsException_StillDisposesDatabase()
        {
            // Arrange
            var logData = _fixture.Create<LogData>();
            var expectedException = new InvalidOperationException("Database error");

            _mockDatabase.Setup(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(expectedException);

            // Act
            await _logRepository.Write(logData);

            // Assert
            _mockDatabase.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public async Task Write_WithNullLogData_ExecutesStoredProcedure()
        {
            // Arrange
            LogData logData = null;

            // Act
            await _logRepository.Write(logData);

            // Assert
            _mockDatabase.Verify(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public void Constructor_WithValidParameters_InitializesCorrectly()
        {
            // Arrange & Act
            var repository = new LogRepository(_mockDbFactory.Object, _mockLogger.Object);

            // Assert
            Assert.NotNull(repository);
        }

        [Fact]
        public async Task Write_WithMultipleExceptions_LogsAllErrors()
        {
            // Arrange
            var logData = _fixture.Create<LogData>();
            var exception1 = new InvalidOperationException("First error");
            var exception2 = new ArgumentException("Second error");

            _mockDatabase.SetupSequence(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(exception1)
                .ThrowsAsync(exception2);

            // Act
            await _logRepository.Write(logData);
            await _logRepository.Write(logData);

            // Assert
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Exactly(2));
        }
    }
}