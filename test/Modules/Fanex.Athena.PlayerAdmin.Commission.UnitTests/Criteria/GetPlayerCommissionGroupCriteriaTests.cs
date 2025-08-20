namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Criteria
{
    using AutoFixture;
    using Commission.Criteria;
    using Commission.MetaModels;
    using Data;
    using Moq;

    public class GetPlayerCommissionGroupCriteriaTests
    {
        private readonly Fixture _fixture;
        private readonly SingleMoveGroupQuery _query;
        private readonly GetPlayerCommissionGroupCriteria _sut;

        public GetPlayerCommissionGroupCriteriaTests()
        {
            _fixture = new Fixture();
            _query = _fixture.Create<SingleMoveGroupQuery>();
            _sut = new GetPlayerCommissionGroupCriteria(_query);
        }

        [Fact]
        public void Constructor_ShouldInitializeCustId_WhenCalled()
        {
            // Assert
            Assert.Equal(_sut.CustId, _query.CustId);
        }

        [Fact]
        public void SPName_ShouldReturnCorrectValue_WhenCalled()
        {
            // Arrange
            const string expected = "Lrf_LicenseeCustCommSel";

            // Assert
            Assert.Equal(expected, _sut.SPName);
        }

        [Fact]
        public void SetParameterValues_ShouldSetCustIdParameter_WhenCalled()
        {
            // Arrange
            var db = new Mock<IObjectDb>();

            // Act
            _sut.SetParameterValues(db.Object);

            // Assert
            db.Verify(x => x.SetParameterValue("CustId", _query.CustId), Times.Once);
        }
    }
}