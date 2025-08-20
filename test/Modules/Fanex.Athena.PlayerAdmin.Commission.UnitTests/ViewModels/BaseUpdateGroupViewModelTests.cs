namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.ViewModels
{
    using AutoFixture;
    using Commission.MetaModels;
    using Commission.ViewModels;

    public class BaseUpdateGroupViewModelTests
    {
        private readonly Fixture _fixture;

        public BaseUpdateGroupViewModelTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenQueryProvided()
        {
            // Arrange
            var query = new BasePlayerCommissionUpdateQuery
            {
                CustIds = "1,2,3",
                CustNames = "John,Jane,Bob"
            };

            // Act
            var sut = new BaseUpdateGroupViewModel(query);

            // Assert
            Assert.Equal("1,2,3", sut.CustIdsStr);
            Assert.Equal("John,Jane,Bob", sut.CustNamesStr);
            Assert.Equal(new[] { "1", "2", "3" }, sut.CustIds);
            Assert.Equal(new[] { "John", "Jane", "Bob" }, sut.CustNames);
        }

        [Fact]
        public void Constructor_ShouldHandleNullCustIds_WhenQueryProvided()
        {
            // Arrange
            var query = new BasePlayerCommissionUpdateQuery
            {
                CustIds = null,
                CustNames = "John"
            };

            // Act
            var sut = new BaseUpdateGroupViewModel(query);

            // Assert
            Assert.Equal(string.Empty, sut.CustIdsStr);
            Assert.Equal(new[] { string.Empty }, sut.CustIds);
        }

        [Fact]
        public void Constructor_ShouldHandleNullCustNames_WhenQueryProvided()
        {
            // Arrange
            var query = new BasePlayerCommissionUpdateQuery
            {
                CustIds = "1",
                CustNames = null
            };

            // Act
            var sut = new BaseUpdateGroupViewModel(query);

            // Assert
            Assert.Equal(string.Empty, sut.CustNamesStr);
            Assert.Equal(new[] { string.Empty }, sut.CustNames);
        }

        [Fact]
        public void Constructor_ShouldHandleEmptyStrings_WhenQueryProvided()
        {
            // Arrange
            var query = new BasePlayerCommissionUpdateQuery
            {
                CustIds = "",
                CustNames = ""
            };

            // Act
            var sut = new BaseUpdateGroupViewModel(query);

            // Assert
            Assert.Equal(string.Empty, sut.CustIdsStr);
            Assert.Equal(string.Empty, sut.CustNamesStr);
            Assert.Equal(new[] { string.Empty }, sut.CustIds);
            Assert.Equal(new[] { string.Empty }, sut.CustNames);
        }

        [Fact]
        public void Constructor_ShouldSplitSingleValues_WhenQueryProvided()
        {
            // Arrange
            var query = new BasePlayerCommissionUpdateQuery
            {
                CustIds = "123",
                CustNames = "SingleName"
            };

            // Act
            var sut = new BaseUpdateGroupViewModel(query);

            // Assert
            Assert.Equal(new[] { "123" }, sut.CustIds);
            Assert.Equal(new[] { "SingleName" }, sut.CustNames);
        }

        [Fact]
        public void Properties_ShouldBeSettableAndGettable_WhenAssigned()
        {
            // Arrange
            var query = _fixture.Create<BasePlayerCommissionUpdateQuery>();
            var sut = new BaseUpdateGroupViewModel(query);
            var custIds = _fixture.Create<string[]>();
            var custNames = _fixture.Create<string[]>();
            var custIdsStr = _fixture.Create<string>();
            var custNamesStr = _fixture.Create<string>();

            // Act
            sut.CustIds = custIds;
            sut.CustNames = custNames;
            sut.CustIdsStr = custIdsStr;
            sut.CustNamesStr = custNamesStr;

            // Assert
            Assert.Equal(custIds, sut.CustIds);
            Assert.Equal(custNames, sut.CustNames);
            Assert.Equal(custIdsStr, sut.CustIdsStr);
            Assert.Equal(custNamesStr, sut.CustNamesStr);
        }

        [Theory]
        [InlineData("1,2,3", "John,Jane,Bob")]
        [InlineData("123", "SingleName")]
        [InlineData("1,2,3,4,5", "A,B,C,D,E")]
        public void Constructor_ShouldCorrectlySplitCommaSeparatedValues_WhenQueryProvided(string custIds, string custNames)
        {
            // Arrange
            var query = new BasePlayerCommissionUpdateQuery
            {
                CustIds = custIds,
                CustNames = custNames
            };

            // Act
            var sut = new BaseUpdateGroupViewModel(query);

            // Assert
            Assert.Equal(custIds.Split(','), sut.CustIds);
            Assert.Equal(custNames.Split(','), sut.CustNames);
        }
    }
}