namespace Fanex.Athena.PlayerAdmin.MVC.UnitTests.Controllers
{
    using Core.Helpers;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;
    using MVC.Controllers;
    using MVC.Services.Interfaces;
    using MVC.ViewModels;

    public class PlayerStatementControllerTest
    {
        private readonly PlayerStatementController _controller;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IPlayerStatementService> _mockService;

        public PlayerStatementControllerTest()
        {
            _mockService = new Mock<IPlayerStatementService>();
            _mockContextService = new Mock<IContextService>();
            _controller = new PlayerStatementController(_mockService.Object, _mockContextService.Object);
        }

        [Fact]
        public void Constructor_WithValidDependencies_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public void Index_WithValidParameters_ReturnsViewWithCorrectModel()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 456;
            _mockContextService.Setup(x => x.IsShowSabaCoins).Returns(true);

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerStatementViewModel>(viewResult.Model);

            Assert.Equal(custId, model.CustId);
            Assert.Equal(currencyId, model.CurrencyId);
            Assert.True(model.IsShowSabaCoins);
            Assert.NotNull(model.FromDate);
            Assert.NotNull(model.ToDate);
            Assert.NotNull(model.MinDate);
            Assert.NotNull(model.MaxDate);
        }

        [Fact]
        public void Index_WithDifferentContextSettings_ReturnsViewWithCorrectSettings()
        {
            // Arrange
            const int custId = 789;
            const int currencyId = 101;
            _mockContextService.Setup(x => x.IsShowSabaCoins).Returns(false);

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerStatementViewModel>(viewResult.Model);

            Assert.Equal(custId, model.CustId);
            Assert.Equal(currencyId, model.CurrencyId);
            Assert.False(model.IsShowSabaCoins);
        }

        [Fact]
        public void Index_ChecksDateRanges_SetsCorrectDateBounds()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 456;
            _mockContextService.Setup(x => x.IsShowSabaCoins).Returns(true);

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerStatementViewModel>(viewResult.Model);

            // Check that date ranges are properly set
            Assert.True(model.MinDate < model.FromDate);
            Assert.True(model.FromDate <= model.ToDate);
            Assert.True(model.ToDate < model.MaxDate);

            // Check year ranges (MinYearRange = 2, MaxYearRange = 5)
            var today = DateTime.Today;
            Assert.Equal(today.Year - 2, model.MinDate.Year);
            Assert.Equal(today.Year + 5, model.MaxDate.Year);
        }

        [Fact]
        public void Index_WithZeroCustId_ReturnsViewWithZeroCustId()
        {
            // Arrange
            const int custId = 0;
            const int currencyId = 456;

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerStatementViewModel>(viewResult.Model);

            Assert.Equal(0, model.CustId);
            Assert.Equal(currencyId, model.CurrencyId);
        }

        [Fact]
        public void Index_WithNegativeCustId_ReturnsViewWithNegativeCustId()
        {
            // Arrange
            const int custId = -1;
            const int currencyId = 456;

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerStatementViewModel>(viewResult.Model);

            Assert.Equal(-1, model.CustId);
            Assert.Equal(currencyId, model.CurrencyId);
        }

        [Fact]
        public void Index_ChecksFromDateIsFirstDayOfMonth()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 456;

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerStatementViewModel>(viewResult.Model);

            // FromDate should be the first day of current month
            Assert.Equal(1, model.FromDate.Day);
            Assert.Equal(DateTime.Today.Month, model.FromDate.Month);
            Assert.Equal(DateTime.Today.Year, model.FromDate.Year);
        }

        [Fact]
        public void Index_ChecksToDateIsToday()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 456;

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerStatementViewModel>(viewResult.Model);

            // ToDate should be today (in database server timezone)
            Assert.Equal(DateTimeHelper.DatabaseServerToDay, model.ToDate.Date);
        }

        [Fact]
        public void Index_ChecksMinDateIsJanuaryFirstTwoYearsAgo()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 456;

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerStatementViewModel>(viewResult.Model);

            // MinDate should be January 1st, 2 years ago
            var expectedMinDate = new DateTime(DateTime.Today.Year - 2, 1, 1);
            Assert.Equal(expectedMinDate, model.MinDate);
        }

        [Fact]
        public void Index_ChecksMaxDateIsDecember31FiveYearsFromNow()
        {
            // Arrange
            const int custId = 123;
            const int currencyId = 456;

            // Act
            var result = _controller.Index(custId, currencyId);

            // Assert
            Assert.NotNull(result);
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<PlayerStatementViewModel>(viewResult.Model);

            // MaxDate should be December 31st, 5 years from now
            var expectedMaxDate = new DateTime(DateTime.Today.Year + 5, 12, 31);
            Assert.Equal(expectedMaxDate, model.MaxDate);
        }
    }
}