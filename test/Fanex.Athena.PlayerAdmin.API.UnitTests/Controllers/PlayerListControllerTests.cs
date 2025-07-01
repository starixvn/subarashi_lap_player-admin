using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fanex.Athena.PlayerAdmin.API.Controllers;
using Fanex.Athena.PlayerAdmin.API.DTO;
using Fanex.Athena.PlayerAdmin.API.MetaModel;
using Fanex.Athena.PlayerAdmin.API.Services.Interfaces;
using Fanex.Athena.PlayerAdmin.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.Controllers
{
    public class PlayerListControllerTests
    {
        private readonly Mock<IPlayerListService> _serviceMock;
        private readonly PlayerListController _controller;

        public PlayerListControllerTests()
        {
            _serviceMock = new Mock<IPlayerListService>();
            _controller = new PlayerListController(_serviceMock.Object);
        }

        [Fact]
        public async Task Search_WithValidMetaModel_ReturnsPlayerListResult()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel
            {
                UserName = "testuser",
                SearchOption = 1,
                PageSize = 10,
                PageIndex = 1
            };

            var expectedResult = new PlayerListResultViewModel
            {
                DataList = new List<Player>
                {
                    new Player
                    {
                        IbcCustId = 123,
                        UserName = "testuser",
                        SystemId = "testuser",
                        Currency = "USD",
                        CurrentBalance = 1000.00m,
                        OutstandingBalance = 0.00m,
                        SpecialLevel = 1,
                        OddsGroup = "A",
                        CommissionGroup = "Standard",
                        CustomerClass = "VIP",
                        RiskLevelKey = "VIP",
                        RiskLevel = "VIP"
                    }
                },
                TotalItem = 1
            };

            _serviceMock.Setup(x => x.Search(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Search(metaModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            var returnedResult = actionResult.Value;

            Assert.Equal(expectedResult.TotalItem, returnedResult.TotalItem);
            Assert.Single(returnedResult.DataList);
            var expectedPlayerList = expectedResult.DataList.ToList();
            var returnedPlayerList = returnedResult.DataList.ToList();
            Assert.Equal(expectedPlayerList[0].IbcCustId, returnedPlayerList[0].IbcCustId);
            Assert.Equal(expectedPlayerList[0].UserName, returnedPlayerList[0].UserName);

            _serviceMock.Verify(x => x.Search(metaModel), Times.Once);
        }

        [Fact]
        public async Task Search_WithEmptyResult_ReturnsEmptyPlayerList()
        {
            // Arrange
            var metaModel = new PlayerSearchingMetaModel
            {
                UserName = "nonexistent",
                PageSize = 10,
                PageIndex = 1
            };

            var emptyResult = new PlayerListResultViewModel
            {
                DataList = new List<Player>(),
                TotalItem = 0
            };

            _serviceMock.Setup(x => x.Search(metaModel)).ReturnsAsync(emptyResult);

            // Act
            var result = await _controller.Search(metaModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            var returnedResult = actionResult.Value;

            Assert.Equal(0, returnedResult.TotalItem);
            Assert.Empty(returnedResult.DataList);

            _serviceMock.Verify(x => x.Search(metaModel), Times.Once);
        }

        [Fact]
        public async Task GetPlayerListById_WithValidMetaModel_ReturnsPlayerListResult()
        {
            // Arrange
            var metaModel = new PlayerListByIdMetaModel
            {
                IdList = "123,456",
                OddsGroup = "A",
                SpecialLevel = "1",
                PageSize = 10,
                PageIndex = 1
            };

            var expectedResult = new PlayerListResultViewModel
            {
                DataList = new List<Player>
                {
                    new Player
                    {
                        IbcCustId = 123,
                        UserName = "user1",
                        SystemId = "user1",
                        Currency = "USD",
                        CurrentBalance = 500.00m,
                        OutstandingBalance = 0.00m,
                        SpecialLevel = 1,
                        OddsGroup = "A",
                        CommissionGroup = "Standard",
                        CustomerClass = "Regular",
                        RiskLevelKey = "Regular",
                        RiskLevel = "Regular"
                    },
                    new Player
                    {
                        IbcCustId = 456,
                        UserName = "user2",
                        SystemId = "user2",
                        Currency = "EUR",
                        CurrentBalance = 750.00m,
                        OutstandingBalance = 0.00m,
                        SpecialLevel = 2,
                        OddsGroup = "A",
                        CommissionGroup = "Premium",
                        CustomerClass = "VIP",
                        RiskLevelKey = "VIP",
                        RiskLevel = "VIP"
                    }
                },
                TotalItem = 2
            };

            _serviceMock.Setup(x => x.GetPlayerListById(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetPlayerListById(metaModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            var returnedResult = actionResult.Value;

            Assert.Equal(expectedResult.TotalItem, returnedResult.TotalItem);
            Assert.Equal(2, returnedResult.DataList.Count());
            var expectedPlayerList = expectedResult.DataList.ToList();
            var returnedPlayerList = returnedResult.DataList.ToList();
            Assert.Equal(expectedPlayerList[0].IbcCustId, returnedPlayerList[0].IbcCustId);
            Assert.Equal(expectedPlayerList[1].IbcCustId, returnedPlayerList[1].IbcCustId);

            _serviceMock.Verify(x => x.GetPlayerListById(metaModel), Times.Once);
        }

        [Fact]
        public async Task GetPlayerListById_WithEmptyIdList_ReturnsEmptyResult()
        {
            // Arrange
            var metaModel = new PlayerListByIdMetaModel
            {
                IdList = "",
                PageSize = 10,
                PageIndex = 1
            };

            var emptyResult = new PlayerListResultViewModel
            {
                DataList = new List<Player>(),
                TotalItem = 0
            };

            _serviceMock.Setup(x => x.GetPlayerListById(metaModel)).ReturnsAsync(emptyResult);

            // Act
            var result = await _controller.GetPlayerListById(metaModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            var returnedResult = actionResult.Value;

            Assert.Equal(0, returnedResult.TotalItem);
            Assert.Empty(returnedResult.DataList);

            _serviceMock.Verify(x => x.GetPlayerListById(metaModel), Times.Once);
        }

        [Fact]
        public async Task ArchivedSearch_WithValidMetaModel_ReturnsArchivedPlayerList()
        {
            // Arrange
            var metaModel = new PlayerArchivedSearchingMetaModel
            {
                UserName = "archiveduser",
                PageSize = 10,
                PageIndex = 1
            };

            var expectedResult = new PlayerListResultViewModel
            {
                DataList = new List<Player>
                {
                    new Player
                    {
                        IbcCustId = 789,
                        UserName = "archiveduser",
                        SystemId = "archiveduser",
                        Currency = "USD",
                        CurrentBalance = 0.00m,
                        OutstandingBalance = 0.00m,
                        SpecialLevel = 0,
                        OddsGroup = "Z",
                        CommissionGroup = "Archived",
                        CustomerClass = "Archived",
                        RiskLevelKey = "Archived",
                        RiskLevel = "Archived"
                    }
                },
                TotalItem = 1
            };

            _serviceMock.Setup(x => x.ArchivedSearch(metaModel)).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ArchivedSearch(metaModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            var returnedResult = actionResult.Value;

            Assert.Equal(expectedResult.TotalItem, returnedResult.TotalItem);
            Assert.Single(returnedResult.DataList);
            var expectedPlayerList = expectedResult.DataList.ToList();
            var returnedPlayerList = returnedResult.DataList.ToList();
            Assert.Equal(expectedPlayerList[0].IbcCustId, returnedPlayerList[0].IbcCustId);
            Assert.Equal(expectedPlayerList[0].UserName, returnedPlayerList[0].UserName);

            _serviceMock.Verify(x => x.ArchivedSearch(metaModel), Times.Once);
        }

        [Fact]
        public async Task ArchivedSearch_WithNoArchivedPlayers_ReturnsEmptyResult()
        {
            // Arrange
            var metaModel = new PlayerArchivedSearchingMetaModel
            {
                UserName = "activeuser",
                PageSize = 10,
                PageIndex = 1
            };

            var emptyResult = new PlayerListResultViewModel
            {
                DataList = new List<Player>(),
                TotalItem = 0
            };

            _serviceMock.Setup(x => x.ArchivedSearch(metaModel)).ReturnsAsync(emptyResult);

            // Act
            var result = await _controller.ArchivedSearch(metaModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            var returnedResult = actionResult.Value;

            Assert.Equal(0, returnedResult.TotalItem);
            Assert.Empty(returnedResult.DataList);

            _serviceMock.Verify(x => x.ArchivedSearch(metaModel), Times.Once);
        }

        [Fact]
        public async Task Search_WithNullMetaModel_ReturnsServiceResult()
        {
            // Arrange
            PlayerSearchingMetaModel? metaModel = null;
            var expectedResult = new PlayerListResultViewModel();

            _serviceMock.Setup(x => x.Search(It.IsAny<PlayerSearchingMetaModel>())).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.Search(metaModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            var returnedResult = actionResult.Value;

            Assert.IsType<PlayerListResultViewModel>(returnedResult);

            _serviceMock.Verify(x => x.Search(It.IsAny<PlayerSearchingMetaModel>()), Times.Once);
        }

        [Fact]
        public async Task GetPlayerListById_WithNullMetaModel_ReturnsServiceResult()
        {
            // Arrange
            PlayerListByIdMetaModel? metaModel = null;
            var expectedResult = new PlayerListResultViewModel();

            _serviceMock.Setup(x => x.GetPlayerListById(It.IsAny<PlayerListByIdMetaModel>())).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.GetPlayerListById(metaModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            var returnedResult = actionResult.Value;

            Assert.IsType<PlayerListResultViewModel>(returnedResult);

            _serviceMock.Verify(x => x.GetPlayerListById(It.IsAny<PlayerListByIdMetaModel>()), Times.Once);
        }

        [Fact]
        public async Task ArchivedSearch_WithNullMetaModel_ReturnsServiceResult()
        {
            // Arrange
            PlayerArchivedSearchingMetaModel? metaModel = null;
            var expectedResult = new PlayerListResultViewModel();

            _serviceMock.Setup(x => x.ArchivedSearch(It.IsAny<PlayerArchivedSearchingMetaModel>())).ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.ArchivedSearch(metaModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<PlayerListResultViewModel>>(result);
            var returnedResult = actionResult.Value;

            Assert.IsType<PlayerListResultViewModel>(returnedResult);

            _serviceMock.Verify(x => x.ArchivedSearch(It.IsAny<PlayerArchivedSearchingMetaModel>()), Times.Once);
        }
    }
} 