namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.ViewModels
{
    using AutoFixture;
    using BatchUpdate.MetaModels;
    using BatchUpdate.ViewModels;

    public class BasePlayerBatchUpdateViewModelTests
    {
        private readonly Fixture _fixture;

        public BasePlayerBatchUpdateViewModelTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenMetaModelHasValues()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBatchUpdateMetaModel>();

            // Act
            var sut = new BasePlayerBatchUpdateViewModel(metaModel);

            // Assert
            Assert.Equal(metaModel.CustIds, sut.CustIdsStr);
            Assert.Equal(metaModel.CustNames, sut.CustNamesStr);
        }

        [Fact]
        public void Constructor_ShouldUseEmptyString_WhenCustIdsIsNull()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBatchUpdateMetaModel>();
            metaModel.CustIds = null;

            // Act
            var sut = new BasePlayerBatchUpdateViewModel(metaModel);

            // Assert
            Assert.Equal(string.Empty, sut.CustIdsStr);
        }

        [Fact]
        public void Constructor_ShouldUseEmptyString_WhenCustNamesIsNull()
        {
            // Arrange
            var metaModel = _fixture.Create<PlayerBatchUpdateMetaModel>();
            metaModel.CustNames = null;

            // Act
            var sut = new BasePlayerBatchUpdateViewModel(metaModel);

            // Assert
            Assert.Equal(string.Empty, sut.CustNamesStr);
        }
    }
}