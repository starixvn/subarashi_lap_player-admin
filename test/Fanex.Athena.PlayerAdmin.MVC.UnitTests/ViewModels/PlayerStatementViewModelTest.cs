namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.ViewModels
{
    using MVC.ViewModels;

    public class PlayerStatementViewModelTest
    {
        [Fact]
        public void DefaultConstructor_CreatesViewModel()
        {
            // Act
            var viewModel = new PlayerStatementViewModel();

            // Assert
            Assert.NotNull(viewModel);
        }

        [Fact]
        public void CustId_CanBeSetAndRetrieved()
        {
            // Arrange
            var viewModel = new PlayerStatementViewModel();
            const int expectedCustId = 12345;

            // Act
            viewModel.CustId = expectedCustId;

            // Assert
            Assert.Equal(expectedCustId, viewModel.CustId);
        }

        [Fact]
        public void CurrencyId_CanBeSetAndRetrieved()
        {
            // Arrange
            var viewModel = new PlayerStatementViewModel();
            const int expectedCurrencyId = 67890;

            // Act
            viewModel.CurrencyId = expectedCurrencyId;

            // Assert
            Assert.Equal(expectedCurrencyId, viewModel.CurrencyId);
        }

        [Fact]
        public void FromDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var viewModel = new PlayerStatementViewModel();
            var expectedFromDate = new DateTime(2023, 1, 1);

            // Act
            viewModel.FromDate = expectedFromDate;

            // Assert
            Assert.Equal(expectedFromDate, viewModel.FromDate);
        }

        [Fact]
        public void ToDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var viewModel = new PlayerStatementViewModel();
            var expectedToDate = new DateTime(2023, 12, 31);

            // Act
            viewModel.ToDate = expectedToDate;

            // Assert
            Assert.Equal(expectedToDate, viewModel.ToDate);
        }

        [Fact]
        public void MinDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var viewModel = new PlayerStatementViewModel();
            var expectedMinDate = new DateTime(2020, 1, 1);

            // Act
            viewModel.MinDate = expectedMinDate;

            // Assert
            Assert.Equal(expectedMinDate, viewModel.MinDate);
        }

        [Fact]
        public void MaxDate_CanBeSetAndRetrieved()
        {
            // Arrange
            var viewModel = new PlayerStatementViewModel();
            var expectedMaxDate = new DateTime(2030, 12, 31);

            // Act
            viewModel.MaxDate = expectedMaxDate;

            // Assert
            Assert.Equal(expectedMaxDate, viewModel.MaxDate);
        }

        [Fact]
        public void IsShowSabaCoins_CanBeSetAndRetrieved()
        {
            // Arrange
            var viewModel = new PlayerStatementViewModel();

            // Act & Assert - Test true
            viewModel.IsShowSabaCoins = true;
            Assert.True(viewModel.IsShowSabaCoins);

            // Act & Assert - Test false
            viewModel.IsShowSabaCoins = false;
            Assert.False(viewModel.IsShowSabaCoins);
        }

        [Fact]
        public void AllProperties_CanBeSetSimultaneously()
        {
            // Arrange
            var viewModel = new PlayerStatementViewModel();
            const int expectedCustId = 111;
            const int expectedCurrencyId = 222;
            var expectedFromDate = new DateTime(2023, 6, 1);
            var expectedToDate = new DateTime(2023, 6, 30);
            var expectedMinDate = new DateTime(2021, 1, 1);
            var expectedMaxDate = new DateTime(2025, 12, 31);
            const bool expectedIsShowSabaCoins = true;

            // Act
            viewModel.CustId = expectedCustId;
            viewModel.CurrencyId = expectedCurrencyId;
            viewModel.FromDate = expectedFromDate;
            viewModel.ToDate = expectedToDate;
            viewModel.MinDate = expectedMinDate;
            viewModel.MaxDate = expectedMaxDate;
            viewModel.IsShowSabaCoins = expectedIsShowSabaCoins;

            // Assert
            Assert.Equal(expectedCustId, viewModel.CustId);
            Assert.Equal(expectedCurrencyId, viewModel.CurrencyId);
            Assert.Equal(expectedFromDate, viewModel.FromDate);
            Assert.Equal(expectedToDate, viewModel.ToDate);
            Assert.Equal(expectedMinDate, viewModel.MinDate);
            Assert.Equal(expectedMaxDate, viewModel.MaxDate);
            Assert.Equal(expectedIsShowSabaCoins, viewModel.IsShowSabaCoins);
        }

        [Fact]
        public void DefaultValues_AreCorrect()
        {
            // Act
            var viewModel = new PlayerStatementViewModel();

            // Assert
            Assert.Equal(0, viewModel.CustId);
            Assert.Equal(0, viewModel.CurrencyId);
            Assert.Equal(default, viewModel.FromDate);
            Assert.Equal(default, viewModel.ToDate);
            Assert.Equal(default, viewModel.MinDate);
            Assert.Equal(default, viewModel.MaxDate);
            Assert.False(viewModel.IsShowSabaCoins);
        }

        [Fact]
        public void NegativeValues_CanBeSet()
        {
            // Arrange
            var viewModel = new PlayerStatementViewModel();

            // Act
            viewModel.CustId = -1;
            viewModel.CurrencyId = -100;

            // Assert
            Assert.Equal(-1, viewModel.CustId);
            Assert.Equal(-100, viewModel.CurrencyId);
        }

        [Fact]
        public void DateTimeMinMaxValues_CanBeSet()
        {
            // Arrange
            var viewModel = new PlayerStatementViewModel();

            // Act
            viewModel.FromDate = DateTime.MinValue;
            viewModel.ToDate = DateTime.MaxValue;
            viewModel.MinDate = DateTime.MinValue;
            viewModel.MaxDate = DateTime.MaxValue;

            // Assert
            Assert.Equal(DateTime.MinValue, viewModel.FromDate);
            Assert.Equal(DateTime.MaxValue, viewModel.ToDate);
            Assert.Equal(DateTime.MinValue, viewModel.MinDate);
            Assert.Equal(DateTime.MaxValue, viewModel.MaxDate);
        }

        [Fact]
        public void Properties_AreIndependent()
        {
            // Arrange
            var viewModel = new PlayerStatementViewModel();

            // Act
            viewModel.CustId = 100;
            viewModel.CurrencyId = 200;

            // Assert - Changing one property doesn't affect others
            Assert.Equal(100, viewModel.CustId);
            Assert.Equal(200, viewModel.CurrencyId);
            Assert.Equal(0, viewModel.FromDate.Ticks); // Default DateTime
            Assert.False(viewModel.IsShowSabaCoins); // Default bool
        }
    }
}