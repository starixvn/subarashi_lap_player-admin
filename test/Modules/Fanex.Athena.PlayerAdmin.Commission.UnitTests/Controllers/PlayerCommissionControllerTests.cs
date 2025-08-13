namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Controllers
{
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using Commission.Controllers;
    using Commission.MetaModels;
    using Commission.Services.Interfaces;
    using Commission.ViewModels;
    using Core.Models;
    using Core.Services;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Memory;
    using Moq;

    public class PlayerCommissionControllerTests
    {
        private readonly Mock<IMemoryCache> _cacheMock;
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly Fixture _fixture;
        private readonly Mock<ILicenseeService> _licenseeServiceMock;
        private readonly Mock<IPlayerCommissionService> _playerCommServiceMock;
        private readonly Mock<ProductAccess> _productAccessMock;
        private readonly PlayerCommissionController _sut;

        public PlayerCommissionControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
            _contextServiceMock = new Mock<IContextService>();
            _licenseeServiceMock = new Mock<ILicenseeService>();
            _playerCommServiceMock = new Mock<IPlayerCommissionService>();
            _cacheMock = new Mock<IMemoryCache>();
            _productAccessMock = new Mock<ProductAccess>(new List<Product>());


            _contextServiceMock.Setup(x => x.LicenseeId).Returns(_fixture.Create<int>());
            _licenseeServiceMock.Setup(x => x.GetProductAccess(It.IsAny<int>(), "Default")).ReturnsAsync(_productAccessMock.Object);
            _licenseeServiceMock.Setup(x => x.GetProductAccess(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(_productAccessMock.Object);


            _sut = new PlayerCommissionController(_contextServiceMock.Object, _licenseeServiceMock.Object, _playerCommServiceMock.Object, _cacheMock.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeAllDependencies_WhenCalled()
        {
            // Arrange
            var contextServiceMock = new Mock<IContextService>();
            var licenseeServiceMock = new Mock<ILicenseeService>();
            var playerCommServiceMock = new Mock<IPlayerCommissionService>();
            var cacheMock = new Mock<IMemoryCache>();

            // Act
            var controller =
                new PlayerCommissionController(contextServiceMock.Object, licenseeServiceMock.Object, playerCommServiceMock.Object, cacheMock.Object);

            // Assert
            Assert.NotNull(controller);
        }

        [Fact]
        public async Task PostMultipleMoveGroup_ShouldReturnJsonResult_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleMoveGroupQuery>();
            var expectedResult = new ResultInfo();


            _playerCommServiceMock.Setup(x => x.MultipleMoveGroup(query, _productAccessMock.Object)).ReturnsAsync(expectedResult);

            // Act
            var result = await _sut.PostMultipleMoveGroup(query);

            // Assert
            Assert.IsType<JsonResult>(result);
            Assert.Equal(expectedResult, ((JsonResult)result).Value);


            _licenseeServiceMock.Verify(x => x.GetProductAccess(_contextServiceMock.Object.LicenseeId, "Default"), Times.Once);
            _playerCommServiceMock.Verify(x => x.MultipleMoveGroup(query, _productAccessMock.Object), Times.Once);
        }

        [Fact]
        public async Task PostMultipleUpdateGroup_ShouldReturnJsonResult_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleUpdateGroupQuery>();
            var expectedResult = new ResultInfo();


            _playerCommServiceMock.Setup(x => x.MultipleUpdateGroup(query)).ReturnsAsync(expectedResult);

            // Act
            var result = await _sut.PostMultipleUpdateGroup(query);

            // Assert
            Assert.IsType<JsonResult>(result);
            Assert.Equal(expectedResult, ((JsonResult)result).Value);


            _playerCommServiceMock.Verify(x => x.MultipleUpdateGroup(query), Times.Once);
        }

        [Fact]
        public async Task PostMultipleRemoveFromGroup_ShouldReturnJsonResult_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleRemoveFromGroupQuery>();
            var expectedResult = new ResultInfo();


            _playerCommServiceMock.Setup(x => x.MultipleRemoveFromGroup(query)).ReturnsAsync(expectedResult);

            // Act
            var result = await _sut.PostMultipleRemoveFromGroup(query);

            // Assert
            Assert.IsType<JsonResult>(result);
            Assert.Equal(expectedResult, ((JsonResult)result).Value);


            _playerCommServiceMock.Verify(x => x.MultipleRemoveFromGroup(query), Times.Once);
        }

        [Fact]
        public async Task MultipleMoveGroup_ShouldReturnViewWithModel_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleMoveGroupQuery>();
            var groups = _fixture.Create<IEnumerable<PlayerCommissionViewModel>>();


            _playerCommServiceMock.Setup(x => x.GetAllCommGroups(query, _productAccessMock.Object)).ReturnsAsync(groups);

            // Act
            var result = await _sut.MultipleMoveGroup(query);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<MultipleMoveGroupViewModel>(result.Model);


            var viewModel = result.Model as MultipleMoveGroupViewModel;
            Assert.Equal(groups, viewModel.Groups);


            _licenseeServiceMock.Verify(x => x.GetProductAccess(_contextServiceMock.Object.LicenseeId, "CommissionGroups"), Times.Once);
            _playerCommServiceMock.Verify(x => x.GetAllCommGroups(query, _productAccessMock.Object), Times.Once);
        }

        [Fact]
        public void MultipleRemoveFromGroup_ShouldReturnViewWithModel_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleRemoveFromGroupQuery>();

            // Act
            var result = _sut.MultipleRemoveFromGroup(query);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<MultipleRemoveFromGroupViewModel>(result.Model);
        }

        [Fact]
        public async Task MultipleUpdateGroup_ShouldReturnViewWithModel_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleUpdateGroupQuery>();
            var commValues = _fixture.Create<IEnumerable<PlayerCommissionSettingViewModel>>();
            var cacheKey = $"CommSettings_{query.LicenseeId}";

            // Setup cache to return false on TryGetValue (cache miss)
            _cacheMock.Setup(x => x.TryGetValue(cacheKey, out It.Ref<object>.IsAny)).Returns(false);

            // Setup cache.CreateEntry to support Set extension method
            _cacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            _playerCommServiceMock.Setup(x => x.InitCommValues(_productAccessMock.Object)).ReturnsAsync(commValues);

            // Act
            var result = await _sut.MultipleUpdateGroup(query);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<MultipleUpdateGroupViewModel>(result.Model);

            var viewModel = result.Model as MultipleUpdateGroupViewModel;
            Assert.Equal(commValues, viewModel.CommValues);

            _licenseeServiceMock.Verify(x => x.GetProductAccess(_contextServiceMock.Object.LicenseeId, "CommissionGroups"), Times.Once);
        }

        [Fact]
        public async Task SingleMoveGroup_ShouldReturnViewWithModel_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<SingleMoveGroupQuery>();
            var groups = _fixture.Create<IEnumerable<PlayerCommissionViewModel>>();
            var currentGroup = _fixture.Create<PlayerCommissionViewModel>();


            _playerCommServiceMock.Setup(x => x.GetAllCommGroups(query, _productAccessMock.Object)).ReturnsAsync(groups);
            _playerCommServiceMock.Setup(x => x.GetPlayerCommissionGroup(query, _productAccessMock.Object)).ReturnsAsync(currentGroup);
            _contextServiceMock.Setup(x => x.IsInRoles(It.IsAny<string[]>())).Returns(true);

            // Act
            var result = await _sut.SingleMoveGroup(query);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<SingleMoveGroupViewModel>(result.Model);


            var viewModel = result.Model as SingleMoveGroupViewModel;
            Assert.Equal(groups, viewModel.Groups);
            Assert.Equal(currentGroup, viewModel.CurrentGroup);


            _licenseeServiceMock.Verify(x => x.GetProductAccess(_contextServiceMock.Object.LicenseeId, "CommissionGroups"), Times.Once);
            _playerCommServiceMock.Verify(x => x.GetAllCommGroups(query, _productAccessMock.Object), Times.Once);
            _playerCommServiceMock.Verify(x => x.GetPlayerCommissionGroup(query, _productAccessMock.Object), Times.Once);
        }

        [Fact]
        public void SingleRemoveFromGroup_ShouldReturnViewWithModel_WhenCalled()
        {
            // Arrange
            var query = new SingleRemoveFromGroupQuery { CustId = 123 };

            // Act
            var result = _sut.SingleRemoveFromGroup(query);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<MultipleRemoveFromGroupViewModel>(result.Model);


            Assert.Equal("123", query.CustIds);
        }

        [Fact]
        public async Task SingleUpdateGroup_ShouldReturnViewWithModel_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<SingleUpdateGroupQuery>();
            var groups = _fixture.Create<IEnumerable<PlayerCommissionViewModel>>();
            var currentGroup = _fixture.Create<PlayerCommissionViewModel>();


            _playerCommServiceMock.Setup(x => x.GetAllCommGroups(query, _productAccessMock.Object)).ReturnsAsync(groups);
            _playerCommServiceMock.Setup(x => x.GetPlayerCommissionGroup(query, _productAccessMock.Object)).ReturnsAsync(currentGroup);

            // Act
            var result = await _sut.SingleUpdateGroup(query);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.IsType<SingleMoveGroupViewModel>(result.Model);


            var viewModel = result.Model as SingleMoveGroupViewModel;
            Assert.Equal(groups, viewModel.Groups);
            Assert.Equal(currentGroup, viewModel.CurrentGroup);
            Assert.True(viewModel.HasCommPermissionWrite);


            _licenseeServiceMock.Verify(x => x.GetProductAccess(_contextServiceMock.Object.LicenseeId, "CommissionGroups"), Times.Once);
            _playerCommServiceMock.Verify(x => x.GetAllCommGroups(query, _productAccessMock.Object), Times.Once);
            _playerCommServiceMock.Verify(x => x.GetPlayerCommissionGroup(query, _productAccessMock.Object), Times.Once);
        }

        [Fact]
        public async Task MultipleMoveGroup_ShouldUseCorrectLicenseeId_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleMoveGroupQuery>();
            var expectedLicenseeId = 12345;


            _contextServiceMock.Setup(x => x.LicenseeId).Returns(expectedLicenseeId);

            // Act
            await _sut.MultipleMoveGroup(query);

            // Assert
            _licenseeServiceMock.Verify(x => x.GetProductAccess(expectedLicenseeId, "CommissionGroups"), Times.Once);
        }

        [Fact]
        public async Task MultipleUpdateGroup_ShouldUseCacheForCommValues_WhenCacheHit()
        {
            // Arrange
            var query = _fixture.Create<MultipleUpdateGroupQuery>();
            var cachedCommValues = _fixture.Create<IEnumerable<PlayerCommissionSettingViewModel>>();
            var cacheKey = $"CommSettings_{query.LicenseeId}";

            // Setup cache to return true on TryGetValue (cache hit) with cached values
            object cacheValue = cachedCommValues;
            _cacheMock.Setup(x => x.TryGetValue(cacheKey, out cacheValue)).Returns(true);

            // Setup cache.CreateEntry to support Set extension method (though it shouldn't be called on cache hit)
            _cacheMock.Setup(x => x.CreateEntry(It.IsAny<object>())).Returns(Mock.Of<ICacheEntry>());

            // Act
            var result = await _sut.MultipleUpdateGroup(query);

            // Assert
            var viewModel = result.Model as MultipleUpdateGroupViewModel;
            Assert.NotNull(viewModel.CommValues);
            Assert.Equal(cachedCommValues, viewModel.CommValues);

            // Verify InitCommValues was not called since we had cache hit
            _playerCommServiceMock.Verify(x => x.InitCommValues(It.IsAny<ProductAccess>()), Times.Never);
        }

        [Fact]
        public void SingleRemoveFromGroup_ShouldSetCustIdsFromCustId_WhenCalled()
        {
            // Arrange
            var query = new SingleRemoveFromGroupQuery { CustId = 456 };

            // Act
            _sut.SingleRemoveFromGroup(query);

            // Assert
            Assert.Equal("456", query.CustIds);
        }
    }
}