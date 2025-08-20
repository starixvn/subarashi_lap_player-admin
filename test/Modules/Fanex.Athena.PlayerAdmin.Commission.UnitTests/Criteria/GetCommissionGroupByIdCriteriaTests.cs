namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Criteria
{
    using AutoFixture;
    using Commission.Criteria;
    using Commission.MetaModels;
    using Data;
    using Moq;

    public class GetCommissionGroupByIdCriteriaTests
    {
        private readonly Fixture _fixture;
        private readonly MultipleMoveGroupQuery _query;
        private readonly GetCommissionGroupByIdCriteria _sut;

        public GetCommissionGroupByIdCriteriaTests()
        {
            _fixture = new Fixture();
            _query = _fixture.Create<MultipleMoveGroupQuery>();
            _sut = new GetCommissionGroupByIdCriteria(_query);
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenCalled()
        {
            // Assert
            Assert.Equal(_sut.LicenseeId, _query.LicenseeId);
            Assert.Equal(_sut.GroupCommId, _query.GroupCommId);
        }

        [Fact]
        public void SPName_ShouldReturnCorrectValue_WhenCalled()
        {
            // Arrange
            const string expected = "Lrf_GroupCommGetById";
            // Assert
            Assert.Equal(expected, _sut.SPName);
        }

        [Fact]
        public void SetParameterValues_ShouldSetAllParameters_WhenCalled()
        {
            // Arrange
            var db = new Mock<IObjectDb>();

            // Act
            _sut.SetParameterValues(db.Object);

            // Assert
            db.Verify(x => x.SetParameterValue("LicenseeID", _query.LicenseeId), Times.Once);
            db.Verify(x => x.SetParameterValue("GroupDiscountID", _query.GroupCommId), Times.Once);
        }
    }
}