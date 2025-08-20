namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Criteria
{
    using AutoFixture;
    using Commission.Criteria;

    public class UpdateCommToAPICriteriaTests
    {
        private readonly Fixture _fixture;
        private readonly UpdateCommToAPICriteria _sut;

        public UpdateCommToAPICriteriaTests()
        {
            _fixture = new Fixture();
            _sut = _fixture.Create<UpdateCommToAPICriteria>();
        }

        [Fact]
        public void CustomerIdList_ShouldBeSettable_WhenAssigned()
        {
            // Arrange
            var expectedCustomerIdList = _fixture.Create<string>();

            // Act
            _sut.CustomerIdList = expectedCustomerIdList;

            // Assert
            Assert.Equal(expectedCustomerIdList, _sut.CustomerIdList);
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
        public void IP_ShouldBeSettable_WhenAssigned()
        {
            // Arrange
            var expectedIP = _fixture.Create<string>();

            // Act
            _sut.IP = expectedIP;

            // Assert
            Assert.Equal(expectedIP, _sut.IP);
        }

        [Fact]
        public void UserId_ShouldBeSettable_WhenAssigned()
        {
            // Arrange
            var expectedUserId = _fixture.Create<int>();

            // Act
            _sut.UserId = expectedUserId;

            // Assert
            Assert.Equal(expectedUserId, _sut.UserId);
        }

        [Fact]
        public void Username_ShouldBeSettable_WhenAssigned()
        {
            // Arrange
            var expectedUsername = _fixture.Create<string>();

            // Act
            _sut.Username = expectedUsername;

            // Assert
            Assert.Equal(expectedUsername, _sut.Username);
        }
    }
}