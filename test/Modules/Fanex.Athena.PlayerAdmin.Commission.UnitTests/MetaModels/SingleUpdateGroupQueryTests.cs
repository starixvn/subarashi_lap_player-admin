namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.MetaModels
{
    using AutoFixture;
    using Commission.MetaModels;

    public class SingleUpdateGroupQueryTests
    {
        private readonly Fixture _fixture;

        public SingleUpdateGroupQueryTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void SingleUpdateGroupQuery_ShouldInheritFromSingleMoveGroupQuery_WhenCreated()
        {
            // Arrange & Act
            var sut = new SingleUpdateGroupQuery();

            // Assert
            Assert.IsAssignableFrom<SingleMoveGroupQuery>(sut);
        }

        [Fact]
        public void SingleUpdateGroupQuery_ShouldInheritFromBasePlayerCommissionUpdateQuery_WhenCreated()
        {
            // Arrange & Act
            var sut = new SingleUpdateGroupQuery();

            // Assert
            Assert.IsAssignableFrom<BasePlayerCommissionUpdateQuery>(sut);
        }

        [Fact]
        public void Properties_ShouldBeInheritedFromParent_WhenAccessed()
        {
            // Arrange
            var sut = new SingleUpdateGroupQuery();
            var custIds = _fixture.Create<string>();
            var licenseeId = _fixture.Create<int>();
            var custId = _fixture.Create<int>();

            // Act
            sut.CustIds = custIds;
            sut.LicenseeId = licenseeId;
            sut.CustId = custId;

            // Assert
            Assert.Equal(custIds, sut.CustIds);
            Assert.Equal(licenseeId, sut.LicenseeId);
            Assert.Equal(custId, sut.CustId);
        }

        [Fact]
        public void IsValid_ShouldInheritFromParent_WhenCustIdsAreValid()
        {
            // Arrange
            var sut = new SingleUpdateGroupQuery
            {
                CustIds = "1,2,3",
                CustId = _fixture.Create<int>()
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldInheritFromParent_WhenCustIdsAreInvalid()
        {
            // Arrange
            var sut = new SingleUpdateGroupQuery
            {
                CustIds = "1,2,invalid",
                CustId = _fixture.Create<int>()
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void SingleUpdateGroupQuery_ShouldHaveEmptyImplementation_WhenCreated()
        {
            // Arrange & Act
            var sut = new SingleUpdateGroupQuery();

            // Assert
            Assert.NotNull(sut);
            Assert.IsType<SingleUpdateGroupQuery>(sut);
        }
    }
}