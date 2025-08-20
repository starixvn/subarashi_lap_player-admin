namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.MetaModels
{
    using AutoFixture;
    using Commission.MetaModels;

    public class BasePlayerCommissionUpdateQueryTests
    {
        private readonly Fixture _fixture;

        public BasePlayerCommissionUpdateQueryTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Properties_ShouldBeSettableAndGettable_WhenAssigned()
        {
            // Arrange
            var sut = new BasePlayerCommissionUpdateQuery();
            var custIds = _fixture.Create<string>();
            var custNames = _fixture.Create<string>();
            var licenseeId = _fixture.Create<int>();
            var userId = _fixture.Create<int>();
            var ip = _fixture.Create<string>();
            var language = _fixture.Create<string>();
            var username = _fixture.Create<string>();
            var custIdsStr = _fixture.Create<string>();

            // Act
            sut.CustIds = custIds;
            sut.CustNames = custNames;
            sut.LicenseeId = licenseeId;
            sut.UserId = userId;
            sut.Ip = ip;
            sut.Language = language;
            sut.Username = username;
            sut.CustIdsStr = custIdsStr;

            // Assert
            Assert.Equal(custIds, sut.CustIds);
            Assert.Equal(custNames, sut.CustNames);
            Assert.Equal(licenseeId, sut.LicenseeId);
            Assert.Equal(userId, sut.UserId);
            Assert.Equal(ip, sut.Ip);
            Assert.Equal(language, sut.Language);
            Assert.Equal(username, sut.Username);
            Assert.Equal(custIdsStr, sut.CustIdsStr);
        }

        [Theory]
        [InlineData("1,2,3", true)]
        [InlineData("123", true)]
        [InlineData("1,2,abc", false)]
        [InlineData("", false)]
        [InlineData(null, false)]
        [InlineData("  ", false)]
        public void IsValid_ShouldReturnExpectedResult_WhenCustIdsProvided(string custIds, bool expectedResult)
        {
            // Arrange
            var sut = new BasePlayerCommissionUpdateQuery
            {
                CustIds = custIds
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenAllCustIdsAreValidIntegers()
        {
            // Arrange
            var sut = new BasePlayerCommissionUpdateQuery
            {
                CustIds = "1,2,3,4,5"
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenAnyCustIdIsNotInteger()
        {
            // Arrange
            var sut = new BasePlayerCommissionUpdateQuery
            {
                CustIds = "1,2,invalid,4"
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCustIdsContainsSpaces()
        {
            // Arrange
            var sut = new BasePlayerCommissionUpdateQuery
            {
                CustIds = "1, 2, 3"
            };

            // Act
            var result = sut.IsValid;

            // Assert
            Assert.False(result);
        }
    }
}