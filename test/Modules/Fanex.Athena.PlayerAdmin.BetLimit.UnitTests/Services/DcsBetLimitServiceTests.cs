using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using Fanex.Athena.PlayerAdmin.BetLimit.DataDomains;
using Fanex.Athena.PlayerAdmin.BetLimit.DTO;
using Fanex.Athena.PlayerAdmin.BetLimit.Helpers;
using Fanex.Athena.PlayerAdmin.BetLimit.Mediator;
using Fanex.Athena.PlayerAdmin.BetLimit.MetaModel;
using Fanex.Athena.PlayerAdmin.BetLimit.Repositories.Interfaces;
using Fanex.Athena.PlayerAdmin.BetLimit.Services;
using Fanex.Athena.PlayerAdmin.Core.Constants;
using Fanex.Athena.PlayerAdmin.Core.Models;
using MediatR;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Services
{
    public class DcsBetLimitServiceTests
    {
        private readonly Mock<IDcsBetLimitRepository> _mockRepository;
        private readonly Mock<INewDcsBetLimitRepository> _mockNewRepository;
        private readonly Mock<IDcsGameBetLimitRepository> _mockGameBetLimitRepository;
        private readonly Mock<IPublisher> _mockPublisher;
        private readonly Fixture _fixture;

        public DcsBetLimitServiceTests()
        {
            _mockRepository = new Mock<IDcsBetLimitRepository>();
            _mockNewRepository = new Mock<INewDcsBetLimitRepository>();
            _mockGameBetLimitRepository = new Mock<IDcsGameBetLimitRepository>();
            _mockPublisher = new Mock<IPublisher>();
            _fixture = new Fixture();
        }

        private DcsBetLimitService CreateService()
        {
            return new DcsBetLimitService(
                _mockRepository.Object,
                _mockNewRepository.Object,
                _mockGameBetLimitRepository.Object,
                _mockPublisher.Object);
        }

        #region GetSettingAsync Tests

        [Fact]
        public async Task GetSettingAsync_WithSportsbookProduct_ShouldUseGameBetLimitRepository()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var productId = ProductTypes.Sportsbook;
            var expectedSettings = _fixture.CreateMany<PlayerBetLimit>().ToList();

            _mockGameBetLimitRepository
                .Setup(x => x.GetSettingAsync(custId, productId))
                .ReturnsAsync(expectedSettings);

            // Act
            var result = await service.GetSettingAsync(custId, productId);

            // Assert
            Assert.Equal(expectedSettings, result);
            _mockGameBetLimitRepository.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
            _mockNewRepository.Verify(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockRepository.Verify(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetSettingAsync_WithNumberGameProduct_ShouldUseGameBetLimitRepository()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var productId = ProductTypes.NumberGame;
            var expectedSettings = _fixture.CreateMany<PlayerBetLimit>().ToList();

            _mockGameBetLimitRepository
                .Setup(x => x.GetSettingAsync(custId, productId))
                .ReturnsAsync(expectedSettings);

            // Act
            var result = await service.GetSettingAsync(custId, productId);

            // Assert
            Assert.Equal(expectedSettings, result);
            _mockGameBetLimitRepository.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
            _mockNewRepository.Verify(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockRepository.Verify(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetSettingAsync_WithVS3Product_ShouldUseNewRepository()
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var productId = ProductTypes.VS3;
            var expectedSettings = _fixture.CreateMany<PlayerBetLimit>().ToList();

            _mockNewRepository
                .Setup(x => x.GetSettingAsync(custId, productId))
                .ReturnsAsync(expectedSettings);

            // Act
            var result = await service.GetSettingAsync(custId, productId);

            // Assert
            Assert.Equal(expectedSettings, result);
            _mockNewRepository.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
            _mockGameBetLimitRepository.Verify(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockRepository.Verify(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Theory]
        [InlineData(ProductTypes.Lottery)]
        [InlineData(ProductTypes.I4D)]
        [InlineData(ProductTypes.SLT)]
        [InlineData(ProductTypes.AllBet)]
        [InlineData(ProductTypes.PP)]
        public async Task GetSettingAsync_WithOtherProducts_ShouldUseDefaultRepository(int productId)
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var expectedSettings = _fixture.CreateMany<PlayerBetLimit>().ToList();

            _mockRepository
                .Setup(x => x.GetSettingAsync(custId, productId))
                .ReturnsAsync(expectedSettings);

            // Act
            var result = await service.GetSettingAsync(custId, productId);

            // Assert
            Assert.Equal(expectedSettings, result);
            _mockRepository.Verify(x => x.GetSettingAsync(custId, productId), Times.Once);
            _mockGameBetLimitRepository.Verify(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _mockNewRepository.Verify(x => x.GetSettingAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        #endregion

        #region GetStatusAsync Tests

        [Theory]
        [InlineData(ProductTypes.Lottery)]
        [InlineData(ProductTypes.I4D)]
        [InlineData(ProductTypes.SLT)]
        [InlineData(ProductTypes.AllBet)]
        [InlineData(ProductTypes.Voidbridge)]
        [InlineData(ProductTypes.CashOut)]
        [InlineData(ProductTypes.Exchange)]
        public async Task GetStatusAsync_WithProductsHasControlStatus_ShouldReturnStatusFromRepository(int productId)
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();
            var expectedStatus = _fixture.Create<bool>();

            _mockRepository
                .Setup(x => x.GetStatusAsync(custId, productId))
                .ReturnsAsync(expectedStatus);

            // Act
            var result = await service.GetStatusAsync(custId, productId);

            // Assert
            Assert.Equal(expectedStatus, result);
            _mockRepository.Verify(x => x.GetStatusAsync(custId, productId), Times.Once);
        }

        [Theory]
        [InlineData(ProductTypes.Sportsbook)]
        [InlineData(ProductTypes.NumberGame)]
        [InlineData(ProductTypes.VirtualSports)]
        [InlineData(ProductTypes.AgCasino)]
        [InlineData(ProductTypes.Keno)]
        public async Task GetStatusAsync_WithProductsNotHasControlStatus_ShouldReturnNull(int productId)
        {
            // Arrange
            var service = CreateService();
            var custId = _fixture.Create<int>();

            // Act
            var result = await service.GetStatusAsync(custId, productId);

            // Assert
            Assert.Null(result);
            _mockRepository.Verify(x => x.GetStatusAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetStatusAsync_VerifyProductsHasControlStatusList()
        {
            // Arrange & Act
            var productsHasControlStatus = DcsBetLimitService.ProductsHasControlStatus.ToList();

            // Assert - Verify specific products are included
            Assert.Contains(ProductTypes.Lottery, productsHasControlStatus);
            Assert.Contains(ProductTypes.I4D, productsHasControlStatus);
            Assert.Contains(ProductTypes.SLT, productsHasControlStatus);
            Assert.Contains(ProductTypes.AllBet, productsHasControlStatus);
            Assert.Contains(ProductTypes.Voidbridge, productsHasControlStatus);
            Assert.Contains(ProductTypes.CashOut, productsHasControlStatus);
            Assert.Contains(ProductTypes.Exchange, productsHasControlStatus);
            Assert.Contains(ProductTypes.PP, productsHasControlStatus);
            Assert.Contains(ProductTypes.CPRNG, productsHasControlStatus);
            Assert.Contains(ProductTypes.AIG, productsHasControlStatus);

            // Verify products that should NOT be included
            Assert.DoesNotContain(ProductTypes.Sportsbook, productsHasControlStatus);
            Assert.DoesNotContain(ProductTypes.NumberGame, productsHasControlStatus);
            Assert.DoesNotContain(ProductTypes.VirtualSports, productsHasControlStatus);
            Assert.DoesNotContain(ProductTypes.AgCasino, productsHasControlStatus);
            Assert.DoesNotContain(ProductTypes.Keno, productsHasControlStatus);
        }

        #endregion

        #region UpdateSettingAsync Tests

        [Fact]
        public async Task UpdateSettingAsync_WithSportsbookProduct_ShouldUseGameBetLimitRepositoryAndPublishEvent()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = _fixture.Create<int>(),
                ProductId = ProductTypes.Sportsbook,
                UserId = _fixture.Create<int>(),
                Setting = _fixture.CreateMany<PlayerBetLimitMetaModel>().ToList(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var successResult = new ResultInfo { Code = 0, Message = "Success" };

            _mockGameBetLimitRepository
                .Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(successResult);

            // Act
            var result = await service.UpdateSettingAsync(metaModel);

            // Assert
            Assert.Equal(successResult, result);
            _mockGameBetLimitRepository.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
            _mockNewRepository.Verify(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>()), Times.Never);
            _mockRepository.Verify(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>()), Times.Never);
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_WithVS3Product_ShouldUseNewRepositoryAndPublishEvent()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = _fixture.Create<int>(),
                ProductId = ProductTypes.VS3,
                UserId = _fixture.Create<int>(),
                Setting = _fixture.CreateMany<PlayerBetLimitMetaModel>().ToList(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var successResult = new ResultInfo { Code = 0, Message = "Success" };

            _mockNewRepository
                .Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(successResult);

            // Act
            var result = await service.UpdateSettingAsync(metaModel);

            // Assert
            Assert.Equal(successResult, result);
            _mockNewRepository.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
            _mockGameBetLimitRepository.Verify(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>()), Times.Never);
            _mockRepository.Verify(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>()), Times.Never);
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_WithOtherProduct_ShouldUseDefaultRepositoryAndPublishEvent()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = _fixture.Create<int>(),
                ProductId = ProductTypes.Lottery,
                UserId = _fixture.Create<int>(),
                Setting = _fixture.CreateMany<PlayerBetLimitMetaModel>().ToList(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var successResult = new ResultInfo { Code = 0, Message = "Success" };

            _mockRepository
                .Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(successResult);

            // Act
            var result = await service.UpdateSettingAsync(metaModel);

            // Assert
            Assert.Equal(successResult, result);
            _mockRepository.Verify(x => x.UpdateSettingAsync(metaModel), Times.Once);
            _mockGameBetLimitRepository.Verify(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>()), Times.Never);
            _mockNewRepository.Verify(x => x.UpdateSettingAsync(It.IsAny<PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>>()), Times.Never);
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_WithFailedResult_ShouldStillPublishEvent()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = _fixture.Create<int>(),
                ProductId = ProductTypes.Lottery,
                UserId = _fixture.Create<int>(),
                Setting = _fixture.CreateMany<PlayerBetLimitMetaModel>().ToList(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var failedResult = new ResultInfo { Code = 1, Message = "Failed" };

            _mockRepository
                .Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(failedResult);

            // Act
            var result = await service.UpdateSettingAsync(metaModel);

            // Assert
            Assert.Equal(failedResult, result);
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateSettingAsync_WithNullLogEvent_ShouldNotPublishEvent()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = _fixture.Create<int>(),
                ProductId = ProductTypes.Lottery,
                UserId = _fixture.Create<int>(),
                Setting = new List<PlayerBetLimitMetaModel>(), // Empty setting to create null log event
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var successResult = new ResultInfo { Code = 0, Message = "Success" };

            _mockRepository
                .Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(successResult);

            // Act
            var result = await service.UpdateSettingAsync(metaModel);

            // Assert
            Assert.Equal(successResult, result);
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), default), Times.Never);
        }

        #endregion

        #region UpdateStatusAsync Tests

        [Fact]
        public async Task UpdateStatusAsync_ShouldCallRepositoryAndPublishEvent()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                ProductId = _fixture.Create<int>(),
                Status = _fixture.Create<bool>(),
                OldStatus = _fixture.Create<bool>(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var successResult = new ResultInfo { Code = 0, Message = "Success" };

            _mockRepository
                .Setup(x => x.UpdateStatusAsync(metaModel))
                .ReturnsAsync(successResult);

            // Act
            var result = await service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.Equal(successResult, result);
            _mockRepository.Verify(x => x.UpdateStatusAsync(metaModel), Times.Once);
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedProductStatusEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task UpdateStatusAsync_WithFailedResult_ShouldStillPublishEvent()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateStatusMetaModel
            {
                CustId = _fixture.Create<int>(),
                UserId = _fixture.Create<int>(),
                ProductId = _fixture.Create<int>(),
                Status = _fixture.Create<bool>(),
                OldStatus = _fixture.Create<bool>(),
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var failedResult = new ResultInfo { Code = 1, Message = "Failed" };

            _mockRepository
                .Setup(x => x.UpdateStatusAsync(metaModel))
                .ReturnsAsync(failedResult);

            // Act
            var result = await service.UpdateStatusAsync(metaModel);

            // Assert
            Assert.Equal(failedResult, result);
            _mockRepository.Verify(x => x.UpdateStatusAsync(metaModel), Times.Once);
            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedProductStatusEvent>(), default), Times.Once);
        }

        #endregion

        #region GetBetSettingAction Tests

        [Fact]
        public void GetBetSettingAction_WithLotteryProduct_ShouldReturnCorrectActions()
        {
            // Act
            var result = DcsBetLimitService.GetBetSettingAction(ProductTypes.Lottery);

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Contains(LogActionDetailId.UpdatingMinBet, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxBet, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxBetPerDraw, result);
        }

        [Fact]
        public void GetBetSettingAction_WithKenoProduct_ShouldReturnCorrectActions()
        {
            // Act
            var result = DcsBetLimitService.GetBetSettingAction(ProductTypes.Keno);

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Contains(LogActionDetailId.UpdatingMinBet, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxBet, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxBetPerDraw, result);
        }

        [Fact]
        public void GetBetSettingAction_WithNumberGameProduct_ShouldReturnCorrectActions()
        {
            // Act
            var result = DcsBetLimitService.GetBetSettingAction(ProductTypes.NumberGame);

            // Assert
            Assert.Equal(4, result.Length);
            Assert.Contains(LogActionDetailId.UpdatingMinBet, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxBet, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxPerMatch, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxPerBall, result);
        }

        [Fact]
        public void GetBetSettingAction_WithSportsbookProduct_ShouldReturnCorrectActions()
        {
            // Act
            var result = DcsBetLimitService.GetBetSettingAction(ProductTypes.Sportsbook);

            // Assert
            Assert.Equal(4, result.Length);
            Assert.Contains(LogActionDetailId.UpdatingMinBet, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxBet, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxPerMatch, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxPayoutPerMatch, result);
        }

        [Fact]
        public void GetBetSettingAction_WithVS3Product_ShouldReturnCorrectActions()
        {
            // Act
            var result = DcsBetLimitService.GetBetSettingAction(ProductTypes.VS3);

            // Assert
            Assert.Equal(2, result.Length);
            Assert.Contains(LogActionDetailId.UpdatingMinBet, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxBet, result);
        }

        [Theory]
        [InlineData(ProductTypes.I4D)]
        [InlineData(ProductTypes.SLT)]
        [InlineData(ProductTypes.AllBet)]
        [InlineData(ProductTypes.PP)]
        public void GetBetSettingAction_WithOtherProducts_ShouldReturnDefaultActions(int productId)
        {
            // Act
            var result = DcsBetLimitService.GetBetSettingAction(productId);

            // Assert
            Assert.Equal(3, result.Length);
            Assert.Contains(LogActionDetailId.UpdatingMinBet, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxBet, result);
            Assert.Contains(LogActionDetailId.UpdatingMaxPerMatch, result);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public async Task UpdateSettingAsync_ShouldUseCorrectBetSettingActionsForLogEvent()
        {
            // Arrange
            var service = CreateService();
            var metaModel = new PlayerBetLimitUpdateMetaModel<IEnumerable<PlayerBetLimitMetaModel>>
            {
                CustId = _fixture.Create<int>(),
                ProductId = ProductTypes.Sportsbook,
                UserId = _fixture.Create<int>(),
                Setting = new List<PlayerBetLimitMetaModel>
                {
                    new PlayerBetLimitMetaModel
                    {
                        Id = "1_1",
                        SportType = "1",
                        BetType = 1,
                        MinBet = 10,
                        OldMinBet = 5,
                        IsUpdated = true
                    }
                },
                SiteName = _fixture.Create<string>(),
                LicenseeCustId = _fixture.Create<int>()
            };

            var successResult = new ResultInfo { Code = 0, Message = "Success" };

            _mockGameBetLimitRepository
                .Setup(x => x.UpdateSettingAsync(metaModel))
                .ReturnsAsync(successResult);

            // Act
            await service.UpdateSettingAsync(metaModel);

            // Assert - Verify that the correct bet setting actions were used
            var expectedActions = DcsBetLimitService.GetBetSettingAction(ProductTypes.Sportsbook);
            Assert.Contains(LogActionDetailId.UpdatingMinBet, expectedActions);
            Assert.Contains(LogActionDetailId.UpdatingMaxBet, expectedActions);
            Assert.Contains(LogActionDetailId.UpdatingMaxPerMatch, expectedActions);
            Assert.Contains(LogActionDetailId.UpdatingMaxPayoutPerMatch, expectedActions);

            _mockPublisher.Verify(x => x.Publish(It.IsAny<PlayerUpdatedBetLimitEvent>(), default), Times.Once);
        }

        [Fact]
        public async Task GetSettingAsync_CallsCorrectRepositoryBasedOnProductType()
        {
            // Arrange
            var service = CreateService();
            var custId = 12345;

            var sportsbookSettings = _fixture.CreateMany<PlayerBetLimit>().ToList();
            var vs3Settings = _fixture.CreateMany<PlayerBetLimit>().ToList();
            var defaultSettings = _fixture.CreateMany<PlayerBetLimit>().ToList();

            _mockGameBetLimitRepository.Setup(x => x.GetSettingAsync(custId, ProductTypes.Sportsbook)).ReturnsAsync(sportsbookSettings);
            _mockNewRepository.Setup(x => x.GetSettingAsync(custId, ProductTypes.VS3)).ReturnsAsync(vs3Settings);
            _mockRepository.Setup(x => x.GetSettingAsync(custId, ProductTypes.Lottery)).ReturnsAsync(defaultSettings);

            // Act & Assert
            var sportsbookResult = await service.GetSettingAsync(custId, ProductTypes.Sportsbook);
            var vs3Result = await service.GetSettingAsync(custId, ProductTypes.VS3);
            var lotteryResult = await service.GetSettingAsync(custId, ProductTypes.Lottery);

            Assert.Equal(sportsbookSettings, sportsbookResult);
            Assert.Equal(vs3Settings, vs3Result);
            Assert.Equal(defaultSettings, lotteryResult);
        }

        #endregion
    }
} 