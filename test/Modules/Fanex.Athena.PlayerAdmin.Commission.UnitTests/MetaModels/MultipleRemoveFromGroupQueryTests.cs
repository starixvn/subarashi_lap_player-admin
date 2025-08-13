namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.MetaModels
{
    using AutoFixture;
    using Commission.MetaModels;

    public class MultipleRemoveFromGroupQueryTests
    {
        private readonly Fixture _fixture;

        public MultipleRemoveFromGroupQueryTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void IsRemovePlayerComm_ShouldBeSettableAndGettable_WhenAssigned()
        {
            // Arrange
            var sut = new MultipleRemoveFromGroupQuery();
            var isRemovePlayerComm = _fixture.Create<bool>();

            // Act
            sut.IsRemovePlayerComm = isRemovePlayerComm;

            // Assert
            Assert.Equal(isRemovePlayerComm, sut.IsRemovePlayerComm);
        }

        [Fact]
        public void MultipleRemoveFromGroupQuery_ShouldInheritFromBasePlayerCommissionUpdateQuery_WhenCreated()
        {
            // Arrange & Act
            var sut = new MultipleRemoveFromGroupQuery();

            // Assert
            Assert.IsAssignableFrom<BasePlayerCommissionUpdateQuery>(sut);
        }

        [Fact]
        public void Properties_ShouldBeInheritedFromBase_WhenAccessed()
        {
            // Arrange
            var sut = new MultipleRemoveFromGroupQuery();
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
        [InlineData(true)]
        [InlineData(false)]
        public void IsRemovePlayerComm_ShouldAcceptBooleanValues_WhenAssigned(bool value)
        {
            // Arrange
            var sut = new MultipleRemoveFromGroupQuery();

            // Act
            sut.IsRemovePlayerComm = value;

            // Assert
            Assert.Equal(value, sut.IsRemovePlayerComm);
        }

        [Fact]
        public void IsValid_ShouldInheritFromBase_WhenCustIdsAreValid()
        {
            // Arrange
            var sut = new MultipleRemoveFromGroupQuery
            {
                CustIds = "1,2,3",
                IsRemovePlayerComm = _fixture.Create<bool>()
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
            var sut = new MultipleRemoveFromGroupQuery
            {
                CustIds = "1,2,invalid",
                IsRemovePlayerComm = _fixture.Create<bool>()
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.False(result);
        }
    }
}