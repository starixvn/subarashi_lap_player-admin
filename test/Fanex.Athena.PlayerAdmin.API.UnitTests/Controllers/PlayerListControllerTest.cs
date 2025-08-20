namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    using API.Controllers;
    using API.DTO;
    using API.MetaModel;
    using API.Services;
    using API.Services.Interfaces;
    using API.ViewModels;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Moq;

    public class PlayerListControllerTest
    {
        private readonly PlayerListController _controller;
        private readonly Mock<IContextService> _mockContextService;
        private readonly Mock<IDepositSettingService> _mockDepositSettingService;
        private readonly Mock<IPlayerListService> _mockPlayerListService;

        public PlayerListControllerTest()
        {
            _mockPlayerListService = new Mock<IPlayerListService>();
            _mockDepositSettingService = new Mock<IDepositSettingService>();
            _mockContextService = new Mock<IContextService>();

            // Setup default mocks for validation to pass
            _mockDepositSettingService.Setup(x => x.GetCommGroupList(It.IsAny<int>()))
                .ReturnsAsync(new List<CommGroupViewModel>());
            _mockDepositSettingService.Setup(x => x.GetRiskLevelList(It.IsAny<string>()))
                .ReturnsAsync(new List<RiskLevel>());
            _mockContextService.Setup(x => x.LicenseeId).Returns(123);
            _mockContextService.Setup(x => x.Language).Returns("en");
            _mockContextService.Setup(x => x.OfferedOddsGroupList).Returns(new List<string>());

            _controller = new PlayerListController(_mockPlayerListService.Object, _mockDepositSettingService.Object, _mockContextService.Object);
        }

        [Fact]
        public void Constructor_WithValidService_CreatesController()
        {
            // Act & Assert
            Assert.NotNull(_controller);
        }

        [Fact]
        public async Task Search_WithValidMetaModel_ReturnsPlayerListResult()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel
            {
                UserName = "testuser",
                PageIndex = 1,
                PageSize = 10,
                SiteName = "TestSite",
                SpecialLevel = "1,2",
                CommGroups = "",
                CustomerClass = "",
                OddsGroup = ""
            };

            var expectedResult = new PlayerListResultViewModel
            {
                DataList = new List<Player>
                {
                    new() { IbcCustId = 1, UserName = "testuser1", Currency = "USD" },
                    new() { IbcCustId = 2, UserName = "testuser2", Currency = "EUR" }
                },
                TotalItem = 2
            };

            _mockPlayerListService.Setup(x => x.Search(metaModel))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Search(metaModel);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            Assert.Equal(expectedResult, actionResult.Value);
            _mockPlayerListService.Verify(x => x.Search(metaModel), Times.Once);
        }

        [Fact]
        public async Task Search_WithEmptyResult_ReturnsEmptyPlayerListResult()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel
            {
                UserName = "nonexistent",
                PageIndex = 1,
                PageSize = 10,
                SiteName = "TestSite",
                SpecialLevel = "1,2",
                CommGroups = "",
                CustomerClass = "",
                OddsGroup = ""
            };

            var expectedResult = new PlayerListResultViewModel
            {
                DataList = new List<Player>(),
                TotalItem = 0
            };

            _mockPlayerListService.Setup(x => x.Search(metaModel))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Search(metaModel);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            Assert.Equal(expectedResult, actionResult.Value);
            Assert.Empty(actionResult.Value.DataList);
            Assert.Equal(0, actionResult.Value.TotalItem);
        }

        [Fact]
        public async Task GetPlayerListById_WithValidMetaModel_ReturnsPlayerListResult()
        {
            // Arrange
            var metaModel = new PlayerListByIdMetaModel
            {
                IdList = "123",
                SiteName = "TestSite",
                SpecialLevel = "1,2",
                CommGroups = "",
                CustomerClass = "",
                OddsGroup = ""
            };

            var expectedResult = new PlayerListResultViewModel
            {
                DataList = new List<Player>
                {
                    new() { IbcCustId = 123, UserName = "specificuser", Currency = "USD" }
                },
                TotalItem = 1
            };

            _mockPlayerListService.Setup(x => x.GetPlayerListById(metaModel))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetPlayerListById(metaModel);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            Assert.Equal(expectedResult, actionResult.Value);
            _mockPlayerListService.Verify(x => x.GetPlayerListById(metaModel), Times.Once);
        }

        [Fact]
        public async Task ArchivedSearch_WithValidMetaModel_ReturnsPlayerListResult()
        {
            // Arrange
            var metaModel = new PlayerArchivedSearchingMetaModel
            {
                UserName = "archiveduser",
                PageIndex = 1,
                PageSize = 10
            };

            var expectedResult = new PlayerListResultViewModel
            {
                DataList = new List<Player>
                {
                    new() { IbcCustId = 999, UserName = "archiveduser", Currency = "GBP" }
                },
                TotalItem = 1
            };

            _mockPlayerListService.Setup(x => x.ArchivedSearch(metaModel))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ArchivedSearch(metaModel);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            Assert.Equal(expectedResult, actionResult.Value);
            _mockPlayerListService.Verify(x => x.ArchivedSearch(metaModel), Times.Once);
        }

        [Fact]
        public async Task Search_WithServiceThrowingException_ThrowsException()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel
            {
                UserName = "testuser",
                PageIndex = 1,
                PageSize = 10,
                SiteName = "TestSite",
                SpecialLevel = "1,2",
                CommGroups = "",
                CustomerClass = "",
                OddsGroup = ""
            };

            _mockPlayerListService.Setup(x => x.Search(metaModel))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.Search(metaModel));
            _mockPlayerListService.Verify(x => x.Search(metaModel), Times.Once);
        }

        [Fact]
        public async Task GetPlayerListById_WithServiceThrowingException_ThrowsException()
        {
            // Arrange
            var metaModel = new PlayerListByIdMetaModel
            {
                IdList = "123",
                SiteName = "TestSite",
                SpecialLevel = "1,2",
                CommGroups = "",
                CustomerClass = "",
                OddsGroup = ""
            };

            _mockPlayerListService.Setup(x => x.GetPlayerListById(metaModel))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.GetPlayerListById(metaModel));
            _mockPlayerListService.Verify(x => x.GetPlayerListById(metaModel), Times.Once);
        }

        [Fact]
        public async Task ArchivedSearch_WithServiceThrowingException_ThrowsException()
        {
            // Arrange
            var metaModel = new PlayerArchivedSearchingMetaModel
            {
                UserName = "archiveduser",
                PageIndex = 1,
                PageSize = 10
            };

            _mockPlayerListService.Setup(x => x.ArchivedSearch(metaModel))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _controller.ArchivedSearch(metaModel));
            _mockPlayerListService.Verify(x => x.ArchivedSearch(metaModel), Times.Once);
        }

        [Fact]
        public async Task Search_WithLargePageSize_ReturnsCorrectResult()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel
            {
                UserName = "testuser",
                PageIndex = 1,
                PageSize = 100,
                SiteName = "TestSite",
                SpecialLevel = "1,2",
                CommGroups = "",
                CustomerClass = "",
                OddsGroup = ""
            };

            var players = new List<Player>();
            for (int i = 1; i <= 50; i++)
            {
                players.Add(new Player { IbcCustId = i, UserName = $"testuser{i}", Currency = "USD" });
            }

            var expectedResult = new PlayerListResultViewModel
            {
                DataList = players,
                TotalItem = 50
            };

            _mockPlayerListService.Setup(x => x.Search(metaModel))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Search(metaModel);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            Assert.Equal(50, actionResult.Value.TotalItem);
            Assert.Equal(50, ((List<Player>)actionResult.Value.DataList).Count);
        }

        [Fact]
        public async Task Search_WithNullResult_ReturnsNull()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel
            {
                UserName = "testuser",
                PageIndex = 1,
                PageSize = 10,
                SiteName = "TestSite"
            };

            _mockPlayerListService.Setup(x => x.Search(metaModel))
                .ReturnsAsync((PlayerListResultViewModel)null);

            // Act
            var result = await _controller.Search(metaModel);

            // Assert
            Assert.NotNull(result);
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            Assert.Null(actionResult.Value);
        }
    }
}