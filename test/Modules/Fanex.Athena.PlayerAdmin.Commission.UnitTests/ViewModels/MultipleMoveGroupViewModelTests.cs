namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.ViewModels
{
    using AutoFixture;
    using Commission.MetaModels;
    using Commission.ViewModels;

    public class MultipleMoveGroupViewModelTests
    {
        private readonly Fixture _fixture;

        public MultipleMoveGroupViewModelTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_ShouldInitializeGroupsFromEnumerable_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleMoveGroupQuery>();
            var groups = new List<PlayerCommissionViewModel>
            {
                new() { GroupCommId = 1, GroupName = "Group 1" },
                new() { GroupCommId = 2, GroupName = "Group 2" }
            };

            // Act
            var sut = new MultipleMoveGroupViewModel(query, groups);

            // Assert
            Assert.NotNull(sut.Groups);
            Assert.Equal(2, sut.Groups.Count);
            Assert.Equal(groups, sut.Groups);
        }

        [Fact]
        public void Constructor_ShouldInheritFromBaseUpdateGroupViewModel_WhenCalled()
        {
            // Arrange
            var query = new MultipleMoveGroupQuery
            {
                CustIds = "1,2,3",
                CustNames = "John,Jane,Bob"
            };
            var groups = _fixture.Create<IEnumerable<PlayerCommissionViewModel>>();

            // Act
            var sut = new MultipleMoveGroupViewModel(query, groups);

            // Assert
            Assert.IsAssignableFrom<BaseUpdateGroupViewModel>(sut);
            Assert.Equal("1,2,3", sut.CustIdsStr);
            Assert.Equal("John,Jane,Bob", sut.CustNamesStr);
        }

        [Fact]
        public void Constructor_ShouldHandleEmptyGroups_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleMoveGroupQuery>();
            var groups = new List<PlayerCommissionViewModel>();

            // Act
            var sut = new MultipleMoveGroupViewModel(query, groups);

            // Assert
            Assert.NotNull(sut.Groups);
            Assert.Empty(sut.Groups);
        }

        [Fact]
        public void Groups_ShouldBeSettableAndGettable_WhenAssigned()
        {
            // Arrange
            var query = _fixture.Create<MultipleMoveGroupQuery>();
            var initialGroups = _fixture.Create<IEnumerable<PlayerCommissionViewModel>>();
            var sut = new MultipleMoveGroupViewModel(query, initialGroups);
            var newGroups = new List<PlayerCommissionViewModel>
            {
                new() { GroupCommId = 99, GroupName = "New Group" }
            };

            // Act
            sut.Groups = newGroups;

            // Assert
            Assert.Equal(newGroups, sut.Groups);
        }

        [Fact]
        public void Constructor_ShouldConvertEnumerableToList_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleMoveGroupQuery>();
            var groups = _fixture.Create<IEnumerable<PlayerCommissionViewModel>>();

            // Act
            var sut = new MultipleMoveGroupViewModel(query, groups);

            // Assert
            Assert.IsType<List<PlayerCommissionViewModel>>(sut.Groups);
            Assert.Equal(groups.Count(), sut.Groups.Count);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(5)]
        [InlineData(10)]
        public void Constructor_ShouldHandleVariousGroupCounts_WhenCalled(int groupCount)
        {
            // Arrange
            var query = _fixture.Create<MultipleMoveGroupQuery>();
            var groups = _fixture.CreateMany<PlayerCommissionViewModel>(groupCount);

            // Act
            var sut = new MultipleMoveGroupViewModel(query, groups);

            // Assert
            Assert.Equal(groupCount, sut.Groups.Count);
        }

        [Fact]
        public void Constructor_ShouldPreserveGroupOrder_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<MultipleMoveGroupQuery>();
            var groups = new List<PlayerCommissionViewModel>
            {
                new() { GroupCommId = 3, GroupName = "Third" },
                new() { GroupCommId = 1, GroupName = "First" },
                new() { GroupCommId = 2, GroupName = "Second" }
            };

            // Act
            var sut = new MultipleMoveGroupViewModel(query, groups);

            // Assert
            Assert.Equal(3, sut.Groups[0].GroupCommId);
            Assert.Equal(1, sut.Groups[1].GroupCommId);
            Assert.Equal(2, sut.Groups[2].GroupCommId);
        }
    }
}