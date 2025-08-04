namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Services
{
    using MetaModel;
    using Moq;
    using MVC.Repositories.Interfaces;
    using MVC.Services;
    using MVC.ViewModels;

    public class PlayerDetailTransactionServiceTest
    {
        private readonly Mock<IPlayerDetailTransactionRepository> _mockRepository;
        private readonly PlayerDetailTransactionService _service;

        public PlayerDetailTransactionServiceTest()
        {
            _mockRepository = new Mock<IPlayerDetailTransactionRepository>();
            _service = new PlayerDetailTransactionService(_mockRepository.Object);
        }

        [Fact]
        public void Constructor_WithValidRepository_CreatesService()
        {
            // Act & Assert
            Assert.NotNull(_service);
        }

        [Fact]
        public async Task GetDetailTransactionReport_WithValidCriteria_CallsRepositoryAndReturnsResult()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel
            {
                CustId = 123,
                CurrencyId = 456,
                FromDate = DateTime.Today.AddDays(-7),
                ToDate = DateTime.Today
            };
            var expectedResult = new PlayerDetailTransactionReportViewModel
            {
                CustId = 123,
                CurrencyId = 456
            };

            _mockRepository.Setup(x => x.GetDetailTransactions(criteria))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _service.GetDetailTransactionReport(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(123, result.CustId);
            Assert.Equal(456, result.CurrencyId);

            _mockRepository.Verify(x => x.GetDetailTransactions(criteria), Times.Once);
        }

        [Fact]
        public async Task GetDetailTransactionReport_WithNullCriteria_CallsRepositoryWithNull()
        {
            // Arrange
            PlayerDetailTransactionsMetaModel criteria = null;
            var expectedResult = new PlayerDetailTransactionReportViewModel();

            _mockRepository.Setup(x => x.GetDetailTransactions(null))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _service.GetDetailTransactionReport(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);

            _mockRepository.Verify(x => x.GetDetailTransactions(null), Times.Once);
        }

        [Fact]
        public async Task GetDetailTransactionReport_RepositoryThrowsException_PropagatesException()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel();
            _mockRepository.Setup(x => x.GetDetailTransactions(It.IsAny<PlayerDetailTransactionsMetaModel>()))
                .ThrowsAsync(new InvalidOperationException("Repository error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _service.GetDetailTransactionReport(criteria));
        }

        [Fact]
        public async Task GetDetailTransactionReport_RepositoryReturnsNull_ReturnsNull()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel();
            _mockRepository.Setup(x => x.GetDetailTransactions(It.IsAny<PlayerDetailTransactionsMetaModel>()))
                .ReturnsAsync((PlayerDetailTransactionReportViewModel)null);

            // Act
            var result = await _service.GetDetailTransactionReport(criteria);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.GetDetailTransactions(criteria), Times.Once);
        }

        [Fact]
        public async Task GetDetailTransactionReport_WithEmptyCriteria_CallsRepositoryCorrectly()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel();
            var expectedResult = new PlayerDetailTransactionReportViewModel();

            _mockRepository.Setup(x => x.GetDetailTransactions(criteria))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _service.GetDetailTransactionReport(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);

            _mockRepository.Verify(x => x.GetDetailTransactions(criteria), Times.Once);
        }

        [Fact]
        public async Task GetDetailTransactionReport_WithComplexCriteria_PassesAllPropertiesCorrectly()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel
            {
                CustId = 789,
                CurrencyId = 101,
                FromDate = new DateTime(2023, 1, 1),
                ToDate = new DateTime(2023, 12, 31),
                IsEnableSingleWallet = true
            };
            var expectedResult = new PlayerDetailTransactionReportViewModel
            {
                CustId = 789,
                CurrencyId = 101,
                IsEnableSingleWallet = true
            };

            _mockRepository.Setup(x => x.GetDetailTransactions(criteria))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _service.GetDetailTransactionReport(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult, result);
            Assert.Equal(789, result.CustId);
            Assert.Equal(101, result.CurrencyId);
            Assert.True(result.IsEnableSingleWallet);

            _mockRepository.Verify(x => x.GetDetailTransactions(criteria), Times.Once);
        }

        [Fact]
        public async Task GetDetailTransactionReport_CallsRepositoryOnlyOnce()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel();
            var expectedResult = new PlayerDetailTransactionReportViewModel();

            _mockRepository.Setup(x => x.GetDetailTransactions(criteria))
                .ReturnsAsync(expectedResult);

            // Act
            await _service.GetDetailTransactionReport(criteria);

            // Assert
            _mockRepository.Verify(x => x.GetDetailTransactions(criteria), Times.Once);
            _mockRepository.VerifyNoOtherCalls();
        }
    }
}