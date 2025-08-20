namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.MetaModels
{
    using AutoFixture;
    using BatchUpdate.MetaModels;

    public class BasePlayerBatchUpdateApiMetaModelTests
    {
        private readonly Fixture _fixture;
        private readonly BasePlayerBatchUpdateApiMetaModel _sut;

        public BasePlayerBatchUpdateApiMetaModelTests()
        {
            _fixture = new Fixture();
            _sut = _fixture.Create<BasePlayerBatchUpdateApiMetaModel>();
        }

        [Fact]
        public void IsValid_ShouldReturnTrue_WhenCustIdsContainsValidIntegers()
        {
            // Arrange
            _sut.CustIds = "123,456,789";

            // Act
            var result = _sut.IsValid;

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCustIdsIsNull()
        {
            // Arrange
            _sut.CustIds = null;

            // Act
            var result = _sut.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCustIdsIsEmpty()
        {
            // Arrange
            _sut.CustIds = string.Empty;

            // Act
            var result = _sut.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCustIdsIsWhitespace()
        {
            // Arrange
            _sut.CustIds = "   ";

            // Act
            var result = _sut.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCustIdsContainsNonIntegers()
        {
            // Arrange
            _sut.CustIds = "123,abc,789";

            // Act
            var result = _sut.IsValid;

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsValid_ShouldReturnFalse_WhenCustIdsContainsEmptyValues()
        {
            // Arrange
            _sut.CustIds = "123,,789";

            // Act
            var result = _sut.IsValid;

            // Assert
            Assert.False(result);
        }
    }
}