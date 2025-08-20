namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.ViewModels
{
    using AutoFixture;
    using Commission.ViewModels;

    public class PlayerCommissionViewModelTests
    {
        private readonly Fixture _fixture;

        public PlayerCommissionViewModelTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Properties_ShouldBeSettableAndGettable_WhenAssigned()
        {
            // Arrange
            var sut = new PlayerCommissionViewModel();
            var licenseeId = _fixture.Create<int>();
            var groupCommId = _fixture.Create<int>();
            var groupName = _fixture.Create<string>();
            var commValues = _fixture.Create<IEnumerable<PlayerCommissionSettingViewModel>>();

            // Act
            sut.LicenseeId = licenseeId;
            sut.GroupCommId = groupCommId;
            sut.GroupName = groupName;
            sut.CommValues = commValues;

            // Assert
            Assert.Equal(licenseeId, sut.LicenseeId);
            Assert.Equal(groupCommId, sut.GroupCommId);
            Assert.Equal(groupName, sut.GroupName);
            Assert.Equal(commValues, sut.CommValues);
        }

        [Fact]
        public void DefaultValues_ShouldBeZeroOrNull_WhenCreated()
        {
            // Arrange & Act
            var sut = new PlayerCommissionViewModel();

            // Assert
            Assert.Equal(0, sut.LicenseeId);
            Assert.Equal(0, sut.GroupCommId);
            Assert.Null(sut.GroupName);
            Assert.Null(sut.CommValues);
        }

        [Fact]
        public void LicenseeId_ShouldAcceptPositiveValues_WhenAssigned()
        {
            // Arrange
            var sut = new PlayerCommissionViewModel();
            var licenseeId = 12345;

            // Act
            sut.LicenseeId = licenseeId;

            // Assert
            Assert.Equal(licenseeId, sut.LicenseeId);
        }

        [Fact]
        public void GroupCommId_ShouldAcceptPositiveValues_WhenAssigned()
        {
            // Arrange
            var sut = new PlayerCommissionViewModel();
            var groupCommId = 67890;

            // Act
            sut.GroupCommId = groupCommId;

            // Assert
            Assert.Equal(groupCommId, sut.GroupCommId);
        }

        [Fact]
        public void GroupName_ShouldAcceptStringValues_WhenAssigned()
        {
            // Arrange
            var sut = new PlayerCommissionViewModel();
            var groupName = "Test Commission Group";

            // Act
            sut.GroupName = groupName;

            // Assert
            Assert.Equal(groupName, sut.GroupName);
        }

        [Fact]
        public void GroupName_ShouldAcceptNullValue_WhenAssigned()
        {
            // Arrange
            var sut = new PlayerCommissionViewModel();

            // Act
            sut.GroupName = null;

            // Assert
            Assert.Null(sut.GroupName);
        }

        [Fact]
        public void GroupName_ShouldAcceptEmptyString_WhenAssigned()
        {
            // Arrange
            var sut = new PlayerCommissionViewModel();

            // Act
            sut.GroupName = string.Empty;

            // Assert
            Assert.Equal(string.Empty, sut.GroupName);
        }

        [Fact]
        public void CommValues_ShouldAcceptEmptyCollection_WhenAssigned()
        {
            // Arrange
            var sut = new PlayerCommissionViewModel();
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
            var sut = new PlayerCommissionViewModel();

            // Act
            sut.CommValues = null;

            // Assert
            Assert.Null(sut.CommValues);
        }

        [Fact]
        public void CommValues_ShouldAcceptMultipleItems_WhenAssigned()
        {
            // Arrange
            var sut = new PlayerCommissionViewModel();
            var commValues = new List<PlayerCommissionSettingViewModel>
            {
                new() { ProductCode = "SB", Discount = 0.05m },
                new() { ProductCode = "NG", Discount = 0.10m },
                new() { ProductCode = "CS", Discount = 0.15m }
            };

            // Act
            sut.CommValues = commValues;

            // Assert
            Assert.Equal(3, sut.CommValues.Count());
            Assert.Equal(commValues, sut.CommValues);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void LicenseeId_ShouldAcceptVariousIntegerValues_WhenAssigned(int licenseeId)
        {
            // Arrange
            var sut = new PlayerCommissionViewModel();

            // Act
            sut.LicenseeId = licenseeId;

            // Assert
            Assert.Equal(licenseeId, sut.LicenseeId);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        public void GroupCommId_ShouldAcceptVariousIntegerValues_WhenAssigned(int groupCommId)
        {
            // Arrange
            var sut = new PlayerCommissionViewModel();

            // Act
            sut.GroupCommId = groupCommId;

            // Assert
            Assert.Equal(groupCommId, sut.GroupCommId);
        }
    }
}