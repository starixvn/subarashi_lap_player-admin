namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Services
{
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using Commission.Criteria;
    using Commission.MetaModels;
    using Commission.Repositories.Interfaces;
    using Commission.Services;
    using Commission.ViewModels;
    using Core.Models;
    using Moq;

    public class PlayerCommissionServiceTests
    {
        private readonly Fixture _fixture;
        private readonly Mock<ProductAccess> _productAccessMock;
        private readonly Mock<IPlayerCommissionRepository> _repositoryMock;
        private readonly PlayerCommissionService _sut;

        public PlayerCommissionServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
            _repositoryMock = new Mock<IPlayerCommissionRepository>();
            _productAccessMock = new Mock<ProductAccess>(new List<Product>());
            _sut = new PlayerCommissionService(_repositoryMock.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeRepository_WhenCalled()
        {
            // Arrange
            var repository = new Mock<IPlayerCommissionRepository>();

            // Act
            var service = new PlayerCommissionService(repository.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public async Task InitCommValues_ShouldReturnFilteredAndOrderedCommissionSettings_WhenCalled()
        {
            // Arrange
            var commissionSettings = new List<PlayerCommissionSettingViewModel>
            {
                new() { ProductCode = "SB" },
                new() { ProductCode = "NG" },
                new() { ProductCode = "InvalidProduct" }
            };

            _repositoryMock.Setup(x => x.GetAllCommSettingAsync(It.IsAny<ProductAccess>())).ReturnsAsync(commissionSettings);
            var realProductAccess = new ProductAccess(new List<Product> { new() { ProductCode = "SB" }, new() { ProductCode = "NG" } });

            // Act
            var result = await _sut.InitCommValues(realProductAccess);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Equal("SB", result.First().ProductCode);
            Assert.Equal("NG", result.Last().ProductCode);
        }

        [Fact]
        public async Task GetAllCommGroups_ShouldReturnMergedCommissionGroups_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<BasePlayerCommissionUpdateQuery>();
            var commissionGroups = new List<PlayerCommissionViewModel>
            {
                new() { GroupCommId = 1, GroupName = "Group 1" },
                new() { GroupCommId = 2, GroupName = "Group 2" }
            };
            var commissionValues = new List<PlayerCommissionSettingViewModel>
            {
                new() { GroupCommId = 1, ProductCode = "SB" },
                new() { GroupCommId = 2, ProductCode = "NG" }
            };

            _repositoryMock.Setup(x => x.GetAllCommGroups(It.IsAny<GetAllCommGroupsCriteria>(), It.IsAny<ProductAccess>()))
                .ReturnsAsync((commissionGroups, commissionValues));
            var realProductAccess2 = new ProductAccess(new List<Product> { new() { ProductCode = "SB" }, new() { ProductCode = "NG" } });

            // Act
            var result = await _sut.GetAllCommGroups(query, realProductAccess2);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task MultipleMoveGroup_ShouldReturnErrorResult_WhenGroupNotExists()
        {
            // Arrange
            var query = _fixture.Create<MultipleMoveGroupQuery>();
            // Return null commValues to simulate group not existing
            _repositoryMock.Setup(x => x.GetCommissionGroupById(It.IsAny<GetCommissionGroupByIdCriteria>(), It.IsAny<ProductAccess>()))
                .ReturnsAsync((new PlayerCommissionViewModel(), null));

            // Act
            var result = await _sut.MultipleMoveGroup(query, _productAccessMock.Object);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Code);
            Assert.False(string.IsNullOrEmpty(result.Message));
        }

        [Fact]
        public async Task MultipleMoveGroup_ShouldCallRepository_WhenGroupExists()
        {
            // Arrange
            var query = _fixture.Create<MultipleMoveGroupQuery>();
            var commission = new PlayerCommissionViewModel();
            var commissionValues = new List<PlayerCommissionSettingViewModel>
            {
                new() { ProductCode = "SB" }
            };
            var expectedResult = new ResultInfo();

            _repositoryMock.Setup(x => x.GetCommissionGroupById(It.IsAny<GetCommissionGroupByIdCriteria>(), It.IsAny<ProductAccess>()))
                .ReturnsAsync((commission, commissionValues));
            _repositoryMock.Setup(x => x.MultipleMoveGroup(It.IsAny<MultipleMoveGroupCriteria>()))
                .ReturnsAsync(expectedResult);
            var realProductAccess3 = new ProductAccess(new List<Product> { new() { ProductCode = "SB" } });

            // Act
            var result = await _sut.MultipleMoveGroup(query, realProductAccess3);

            // Assert
            Assert.Equal(expectedResult, result);
            _repositoryMock.Verify(x => x.MultipleMoveGroup(It.IsAny<MultipleMoveGroupCriteria>()), Times.Once);
        }

        [Fact]
        public async Task MultipleRemoveFromGroup_ShouldCallRepository_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleRemoveFromGroupQuery>();
            var expectedResult = new ResultInfo();

            _repositoryMock.Setup(x => x.MultipleRemoveFromGroup(It.IsAny<MultipleRemoveFromGroupCriteria>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _sut.MultipleRemoveFromGroup(query);

            // Assert
            Assert.Equal(expectedResult, result);
            _repositoryMock.Verify(x => x.MultipleRemoveFromGroup(It.IsAny<MultipleRemoveFromGroupCriteria>()), Times.Once);
        }

        [Fact]
        public async Task MultipleUpdateGroup_ShouldCallRepository_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleUpdateGroupQuery>();
            var expectedResult = new ResultInfo();

            _repositoryMock.Setup(x => x.MultipleUpdateGroup(It.IsAny<MultipleUpdateGroupCriteria>()))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _sut.MultipleUpdateGroup(query);

            // Assert
            Assert.Equal(expectedResult, result);
            _repositoryMock.Verify(x => x.MultipleUpdateGroup(It.IsAny<MultipleUpdateGroupCriteria>()), Times.Once);
        }

        [Fact]
        public async Task GetPlayerCommissionGroup_ShouldReturnMergedCommissionGroup_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<SingleMoveGroupQuery>();
            var commission = new PlayerCommissionViewModel { GroupCommId = 123 };
            var commissionValues = new List<PlayerCommissionSettingViewModel>
            {
                new() { ProductCode = "SB" }
            };

            _repositoryMock.Setup(x => x.GetPlayerCommissionGroup(It.IsAny<GetPlayerCommissionGroupCriteria>(), It.IsAny<ProductAccess>()))
                .ReturnsAsync((commission, commissionValues));
            var realProductAccess4 = new ProductAccess(new List<Product> { new() { ProductCode = "SB" } });

            // Act
            var result = await _sut.GetPlayerCommissionGroup(query, realProductAccess4);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(123, result.GroupCommId);
            Assert.NotNull(result.CommValues);
        }

        [Fact]
        public async Task InitCommValues_ShouldFilterByProductPermissions_WhenCalled()
        {
            // Arrange
            var commissionSettings = new List<PlayerCommissionSettingViewModel>
            {
                new() { ProductCode = "SB" },
                new() { ProductCode = "NG" },
                new() { ProductCode = "Parlay" }
            };

            _repositoryMock.Setup(x => x.GetAllCommSettingAsync(It.IsAny<ProductAccess>())).ReturnsAsync(commissionSettings);
            var realProductAccess5 = new ProductAccess(new List<Product> { new() { ProductCode = "SB" } });

            // Act
            var result = await _sut.InitCommValues(realProductAccess5);

            // Assert
            Assert.NotNull(result);
            //Assert.Equal(2, result.Count()); // SB + Parlay (which follows SB permission)
            Assert.Equal(1, result.Count()); // SB
        }

        [Fact]
        public async Task GetAllCommGroups_ShouldCreateCorrectCriteria_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<BasePlayerCommissionUpdateQuery>();
            _repositoryMock.Setup(x => x.GetAllCommGroups(It.IsAny<GetAllCommGroupsCriteria>(), It.IsAny<ProductAccess>()))
                .ReturnsAsync((new List<PlayerCommissionViewModel>(), new List<PlayerCommissionSettingViewModel>()));

            // Act
            await _sut.GetAllCommGroups(query, _productAccessMock.Object);

            // Assert
            _repositoryMock.Verify(x => x.GetAllCommGroups(
                It.Is<GetAllCommGroupsCriteria>(c => c.LicenseeId == query.LicenseeId),
                _productAccessMock.Object), Times.Once);
        }

        [Fact]
        public async Task GetPlayerCommissionGroup_ShouldCreateCorrectCriteria_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<SingleMoveGroupQuery>();
            _repositoryMock.Setup(x => x.GetPlayerCommissionGroup(It.IsAny<GetPlayerCommissionGroupCriteria>(), It.IsAny<ProductAccess>()))
                .ReturnsAsync((new PlayerCommissionViewModel(), new List<PlayerCommissionSettingViewModel>()));

            // Act
            await _sut.GetPlayerCommissionGroup(query, _productAccessMock.Object);

            // Assert
            _repositoryMock.Verify(x => x.GetPlayerCommissionGroup(
                It.Is<GetPlayerCommissionGroupCriteria>(c => c.CustId == query.CustId),
                _productAccessMock.Object), Times.Once);
        }
    }
}