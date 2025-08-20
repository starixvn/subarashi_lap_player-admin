namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.ViewModels
{
    using AutoFixture;
    using Commission.MetaModels;
    using Commission.ViewModels;

    public class SingleMoveGroupViewModelTests
    {
        private readonly Fixture _fixture;

        public SingleMoveGroupViewModelTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_ShouldInitializeAllProperties_WhenCalled()
        {
            // Arrange
            var query = new SingleMoveGroupQuery { CustId = 123 };
            var groups = new List<PlayerCommissionViewModel>
            {
                new() { GroupCommId = 1, GroupName = "Group 1" },
                new() { GroupCommId = 2, GroupName = "Group 2" }
            };
            var currentGroup = new PlayerCommissionViewModel { GroupCommId = 5 };
            var hasCommPermissionWrite = true;

            // Act
            var sut = new SingleMoveGroupViewModel(query, groups, currentGroup, hasCommPermissionWrite);

            // Assert
            Assert.Equal(123, sut.CustId);
            Assert.Equal(2, sut.Groups.Count);
            Assert.Equal(currentGroup, sut.CurrentGroup);
            Assert.True(sut.HasCommPermissionWrite);
        }

        [Fact]
        public void Constructor_ShouldConvertGroupsToList_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<SingleMoveGroupQuery>();
            var groups = _fixture.Create<IEnumerable<PlayerCommissionViewModel>>();
            var currentGroup = _fixture.Create<PlayerCommissionViewModel>();
            var hasCommPermissionWrite = _fixture.Create<bool>();

            // Act
            var sut = new SingleMoveGroupViewModel(query, groups, currentGroup, hasCommPermissionWrite);

            // Assert
            Assert.IsType<List<PlayerCommissionViewModel>>(sut.Groups);
            Assert.Equal(groups.Count(), sut.Groups.Count);
        }

        [Fact]
        public void Constructor_ShouldNotInsertGroupNone_WhenCurrentGroupIsNotZero()
        {
            // Arrange
            var query = new SingleMoveGroupQuery { CustId = 123 };
            var groups = new List<PlayerCommissionViewModel>
            {
                new() { GroupCommId = 1, GroupName = "Group 1" }
            };
            var currentGroup = new PlayerCommissionViewModel { GroupCommId = 5 }; // Not zero
            var hasCommPermissionWrite = true;

            // Act
            var sut = new SingleMoveGroupViewModel(query, groups, currentGroup, hasCommPermissionWrite);

            // Assert
            Assert.Equal(1, sut.Groups.Count);
            Assert.Equal(1, sut.Groups.First().GroupCommId);
        }

        [Fact]
        public void Constructor_ShouldInsertGroupNone_WhenCurrentGroupIsZero()
        {
            // Arrange
            var query = new SingleMoveGroupQuery { CustId = 123 };
            var groups = new List<PlayerCommissionViewModel>
            {
                new() { GroupCommId = 1, GroupName = "Group 1" }
            };
            var currentGroup = new PlayerCommissionViewModel
            {
                GroupCommId = 0,
                CommValues = _fixture.Create<IEnumerable<PlayerCommissionSettingViewModel>>()
            };
            var hasCommPermissionWrite = true;

            // Act
            var sut = new SingleMoveGroupViewModel(query, groups, currentGroup, hasCommPermissionWrite);

            // Assert
            Assert.Equal(2, sut.Groups.Count);
            Assert.Equal(0, sut.Groups.First().GroupCommId);
            Assert.NotNull(sut.Groups.First().GroupName);
            Assert.Equal(currentGroup.CommValues, sut.Groups.First().CommValues);
        }

        [Fact]
        public void Properties_ShouldBeSettableAndGettable_WhenAssigned()
        {
            // Arrange
            var query = _fixture.Create<SingleMoveGroupQuery>();
            var groups = _fixture.Create<IEnumerable<PlayerCommissionViewModel>>();
            var currentGroup = _fixture.Create<PlayerCommissionViewModel>();
            var hasCommPermissionWrite = _fixture.Create<bool>();
            var sut = new SingleMoveGroupViewModel(query, groups, currentGroup, hasCommPermissionWrite);

            var newCustId = _fixture.Create<int>();
            var newCurrentGroup = _fixture.Create<PlayerCommissionViewModel>();
            var newGroups = _fixture.Create<List<PlayerCommissionViewModel>>();
            var newHasCommPermissionWrite = !hasCommPermissionWrite;

            // Act
            sut.CustId = newCustId;
            sut.CurrentGroup = newCurrentGroup;
            sut.Groups = newGroups;
            sut.HasCommPermissionWrite = newHasCommPermissionWrite;

            // Assert
            Assert.Equal(newCustId, sut.CustId);
            Assert.Equal(newCurrentGroup, sut.CurrentGroup);
            Assert.Equal(newGroups, sut.Groups);
            Assert.Equal(newHasCommPermissionWrite, sut.HasCommPermissionWrite);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Constructor_ShouldSetHasCommPermissionWrite_WhenCalled(bool hasCommPermissionWrite)
        {
            // Arrange
            var query = _fixture.Create<SingleMoveGroupQuery>();
            var groups = _fixture.Create<IEnumerable<PlayerCommissionViewModel>>();
            var currentGroup = _fixture.Create<PlayerCommissionViewModel>();

            // Act
            var sut = new SingleMoveGroupViewModel(query, groups, currentGroup, hasCommPermissionWrite);

            // Assert
            Assert.Equal(hasCommPermissionWrite, sut.HasCommPermissionWrite);
        }

        [Fact]
        public void Constructor_ShouldHandleEmptyGroups_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<SingleMoveGroupQuery>();
            var groups = new List<PlayerCommissionViewModel>();
            var currentGroup = new PlayerCommissionViewModel { GroupCommId = 1 };
            var hasCommPermissionWrite = true;

            // Act
            var sut = new SingleMoveGroupViewModel(query, groups, currentGroup, hasCommPermissionWrite);

            // Assert
            Assert.Empty(sut.Groups);
        }

        [Fact]
        public void Constructor_ShouldPreserveGroupOrder_WhenCalled()
        {
            // Arrange
            var query = _fixture.Create<SingleMoveGroupQuery>();
            var groups = new List<PlayerCommissionViewModel>
            {
                new() { GroupCommId = 3, GroupName = "Third" },
                new() { GroupCommId = 1, GroupName = "First" },
                new() { GroupCommId = 2, GroupName = "Second" }
            };
            var currentGroup = new PlayerCommissionViewModel { GroupCommId = 5 };
            var hasCommPermissionWrite = true;

            // Act
            var sut = new SingleMoveGroupViewModel(query, groups, currentGroup, hasCommPermissionWrite);

            // Assert
            Assert.Equal(3, sut.Groups[0].GroupCommId);
            Assert.Equal(1, sut.Groups[1].GroupCommId);
            Assert.Equal(2, sut.Groups[2].GroupCommId);
        }

        [Fact]
        public void InsertGroupNone_ShouldInsertAtBeginning_WhenCurrentGroupIsZero()
        {
            // Arrange
            var query = _fixture.Create<SingleMoveGroupQuery>();
            var groups = new List<PlayerCommissionViewModel>
            {
                new() { GroupCommId = 1, GroupName = "Group 1" },
                new() { GroupCommId = 2, GroupName = "Group 2" }
            };
            var currentGroup = new PlayerCommissionViewModel
            {
                GroupCommId = 0,
                CommValues = new List<PlayerCommissionSettingViewModel>()
            };
            var hasCommPermissionWrite = true;

            // Act
            var sut = new SingleMoveGroupViewModel(query, groups, currentGroup, hasCommPermissionWrite);

            // Assert
            Assert.Equal(3, sut.Groups.Count);
            Assert.Equal(0, sut.Groups[0].GroupCommId);
            Assert.Equal(1, sut.Groups[1].GroupCommId);
            Assert.Equal(2, sut.Groups[2].GroupCommId);
        }
    }
}