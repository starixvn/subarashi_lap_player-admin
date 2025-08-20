namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Criteria
{
    using AutoFixture;
    using Commission.Criteria;

    public class RNGCasinoUpdateMemberComCriteriaTests
    {
        private readonly Fixture _fixture;
        private readonly RNGCasinoUpdateMemberComCriteria _sut;

        public RNGCasinoUpdateMemberComCriteriaTests()
        {
            _fixture = new Fixture();
            _sut = _fixture.Create<RNGCasinoUpdateMemberComCriteria>();
        }

        [Fact]
        public void Comm_ShouldBeSettable_WhenAssigned()
        {
            // Arrange
            var expectedComm = _fixture.Create<decimal>();

            // Act
            _sut.Comm = expectedComm;

            // Assert
            Assert.Equal(expectedComm, _sut.Comm);
        }

        [Fact]
        public void CustomerId_ShouldBeSettable_WhenAssigned()
        {
            // Arrange
            var expectedCustomerId = _fixture.Create<string[]>();

            // Act
            _sut.CustomerId = expectedCustomerId;

            // Assert
            Assert.Equal(expectedCustomerId, _sut.CustomerId);
        }

        [Fact]
        public void FromIp_ShouldBeSettable_WhenAssigned()
        {
            // Arrange
            var expectedFromIp = _fixture.Create<string>();

            // Act
            _sut.FromIp = expectedFromIp;

            // Assert
            Assert.Equal(expectedFromIp, _sut.FromIp);
        }

        [Fact]
        public void ModifyBy_ShouldBeSettable_WhenAssigned()
        {
            // Arrange
            var expectedModifyBy = _fixture.Create<int>();

            // Act
            _sut.ModifyBy = expectedModifyBy;

            // Assert
            Assert.Equal(expectedModifyBy, _sut.ModifyBy);
        }

        [Fact]
        public void ModifyUserName_ShouldBeSettable_WhenAssigned()
        {
            // Arrange
            var expectedModifyUserName = _fixture.Create<string>();

            // Act
            _sut.ModifyUserName = expectedModifyUserName;

            // Assert
            Assert.Equal(expectedModifyUserName, _sut.ModifyUserName);
        }
    }
}