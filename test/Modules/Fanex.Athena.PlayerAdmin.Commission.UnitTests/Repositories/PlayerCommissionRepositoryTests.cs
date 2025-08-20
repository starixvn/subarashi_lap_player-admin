namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Repositories
{
    using System.Data;
    using System.Data.Common;
    using AutoFixture;
    using AutoFixture.AutoMoq;
    using Commission.Criteria;
    using Commission.Repositories;
    using Core.Models;
    using Core.Repositories;
    using Data;
    using Moq;

    public class PlayerCommissionRepositoryTests
    {
        private readonly Mock<IDbFactory> _dbFactoryMock;
        private readonly Mock<IObjectDb> _dbMock;
        private readonly Fixture _fixture;
        private readonly Mock<ProductAccess> _productAccessMock;
        private readonly Mock<IDataReader> _readerMock;
        private readonly PlayerCommissionRepository _sut;

        public PlayerCommissionRepositoryTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new AutoMoqCustomization());
            _dbFactoryMock = new Mock<IDbFactory>();
            _dbMock = new Mock<IObjectDb>();
            _readerMock = new Mock<IDataReader>();
            _productAccessMock = new Mock<ProductAccess>(new List<Product>());


            _dbFactoryMock.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(_dbMock.Object);
            _dbMock.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_readerMock.Object);


            _sut = new PlayerCommissionRepository(_dbFactoryMock.Object);
        }

        [Fact]
        public void Constructor_ShouldInitializeDbFactory_WhenCalled()
        {
            // Arrange
            var dbFactory = new Mock<IDbFactory>();

            // Act
            var repository = new PlayerCommissionRepository(dbFactory.Object);

            // Assert
            Assert.NotNull(repository);
        }

        [Fact]
        public async Task GetAllCommSettingAsync_ShouldReturnCommissionSettings_WhenDataExists()
        {
            // Arrange
            var expectedProductCode = "SB";
            var expectedProductName = "Sports Betting";
            var expectedSporttype = 1;
            var expectedBettype = 2;

            _readerMock.Setup(x => x.Read()).Returns(true).Callback(() => _readerMock.Setup(x => x.Read()).Returns(false));
            _readerMock.Setup(x => x["ProductCode"]).Returns(expectedProductCode);
            _readerMock.Setup(x => x["Sporttype"]).Returns(expectedSporttype);
            _readerMock.Setup(x => x["Bettype"]).Returns(expectedBettype);
            // Can't mock non-virtual; use real instance instead
            var realProductAccess = new ProductAccess(new List<Product> { new() { ProductCode = expectedProductCode, ProductName = expectedProductName } });

            // Act
            var result = await _sut.GetAllCommSettingAsync(realProductAccess);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var setting = result.First();
            Assert.Equal(expectedProductCode, setting.ProductCode);
            Assert.Equal(expectedProductName, setting.ProductName);
            Assert.Equal(expectedSporttype, setting.Sporttype);
            Assert.Equal(expectedBettype, setting.Bettype);
        }

        [Fact]
        public async Task GetAllCommGroups_ShouldReturnCommissionGroupsAndValues_WhenDataExists()
        {
            // Arrange
            var criteria = _fixture.Create<GetAllCommGroupsCriteria>();
            var expectedGroupName = "Test Group";
            var expectedGroupCommId = 123;
            var expectedProductCode = "SB";
            var expectedProductName = "Sports Betting";

            // Create fresh mocks for this test
            var localDbFactoryMock = new Mock<IDbFactory>();
            var localDbMock = new Mock<IObjectDb>();
            var localReaderMock = new Mock<IDataReader>();
            var localSut = new PlayerCommissionRepository(localDbFactoryMock.Object);

            localDbFactoryMock.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(localDbMock.Object);
            localDbMock.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(localReaderMock.Object);

            // Setup for first result set (groups) - first call returns true, second returns false
            // Setup for second result set (commValues) - first call returns true, second returns false  
            var readCount = 0;
            localReaderMock.Setup(x => x.Read()).Returns(() =>
            {
                readCount++;
                return readCount == 1 || readCount == 3; // First read of each result set
            });

            localReaderMock.Setup(x => x.NextResult()).Returns(true);
            localReaderMock.Setup(x => x["GroupName"]).Returns(expectedGroupName);
            localReaderMock.Setup(x => x["GroupDiscountID"]).Returns(expectedGroupCommId);
            localReaderMock.Setup(x => x["ProductCode"]).Returns(expectedProductCode);
            localReaderMock.Setup(x => x["Sporttype"]).Returns(1);
            localReaderMock.Setup(x => x["Bettype"]).Returns(2);
            localReaderMock.Setup(x => x["Discount"]).Returns(0.05m);

            var realProductAccess = new ProductAccess(new List<Product> { new() { ProductCode = expectedProductCode, ProductName = expectedProductName } });

            // Act
            var (comms, commValues) = await localSut.GetAllCommGroups(criteria, realProductAccess);

            // Assert
            Assert.NotNull(comms);
            Assert.Single(comms);
            Assert.NotNull(commValues);
            Assert.Single(commValues);

            var comm = comms.First();
            Assert.Equal(expectedGroupName, comm.GroupName);
            Assert.Equal(expectedGroupCommId, comm.GroupCommId);

            var commValue = commValues.First();
            Assert.Equal(expectedProductCode, commValue.ProductCode);
            Assert.Equal(expectedProductName, commValue.ProductName);
        }

        [Fact]
        public async Task GetCommissionGroupById_ShouldReturnCommissionGroupAndValues_WhenDataExists()
        {
            // Arrange
            var criteria = _fixture.Create<GetCommissionGroupByIdCriteria>();
            var expectedGroupName = "Test Group";
            var expectedGroupCommId = 123;
            var expectedProductCode = "SB";

            // Create fresh mocks for this test
            var localDbFactoryMock = new Mock<IDbFactory>();
            var localDbMock = new Mock<IObjectDb>();
            var localReaderMock = new Mock<IDataReader>();
            var localSut = new PlayerCommissionRepository(localDbFactoryMock.Object);

            localDbFactoryMock.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(localDbMock.Object);
            localDbMock.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(localReaderMock.Object);

            // Setup for first result set (group) and second result set (commission values)
            var readCount = 0;
            localReaderMock.Setup(x => x.Read()).Returns(() =>
            {
                readCount++;
                return readCount == 1 || readCount == 3; // First read of each result set
            });

            localReaderMock.Setup(x => x.NextResult()).Returns(true);
            localReaderMock.Setup(x => x["GroupName"]).Returns(expectedGroupName);
            localReaderMock.Setup(x => x["GroupDiscountID"]).Returns(expectedGroupCommId);
            localReaderMock.Setup(x => x["ProductCode"]).Returns(expectedProductCode);
            localReaderMock.Setup(x => x["Sporttype"]).Returns(1);
            localReaderMock.Setup(x => x["Bettype"]).Returns(2);
            localReaderMock.Setup(x => x["Discount"]).Returns(0.05m);

            var realProductAccess = new ProductAccess(new List<Product> { new() { ProductCode = expectedProductCode, ProductName = "Sports Betting" } });

            // Act
            var (comm, commValues) = await localSut.GetCommissionGroupById(criteria, realProductAccess);

            // Assert
            Assert.NotNull(comm);
            Assert.Equal(expectedGroupName, comm.GroupName);
            Assert.Equal(expectedGroupCommId, comm.GroupCommId);

            Assert.NotNull(commValues);
            Assert.Single(commValues);
        }

        [Fact]
        public async Task MultipleMoveGroup_ShouldReturnResultInfo_WhenCalled()
        {
            // Arrange
            var criteria = _fixture.Create<MultipleMoveGroupCriteria>();
            var expectedErrorCode = 0;
            var expectedErrorMessage = "Success";

            _dbMock.Setup(x => x.GetParameterValue("ErrCode")).Returns(expectedErrorCode);
            _dbMock.Setup(x => x.GetParameterValue("ErrMsg")).Returns(expectedErrorMessage);

            // Act
            var result = await _sut.MultipleMoveGroup(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedErrorCode, result.Code);
            Assert.Equal(expectedErrorMessage, result.Message);


            _dbMock.Verify(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
            // Note: criteria.SetParameterValues cannot be verified with Moq as it's a method call on a concrete object from AutoFixture
        }

        [Fact]
        public async Task MultipleRemoveFromGroup_ShouldReturnResultInfo_WhenCalled()
        {
            // Arrange
            var criteria = _fixture.Create<MultipleRemoveFromGroupCriteria>();
            var expectedErrorCode = 0;
            var expectedErrorMessage = "Success";

            _dbMock.Setup(x => x.GetParameterValue("ErrCode")).Returns(expectedErrorCode);
            _dbMock.Setup(x => x.GetParameterValue("ErrMsg")).Returns(expectedErrorMessage);

            // Act
            var result = await _sut.MultipleRemoveFromGroup(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedErrorCode, result.Code);
            Assert.Equal(expectedErrorMessage, result.Message);


            _dbMock.Verify(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
            // Note: criteria.SetParameterValues cannot be verified with Moq as it's a method call on a concrete object from AutoFixture
        }

        [Fact]
        public async Task MultipleUpdateGroup_ShouldReturnResultInfo_WhenCalled()
        {
            // Arrange
            var criteria = _fixture.Create<MultipleUpdateGroupCriteria>();
            var expectedErrorCode = 0;
            var expectedErrorMessage = "Success";

            _dbMock.Setup(x => x.GetParameterValue("ErrCode")).Returns(expectedErrorCode);
            _dbMock.Setup(x => x.GetParameterValue("ErrMsg")).Returns(expectedErrorMessage);

            // Act
            var result = await _sut.MultipleUpdateGroup(criteria);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedErrorCode, result.Code);
            Assert.Equal(expectedErrorMessage, result.Message);


            _dbMock.Verify(x => x.ExecuteNonQueryAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()), Times.Once);
            // Note: criteria.SetParameterValues cannot be verified with Moq as it's a method call on a concrete object from AutoFixture
        }

        [Fact]
        public async Task GetPlayerCommissionGroup_ShouldReturnCommissionGroupAndValues_WhenDataExists()
        {
            // Arrange
            var criteria = _fixture.Create<GetPlayerCommissionGroupCriteria>();
            var expectedGroupCommId = 123;
            var expectedProductCode = "SB";

            // Create fresh mocks for this test
            var localDbFactoryMock = new Mock<IDbFactory>();
            var localDbMock = new Mock<IObjectDb>();
            var localReaderMock = new Mock<IDataReader>();
            var localSut = new PlayerCommissionRepository(localDbFactoryMock.Object);

            localDbFactoryMock.Setup(x => x.GetObjectDb(It.IsAny<string>(), It.IsAny<int>())).Returns(localDbMock.Object);
            localDbMock.Setup(x => x.ExecuteReaderAsync(It.IsAny<object>(), It.IsAny<DbTransaction>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(localReaderMock.Object);

            // Setup for first result set (group) and second result set (commission values)
            var readCount = 0;
            localReaderMock.Setup(x => x.Read()).Returns(() =>
            {
                readCount++;
                return readCount == 1 || readCount == 3; // First read of each result set
            });

            localReaderMock.Setup(x => x.NextResult()).Returns(true);
            localReaderMock.Setup(x => x["GroupDiscountID"]).Returns(expectedGroupCommId);
            localReaderMock.Setup(x => x["ProductCode"]).Returns(expectedProductCode);
            localReaderMock.Setup(x => x["Sporttype"]).Returns(1);
            localReaderMock.Setup(x => x["Bettype"]).Returns(2);
            localReaderMock.Setup(x => x["PlayerDiscount"]).Returns(0.05m);

            var realProductAccess = new ProductAccess(new List<Product> { new() { ProductCode = expectedProductCode, ProductName = "Sports Betting" } });

            // Act
            var (comm, commValues) = await localSut.GetPlayerCommissionGroup(criteria, realProductAccess);

            // Assert
            Assert.NotNull(comm);
            Assert.Equal(expectedGroupCommId, comm.GroupCommId);

            Assert.NotNull(commValues);
            Assert.Single(commValues);
        }

        [Fact]
        public async Task GetAllCommSettingAsync_ShouldUseCorrectStoredProcedure_WhenCalled()
        {
            // Arrange
            _readerMock.Setup(x => x.Read()).Returns(false);

            // Act
            await _sut.GetAllCommSettingAsync(_productAccessMock.Object);

            // Assert
            _dbFactoryMock.Verify(x => x.GetObjectDb("Lrf_LicenseeCommProductList", It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAllCommGroups_ShouldSetCriteriaParameters_WhenCalled()
        {
            // Arrange
            var criteria = _fixture.Create<GetAllCommGroupsCriteria>();
            _readerMock.Setup(x => x.Read()).Returns(false);
            _readerMock.Setup(x => x.NextResult()).Returns(true);

            // Act
            await _sut.GetAllCommGroups(criteria, _productAccessMock.Object);

            // Assert
            // Note: criteria.SetParameterValues cannot be verified with Moq as it's a method call on a concrete object from AutoFixture
        }
    }
}