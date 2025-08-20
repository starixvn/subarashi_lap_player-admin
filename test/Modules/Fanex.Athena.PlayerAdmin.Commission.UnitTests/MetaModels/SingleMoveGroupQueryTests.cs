namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.MetaModels
{
    using AutoFixture;
    using Commission.MetaModels;

    public class SingleMoveGroupQueryTests
    {
        private readonly Fixture _fixture;

        public SingleMoveGroupQueryTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void CustId_ShouldBeSettableAndGettable_WhenAssigned()
        {
            // Arrange
            var sut = new SingleMoveGroupQuery();
            var custId = _fixture.Create<int>();

            // Act
            sut.CustId = custId;

            // Assert
            Assert.Equal(custId, sut.CustId);
        }

        [Fact]
        public void SingleMoveGroupQuery_ShouldInheritFromBasePlayerCommissionUpdateQuery_WhenCreated()
        {
            // Arrange & Act
            var sut = new SingleMoveGroupQuery();

            // Assert
            Assert.IsAssignableFrom<BasePlayerCommissionUpdateQuery>(sut);
        }

        [Fact]
        public void Properties_ShouldBeInheritedFromBase_WhenAccessed()
        {
            // Arrange
            var sut = new SingleMoveGroupQuery();
            var custIds = _fixture.Create<string>();
            var licenseeId = _fixture.Create<int>();

            // Act
            sut.CustIds = custIds;
            sut.LicenseeId = licenseeId;

            // Assert
            Assert.Equal(custIds, sut.CustIds);
            Assert.Equal(licenseeId, sut.LicenseeId);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(100)]
        [InlineData(999999)]
        public void CustId_ShouldAcceptValidIntegerValues_WhenAssigned(int custId)
        {
            // Arrange
            var sut = new SingleMoveGroupQuery();

            // Act
            sut.CustId = custId;

            // Assert
            Assert.Equal(custId, sut.CustId);
        }

        [Fact]
        public void IsValid_ShouldInheritFromBase_WhenCustIdsAreValid()
        {
            // Arrange
            var sut = new SingleMoveGroupQuery
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
        public void IsValid_ShouldInheritFromBase_WhenCustIdsAreInvalid()
        {
            // Arrange
            var sut = new SingleMoveGroupQuery
            {
                CustIds = "1,2,invalid",
                CustId = _fixture.Create<int>()
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.False(result);
        }
    }
}