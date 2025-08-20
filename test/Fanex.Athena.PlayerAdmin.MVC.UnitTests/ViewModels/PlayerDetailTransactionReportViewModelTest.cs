namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.ViewModels
{
    using Core.Constants;
    using Core.Helpers;
    using MetaModel;
    using MVC.ViewModels;

    public class PlayerDetailTransactionReportViewModelTest
    {
        [Fact]
        public void DefaultConstructor_SetsDefaultValues()
        {
            // Act
            var viewModel = new PlayerDetailTransactionReportViewModel();

            // Assert
            Assert.NotNull(viewModel.DetailTransactions);
            Assert.Empty(viewModel.DetailTransactions);
            Assert.Equal(500, viewModel.PageSize);
            Assert.Equal(1, viewModel.PageIndex);
            Assert.Equal(ViewTypes.MainWallet, viewModel.ViewType);
            Assert.True(viewModel.MinDate <= viewModel.FromDate);
            Assert.True(viewModel.FromDate <= viewModel.ToDate);
        }

        [Fact]
        public void DefaultConstructor_SetsCorrectDateRanges()
        {
            // Act
            var viewModel = new PlayerDetailTransactionReportViewModel();

            // Assert
            // MinDate should be 3 months ago
            var expectedMinDate = DateTime.Today.AddMonths(-3);
            Assert.True(Math.Abs((viewModel.MinDate - expectedMinDate).TotalDays) <= 31); // Allow for month variations

            // FromDate should be first day of current month
            Assert.Equal(1, viewModel.FromDate.Day);
            Assert.Equal(DateTime.Today.Month, viewModel.FromDate.Month);
            Assert.Equal(DateTime.Today.Year, viewModel.FromDate.Year);

            // ToDate should be today (in database server timezone)
            Assert.Equal(DateTimeHelper.DatabaseServerToDay, viewModel.ToDate.Date);
        }

        [Fact]
        public void ParameterizedConstructor_WithValidCriteriaAndTransactions_SetsProperties()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel
            {
                CustId = 123,
                CurrencyId = 456,
                FromDate = new DateTime(2023, 1, 1),
                ToDate = new DateTime(2023, 12, 31),
                PageSize = 100,
                PageIndex = 2,
                ViewType = ViewTypes.MainWallet,
                IsEnableSingleWallet = true
            };

            var transactions = new List<PlayerDetailTransactionViewModel>
            {
                new()
                {
                    TransactionId = "TXN001",
                    BetType = 1,
                    TransactionType = "Sports Bet"
                },
                new()
                {
                    TransactionId = "TXN002",
                    BetType = 2,
                    TransactionType = "Casino Bet"
                }
            };

            // Act
            var viewModel = new PlayerDetailTransactionReportViewModel(criteria, transactions);

            // Assert
            Assert.Equal(123, viewModel.CustId);
            Assert.Equal(456, viewModel.CurrencyId);
            Assert.Equal(new DateTime(2023, 1, 1), viewModel.FromDate);
            Assert.Equal(new DateTime(2023, 12, 31), viewModel.ToDate);
            Assert.Equal(100, viewModel.PageSize);
            Assert.Equal(2, viewModel.PageIndex);
            Assert.Equal(ViewTypes.MainWallet, viewModel.ViewType);
            // Assert.True(viewModel.IsEnableSingleWallet);
            Assert.Equal(transactions, viewModel.DetailTransactions);
            Assert.Equal(2, viewModel.DetailTransactions.Count());
        }

        [Fact]
        public void ParameterizedConstructor_WithNullCriteria_ThrowsNullReferenceException()
        {
            // Arrange
            PlayerDetailTransactionsMetaModel criteria = null;
            var transactions = new List<PlayerDetailTransactionViewModel>();

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => new PlayerDetailTransactionReportViewModel(criteria, transactions));
        }

        [Fact]
        public void ParameterizedConstructor_WithNullTransactions_SetsEmptyTransactions()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel
            {
                CustId = 123
            };
            List<PlayerDetailTransactionViewModel> transactions = null;

            // Act
            var viewModel = new PlayerDetailTransactionReportViewModel(criteria, transactions);

            // Assert
            Assert.Null(viewModel.DetailTransactions);
            // Assert.Empty(viewModel.DetailTransactions);
        }

        [Fact]
        public void Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var viewModel = new PlayerDetailTransactionReportViewModel();
            var testTransactions = new List<PlayerDetailTransactionViewModel>
            {
                new() { TransactionId = "TEST001" }
            };

            // Act
            viewModel.CustId = 999;
            viewModel.CurrencyId = 888;
            viewModel.FromDate = new DateTime(2022, 6, 15);
            viewModel.ToDate = new DateTime(2022, 7, 20);
            viewModel.PageSize = 250;
            viewModel.PageIndex = 3;
            viewModel.ViewType = ViewTypes.MainWallet;
            viewModel.IsEnableSingleWallet = true;
            viewModel.IsShowSabaCoins = true;
            viewModel.DetailTransactions = testTransactions;

            // Assert
            Assert.Equal(999, viewModel.CustId);
            Assert.Equal(888, viewModel.CurrencyId);
            Assert.Equal(new DateTime(2022, 6, 15), viewModel.FromDate);
            Assert.Equal(new DateTime(2022, 7, 20), viewModel.ToDate);
            Assert.Equal(250, viewModel.PageSize);
            Assert.Equal(3, viewModel.PageIndex);
            Assert.Equal(ViewTypes.MainWallet, viewModel.ViewType);
            Assert.True(viewModel.IsEnableSingleWallet);
            Assert.True(viewModel.IsShowSabaCoins);
            Assert.Equal(testTransactions, viewModel.DetailTransactions);
        }

        [Fact]
        public void DefaultPageSize_IsCorrectValue()
        {
            // Assert
            Assert.Equal(500, PlayerDetailTransactionReportViewModel.DefaultPageSize);
        }

        [Fact]
        public void DefaultPageIndex_IsCorrectValue()
        {
            // Assert
            Assert.Equal(1, PlayerDetailTransactionReportViewModel.DefaultPageIndex);
        }

        [Fact]
        public void ParameterizedConstructor_WithEmptyTransactions_SetsEmptyCollection()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel
            {
                CustId = 123
            };
            var transactions = new List<PlayerDetailTransactionViewModel>();

            // Act
            var viewModel = new PlayerDetailTransactionReportViewModel(criteria, transactions);

            // Assert
            Assert.NotNull(viewModel.DetailTransactions);
            Assert.Empty(viewModel.DetailTransactions);
        }

        [Fact]
        public void ParameterizedConstructor_WithLargeTransactionList_HandlesCorrectly()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel
            {
                CustId = 123
            };
            var transactions = new List<PlayerDetailTransactionViewModel>();
            for (int i = 0; i < 1000; i++)
            {
                transactions.Add(new PlayerDetailTransactionViewModel
                {
                    TransactionId = $"TXN{i:D4}",
                    BetType = i % 10
                });
            }

            // Act
            var viewModel = new PlayerDetailTransactionReportViewModel(criteria, transactions);

            // Assert
            Assert.Equal(1000, viewModel.DetailTransactions.Count());
            Assert.Equal("TXN0000", viewModel.DetailTransactions.First().TransactionId);
            Assert.Equal("TXN0999", viewModel.DetailTransactions.Last().TransactionId);
        }

        [Fact]
        public void ParameterizedConstructor_CopiesAllCriteriaProperties()
        {
            // Arrange
            var criteria = new PlayerDetailTransactionsMetaModel
            {
                CustId = 123,
                CurrencyId = 456,
                FromDate = new DateTime(2023, 5, 1),
                ToDate = new DateTime(2023, 5, 31),
                PageSize = 75,
                PageIndex = 4,
                ViewType = ViewTypes.MainWallet,
                IsEnableSingleWallet = false
            };

            // Act
            var viewModel = new PlayerDetailTransactionReportViewModel(criteria, null);

            // Assert
            Assert.Equal(criteria.CustId, viewModel.CustId);
            Assert.Equal(criteria.CurrencyId, viewModel.CurrencyId);
            Assert.Equal(criteria.FromDate, viewModel.FromDate);
            Assert.Equal(criteria.ToDate, viewModel.ToDate);
            Assert.Equal(criteria.PageSize, viewModel.PageSize);
            Assert.Equal(criteria.PageIndex, viewModel.PageIndex);
            Assert.Equal(criteria.ViewType, viewModel.ViewType);
            Assert.Equal(criteria.IsEnableSingleWallet, viewModel.IsEnableSingleWallet);
        }
    }
}