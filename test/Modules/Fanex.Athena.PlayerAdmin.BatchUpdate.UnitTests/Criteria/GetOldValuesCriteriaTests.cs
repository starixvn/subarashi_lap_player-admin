namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Criteria
{
    using AutoFixture;
    using BatchUpdate.Criteria;
    using Data;
    using Moq;

    public class GetOldValuesCriteriaTests
    {
        private readonly IEnumerable<int> _custIds;
        private readonly Fixture _fixture;

        public GetOldValuesCriteriaTests()
        {
            _fixture = new Fixture();
            _custIds = _fixture.CreateMany<int>();
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenCalled()
        {
            // Arrange & Act
            var sut = new GetOldValuesCriteria(_custIds, GetOldValuesCriteria.SpecialLevelType);

            // Assert
            Assert.Equal(_custIds, sut.CustIds);
            Assert.Equal(GetOldValuesCriteria.SpecialLevelType, sut.ValueType);
        }

        [Fact]
        public void GetSPName_ShouldReturnSpecialLevelSP_WhenValueTypeIsSpecialLevel()
        {
            // Arrange
            var sut = new GetOldValuesCriteria(_custIds, GetOldValuesCriteria.SpecialLevelType);

            // Act
            var result = sut.GetSPName();

            // Assert
            Assert.Equal("Lrf_CustomerSpecialLevelSel", result);
        }

        [Fact]
        public void GetSPName_ShouldReturnOddsGroupSP_WhenValueTypeIsOddsGroup()
        {
            // Arrange
            var sut = new GetOldValuesCriteria(_custIds, GetOldValuesCriteria.OddsGroupType);

            // Act
            var result = sut.GetSPName();

            // Assert
            Assert.Equal("Lrf_CustomerUgroupSel", result);
        }

        [Fact]
        public void GetSPName_ShouldReturnEmptyString_WhenValueTypeIsInvalid()
        {
            // Arrange
            var sut = new GetOldValuesCriteria(_custIds, 999);

            // Act
            var result = sut.GetSPName();

            // Assert
            Assert.Equal(string.Empty, result);
        }

        [Fact]
        public void SetParameterValues_ShouldSetCustIdsParameter_WhenCalled()
        {
            // Arrange
            var sut = new GetOldValuesCriteria(_custIds, GetOldValuesCriteria.SpecialLevelType);
            var dbMock = new Mock<IObjectDb>();
            var expectedCustIds = string.Join(",", _custIds);

            // Act
            sut.SetParameterValues(dbMock.Object);

            // Assert
            dbMock.Verify(x => x.SetParameterValue("custids", expectedCustIds), Times.Once);
        }

        [Fact]
        public void GetValueColumnName_ShouldReturnSpecialLevel_WhenValueTypeIsSpecialLevel()
        {
            // Arrange
            var sut = new GetOldValuesCriteria(_custIds, GetOldValuesCriteria.SpecialLevelType);

            // Act
            var result = sut.GetValueColumnName();

            // Assert
            Assert.Equal("SpecialLevel", result);
        }

        [Fact]
        public void GetValueColumnName_ShouldReturnUgroup_WhenValueTypeIsOddsGroup()
        {
            // Arrange
            var sut = new GetOldValuesCriteria(_custIds, GetOldValuesCriteria.OddsGroupType);

            // Act
            var result = sut.GetValueColumnName();

            // Assert
            Assert.Equal("ugroup", result);
        }

        [Fact]
        public void GetValueColumnName_ShouldReturnEmptyString_WhenValueTypeIsInvalid()
        {
            // Arrange
            var sut = new GetOldValuesCriteria(_custIds, 999);

            // Act
            var result = sut.GetValueColumnName();

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}