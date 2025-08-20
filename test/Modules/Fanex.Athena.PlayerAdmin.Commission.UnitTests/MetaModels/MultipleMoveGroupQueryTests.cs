namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.MetaModels
{
    using AutoFixture;
    using Commission.MetaModels;

    public class MultipleMoveGroupQueryTests
    {
        private readonly Fixture _fixture;

        public MultipleMoveGroupQueryTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void GroupCommId_ShouldBeSettableAndGettable_WhenAssigned()
        {
            // Arrange
            var sut = new MultipleMoveGroupQuery();
            var groupCommId = _fixture.Create<string>();

            // Act
            sut.GroupCommId = groupCommId;

            // Assert
            Assert.Equal(groupCommId, sut.GroupCommId);
        }

        [Fact]
        public void MultipleMoveGroupQuery_ShouldInheritFromBasePlayerCommissionUpdateQuery_WhenCreated()
        {
            // Arrange & Act
            var sut = new MultipleMoveGroupQuery();

            // Assert
            Assert.IsAssignableFrom<BasePlayerCommissionUpdateQuery>(sut);
        }

        [Fact]
        public void Properties_ShouldBeInheritedFromBase_WhenAccessed()
        {
            // Arrange
            var sut = new MultipleMoveGroupQuery();
            var custIds = _fixture.Create<string>();
            var licenseeId = _fixture.Create<int>();

            // Act
            sut.CustIds = custIds;
            sut.LicenseeId = licenseeId;

            // Assert
            Assert.Equal(custIds, sut.CustIds);
            Assert.Equal(licenseeId, sut.LicenseeId);
        }

        [Fact]
        public void IsValid_ShouldInheritFromBase_WhenCustIdsAreValid()
        {
            // Arrange
            var sut = new MultipleMoveGroupQuery
            {
                CustIds = "1,2,3",
                GroupCommId = _fixture.Create<string>()
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldInheritFromBase_WhenCustIdsAreInvalid()
        {
            // Arrange
            var sut = new MultipleMoveGroupQuery
            {
                CustIds = "1,2,invalid",
                GroupCommId = _fixture.Create<string>()
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.False(result);
        }
    }
}