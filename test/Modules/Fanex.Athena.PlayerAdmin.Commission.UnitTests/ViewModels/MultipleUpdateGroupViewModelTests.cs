namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.ViewModels
{
    using AutoFixture;
    using Commission.MetaModels;
    using Commission.ViewModels;

    public class MultipleUpdateGroupViewModelTests
    {
        private readonly Fixture _fixture;

        public MultipleUpdateGroupViewModelTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_ShouldInheritFromBaseUpdateGroupViewModel_WhenCalled()
        {
            // Arrange
            var query = new MultipleUpdateGroupQuery
            {
                CustIds = "1,2,3",
                CustNames = "John,Jane,Bob"
            };

            // Act
            var sut = new MultipleUpdateGroupViewModel(query);

            // Assert
            Assert.IsAssignableFrom<BaseUpdateGroupViewModel>(sut);
            Assert.Equal("1,2,3", sut.CustIdsStr);
            Assert.Equal("John,Jane,Bob", sut.CustNamesStr);
        }

        [Fact]
        public void Constructor_ShouldInitializeFromQuery_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleUpdateGroupQuery>();

            // Act
            var sut = new MultipleUpdateGroupViewModel(query);

            // Assert
            Assert.Equal(query.CustIds ?? string.Empty, sut.CustIdsStr);
            Assert.Equal(query.CustNames ?? string.Empty, sut.CustNamesStr);
        }

        [Fact]
        public void CommValues_ShouldBeSettableAndGettable_WhenAssigned()
        {
            // Arrange
            var query = _fixture.Create<MultipleUpdateGroupQuery>();
            var sut = new MultipleUpdateGroupViewModel(query);
            var commValues = _fixture.Create<IEnumerable<PlayerCommissionSettingViewModel>>();

            // Act
            sut.CommValues = commValues;

            // Assert
            Assert.Equal(commValues, sut.CommValues);
        }

        [Fact]
        public void CommValues_ShouldBeNullByDefault_WhenCreated()
        {
            // Arrange
            var query = _fixture.Create<MultipleUpdateGroupQuery>();

            // Act
            var sut = new MultipleUpdateGroupViewModel(query);

            // Assert
            Assert.Null(sut.CommValues);
        }

        [Fact]
        public void Constructor_ShouldHandleNullValues_WhenQueryProvided()
        {
            // Arrange
            var query = new MultipleUpdateGroupQuery
            {
                CustIds = null,
                CustNames = null
            };

            // Act
            var sut = new MultipleUpdateGroupViewModel(query);

            // Assert
            Assert.Equal(string.Empty, sut.CustIdsStr);
            Assert.Equal(string.Empty, sut.CustNamesStr);
        }

        [Fact]
        public void Constructor_ShouldSplitCustIds_WhenQueryProvided()
        {
            // Arrange
            var query = new MultipleUpdateGroupQuery
            {
                CustIds = "1,2,3,4",
                CustNames = "A,B,C,D"
            };

            // Act
            var sut = new MultipleUpdateGroupViewModel(query);

            // Assert
            Assert.Equal(new[] { "1", "2", "3", "4" }, sut.CustIds);
            Assert.Equal(new[] { "A", "B", "C", "D" }, sut.CustNames);
        }

        [Fact]
        public void CommValues_ShouldAcceptEmptyCollection_WhenAssigned()
        {
            // Arrange
            var query = _fixture.Create<MultipleUpdateGroupQuery>();
            var sut = new MultipleUpdateGroupViewModel(query);
            var emptyCommValues = new List<PlayerCommissionSettingViewModel>();

            // Act
            sut.CommValues = emptyCommValues;

            // Assert
            Assert.NotNull(sut.CommValues);
            Assert.Empty(sut.CommValues);
        }

        [Fact]
        public void CommValues_ShouldAcceptNullValue_WhenAssigned()
        {
            // Arrange
            var query = _fixture.Create<MultipleUpdateGroupQuery>();
            var sut = new MultipleUpdateGroupViewModel(query);

            // Act
            sut.CommValues = null;

            // Assert
            Assert.Null(sut.CommValues);
        }

        [Theory]
        [InlineData("1", "John")]
        [InlineData("1,2", "John,Jane")]
        [InlineData("1,2,3,4,5", "John,Jane,Bob,Alice,Charlie")]
        public void Constructor_ShouldCorrectlyParseDifferentInputSizes_WhenQueryProvided(string custIds, string custNames)
        {
            // Arrange
            var query = new MultipleUpdateGroupQuery
            {
                CustIds = custIds,
                CustNames = custNames
            };

            // Act
            var sut = new MultipleUpdateGroupViewModel(query);

            // Assert
            Assert.Equal(custIds.Split(','), sut.CustIds);
            Assert.Equal(custNames.Split(','), sut.CustNames);
        }

        [Fact]
        public void Properties_ShouldBeInheritedFromBase_WhenAccessed()
        {
            // Arrange
            var query = _fixture.Create<MultipleUpdateGroupQuery>();
            var sut = new MultipleUpdateGroupViewModel(query);

            // Act & Assert
            Assert.NotNull(sut.CustIds);
            Assert.NotNull(sut.CustNames);
            Assert.NotNull(sut.CustIdsStr);
            Assert.NotNull(sut.CustNamesStr);
        }
    }
}