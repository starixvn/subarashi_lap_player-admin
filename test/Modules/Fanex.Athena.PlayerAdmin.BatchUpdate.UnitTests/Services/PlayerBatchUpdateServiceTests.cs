namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Services
{
    using AutoFixture;
    using BatchUpdate.Criteria;
    using BatchUpdate.MetaModels;
    using BatchUpdate.Repositories;
    using BatchUpdate.Services;
    using Core.Models;
    using Core.Services;
    using MediatR;
    using Moq;

    public class PlayerBatchUpdateServiceTests
    {
        private readonly Mock<IContextService> _contextServiceMock;
        private readonly Fixture _fixture;
        private readonly Mock<ILicenseeService> _licenseeServiceMock;
        private readonly Mock<IPublisher> _publisherMock;
        private readonly Mock<IPlayerBatchUpdateRepository> _repositoryMock;
        private readonly PlayerBatchUpdateService _sut;

        public PlayerBatchUpdateServiceTests()
        {
            _fixture = new Fixture();
            _publisherMock = new Mock<IPublisher>();
            _repositoryMock = new Mock<IPlayerBatchUpdateRepository>();
            _licenseeServiceMock = new Mock<ILicenseeService>();
            _contextServiceMock = new Mock<IContextService>();
            _sut = new PlayerBatchUpdateService(_publisherMock.Object, _contextServiceMock.Object, _licenseeServiceMock.Object, _repositoryMock.Object);
        }

        [Fact]
        public async Task UpdateOddsGroup_ShouldReturnSuccessResult_WhenAllCustomersAreValid()
        {
            // Arrange
            var metaModel = _fixture.Create<UpdateOddsGroupApiMetaModel>();
            metaModel.CustIds = "123,456";
            metaModel.CustNames = "Customer1,Customer2";
            var validCustIds = new List<int> { 123, 456 };
            var oldValues = new Dictionary<string, string> { { "123", "oldGroup1" }, { "456", "oldGroup2" } };
            var updateResult = new ResultInfo { Code = 0, Message = string.Empty };

            _contextServiceMock.Setup(x => x.AgentList).Returns(_fixture.Create<IReadOnlyDictionary<int, int[]>>());
            _licenseeServiceMock.Setup(x => x.GetCustomersBelongTo(It.IsAny<IReadOnlyDictionary<int, int[]>>(), It.IsAny<string>()))
                .ReturnsAsync(validCustIds);
            _repositoryMock.Setup(x => x.GetOldValues(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()))
                .ReturnsAsync(oldValues);
            _repositoryMock.Setup(x => x.Update(It.IsAny<BasePlayerBatchUpdateCriteria>()))
                .ReturnsAsync(updateResult);

            // Act
            var result = await _sut.UpdateOddsGroup(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public async Task UpdateOddsGroup_ShouldReturnErrorResult_WhenSomeCustomersAreInvalid()
        {
            // Arrange
            var metaModel = _fixture.Create<UpdateOddsGroupApiMetaModel>();
            metaModel.CustIds = "123,456";
            metaModel.CustNames = "Customer1,Customer2";
            var validCustIds = new List<int> { 123 }; // Only first customer is valid
            var oldValues = new Dictionary<string, string> { { "123", "oldGroup1" } };
            var updateResult = new ResultInfo { Code = 0, Message = string.Empty };

            _contextServiceMock.Setup(x => x.AgentList).Returns(_fixture.Create<IReadOnlyDictionary<int, int[]>>());
            _licenseeServiceMock.Setup(x => x.GetCustomersBelongTo(It.IsAny<IReadOnlyDictionary<int, int[]>>(), It.IsAny<string>()))
                .ReturnsAsync(validCustIds);
            _repositoryMock.Setup(x => x.GetOldValues(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()))
                .ReturnsAsync(oldValues);
            _repositoryMock.Setup(x => x.Update(It.IsAny<BasePlayerBatchUpdateCriteria>()))
                .ReturnsAsync(updateResult);

            // Act
            var result = await _sut.UpdateOddsGroup(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Code);
            Assert.Contains("Access Denied", result.Message);
        }

        [Fact]
        public async Task UpdateSpecialLevel_ShouldReturnSuccessResult_WhenAllCustomersAreValid()
        {
            // Arrange
            var metaModel = _fixture.Create<UpdateSpecialLevelApiMetaModel>();
            metaModel.CustIds = "123,456";
            metaModel.CustNames = "Customer1,Customer2";
            var validCustIds = new List<int> { 123, 456 };
            var oldValues = new Dictionary<string, string> { { "123", "oldLevel1" }, { "456", "oldLevel2" } };
            var updateResult = new ResultInfo { Code = 0, Message = string.Empty };

            _contextServiceMock.Setup(x => x.AgentList).Returns(_fixture.Create<IReadOnlyDictionary<int, int[]>>());
            _licenseeServiceMock.Setup(x => x.GetCustomersBelongTo(It.IsAny<IReadOnlyDictionary<int, int[]>>(), It.IsAny<string>()))
                .ReturnsAsync(validCustIds);
            _repositoryMock.Setup(x => x.GetOldValues(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()))
                .ReturnsAsync(oldValues);
            _repositoryMock.Setup(x => x.Update(It.IsAny<BasePlayerBatchUpdateCriteria>()))
                .ReturnsAsync(updateResult);

            // Act
            var result = await _sut.UpdateSpecialLevel(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            Assert.True(result.IsSuccessful);
        }

        [Fact]
        public async Task UpdateSpecialLevel_ShouldSkipCustomer_WhenNewValueSameAsOldValue()
        {
            // Arrange
            var metaModel = _fixture.Create<UpdateSpecialLevelApiMetaModel>();
            metaModel.CustIds = "123";
            metaModel.CustNames = "Customer1";
            metaModel.SpecialLevelId = "level1";
            var validCustIds = new List<int> { 123 };
            var oldValues = new Dictionary<string, string> { { "123", "level1" } }; // Same as new value

            _contextServiceMock.Setup(x => x.AgentList).Returns(_fixture.Create<IReadOnlyDictionary<int, int[]>>());
            _licenseeServiceMock.Setup(x => x.GetCustomersBelongTo(It.IsAny<IReadOnlyDictionary<int, int[]>>(), It.IsAny<string>()))
                .ReturnsAsync(validCustIds);
            _repositoryMock.Setup(x => x.GetOldValues(It.IsAny<IEnumerable<int>>(), It.IsAny<int>()))
                .ReturnsAsync(oldValues);

            // Act
            var result = await _sut.UpdateSpecialLevel(metaModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Code);
            _repositoryMock.Verify(x => x.Update(It.IsAny<BasePlayerBatchUpdateCriteria>()), Times.Never);
        }
    }
}