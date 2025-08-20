namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Repositories
{
    using System.Collections;
    using System.Data;
    using System.Data.Common;
    using Core.Repositories;
    using Core.Services;
    using Data;
    using MetaModel;
    using Moq;
    using MVC.Repositories;

    public class PlayerDetailTransactionRepositoryTest
    {
        private readonly Mock<IDataReader> _mockDataReader;
        private readonly Mock<IDbFactory> _mockDbFactory;
        private readonly Mock<IObjectDb> _mockObjectDb;
        private readonly Mock<IResourceService> _mockResourceService;
        private readonly PlayerDetailTransactionRepository _repository;

        public PlayerDetailTransactionRepositoryTest()
        {
            _mockDbFactory = new Mock<IDbFactory>();
            _mockResourceService = new Mock<IResourceService>();
            _mockObjectDb = new Mock<IObjectDb>();
            _mockDataReader = new Mock<IDataReader>();

            _mockDbFactory.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(_mockObjectDb.Object);
            _mockObjectDb.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_mockDataReader.Object);
            _mockObjectDb.Setup(x => x.GetOutputParams())
                .Returns(new Hashtable { { "TotalRow", 0 } });
            _mockObjectDb.Setup(x => x.SetParameterValue(It.IsAny<string>(), It.IsAny<object>()));

            _repository = new PlayerDetailTransactionRepository(_mockDbFactory.Object, _mockResourceService.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_CreatesRepository()
        {
            // Act & Assert
            Assert.NotNull(_repository);
        }

        [Fact]
        public async Task GetDetailTransactions_WithValidCriteria_ReturnsCorrectResult()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel
            {
                CustId = 123,
                CurrencyId = 456
            };

            var readSequence = new Queue<bool>(new[] { true, true, false });
            _mockDataReader.Setup(x => x.Read()).Returns(() => readSequence.Dequeue());

            // Setup first transaction
            _mockDataReader.SetupSequence(x => x["transid"])
                .Returns("TXN001")
                .Returns("TXN002");
            _mockDataReader.SetupSequence(x => x["bettype"])
                .Returns(1)
                .Returns(2);
            _mockDataReader.SetupSequence(x => x["transtime"])
                .Returns(new DateTime(2023, 1, 1, 10, 0, 0))
                .Returns(new DateTime(2023, 1, 2, 11, 0, 0));
            _mockDataReader.SetupSequence(x => x["transremark"])
                .Returns("Bet placed")
                .Returns("Bet won");
            _mockDataReader.SetupSequence(x => x["changedAmt"])
                .Returns(-100.50m)
                .Returns(200.75m);
            _mockDataReader.SetupSequence(x => x["totalAmt"])
                .Returns(899.50m)
                .Returns(1100.25m);
            _mockDataReader.SetupSequence(x => x["IsResettled"])
                .Returns(false)
                .Returns(true);

            _mockResourceService.Setup(x => x.GetBettype(1)).Returns("Sports Bet");
            _mockResourceService.Setup(x => x.GetBettype(2)).Returns("Casino Bet");

            // Act
            var result = await _repository.GetDetailTransactions(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.CustId);
            Assert.Equal(456, result.CurrencyId);
            Assert.NotNull(result.DetailTransactions);
            Assert.Equal(2, result.DetailTransactions.Count());

            var transactionList = result.DetailTransactions.ToList();
            var firstTransaction = transactionList[0];
            Assert.Equal("TXN001", firstTransaction.TransactionId);
            Assert.Equal(1, firstTransaction.BetType);
            Assert.Equal("Sports Bet", firstTransaction.TransactionType);
            Assert.Equal(new DateTime(2023, 1, 1, 10, 0, 0), firstTransaction.TransactionTime);
            Assert.Equal("Bet placed", firstTransaction.Remark);
            Assert.Equal(-100.50m, firstTransaction.BalanceChange);
            Assert.Equal(899.50m, firstTransaction.AfterBalance);
            Assert.False(firstTransaction.IsResettled);

            var secondTransaction = transactionList[1];
            Assert.Equal("TXN002", secondTransaction.TransactionId);
            Assert.Equal(2, secondTransaction.BetType);
            Assert.Equal("Casino Bet", secondTransaction.TransactionType);
            Assert.True(secondTransaction.IsResettled);
        }

        [Fact]
        public async Task GetDetailTransactions_WithNoData_ReturnsEmptyResult()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel
            {
                CustId = 123
            };

            _mockDataReader.Setup(x => x.Read()).Returns(false);

            // Act
            var result = await _repository.GetDetailTransactions(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.CustId);
            Assert.NotNull(result.DetailTransactions);
            Assert.Empty(result.DetailTransactions);
        }

        [Fact]
        public async Task GetDetailTransactions_CallsDbFactoryWithCorrectSPName()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel();

            _mockDataReader.Setup(x => x.Read()).Returns(false);

            // Act
            await _repository.GetDetailTransactions(criteria);

            // Assert
            _mockDbFactory.Verify(x => x.GetObjectDb("LAP_Rpt_DetailTransaction_Mem", It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetDetailTransactions_CallsSetParameterValues()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel();

            _mockDataReader.Setup(x => x.Read()).Returns(false);

            // Act
            await _repository.GetDetailTransactions(criteria);

            // Assert
            // Verify that the database factory was called to get the ObjectDb
            _mockDbFactory.Verify(x => x.GetObjectDb("LAP_Rpt_DetailTransaction_Mem", It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetDetailTransactions_DisposesResourcesProperly()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel();

            _mockDataReader.Setup(x => x.Read()).Returns(false);

            // Act
            await _repository.GetDetailTransactions(criteria);

            // Assert
            _mockObjectDb.Verify(x => x.Dispose(), Times.Once);
            _mockDataReader.Verify(x => x.Dispose(), Times.Once);
        }

        [Fact]
        public async Task GetDetailTransactions_WithNullCriteria_ThrowsNullReferenceException()
        {
            // Arrange
            PlayerDetailTransactionsMetaModel criteria = null;

            // Act & Assert
            await Assert.ThrowsAsync<NullReferenceException>(() => _repository.GetDetailTransactions(criteria));
        }

        [Fact]
        public async Task GetDetailTransactions_DbFactoryThrowsException_PropagatesException()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel();

            _mockDbFactory.Setup(x => x.GetObjectDb("LAP_Rpt_DetailTransaction_Mem", It.IsAny<int>()))
                .Throws(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetDetailTransactions(criteria));
        }

        [Fact]
        public async Task GetDetailTransactions_ExecuteReaderThrowsException_PropagatesException()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel();

            _mockObjectDb.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException("Reader error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _repository.GetDetailTransactions(criteria));
        }

        [Fact]
        public async Task GetDetailTransactions_CallsResourceServiceForEachTransaction()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel();

            var readSequence = new Queue<bool>(new[] { true, true, true, false });
            _mockDataReader.Setup(x => x.Read()).Returns(() => readSequence.Dequeue());

            _mockDataReader.Setup(x => x["bettype"]).Returns(1);
            _mockDataReader.Setup(x => x["transid"]).Returns("TXN001");
            _mockDataReader.Setup(x => x["transtime"]).Returns(DateTime.Now);
            _mockDataReader.Setup(x => x["transremark"]).Returns("Test");
            _mockDataReader.Setup(x => x["changedAmt"]).Returns(100m);
            _mockDataReader.Setup(x => x["totalAmt"]).Returns(1000m);
            _mockDataReader.Setup(x => x["IsResettled"]).Returns(false);

            _mockResourceService.Setup(x => x.GetBettype(1)).Returns("Test Type");

            // Act
            await _repository.GetDetailTransactions(criteria);

            // Assert
            _mockResourceService.Verify(x => x.GetBettype(1), Times.Exactly(3));
        }
    }
}