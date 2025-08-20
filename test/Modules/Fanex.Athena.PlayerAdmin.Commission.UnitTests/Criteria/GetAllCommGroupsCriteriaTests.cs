namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Criteria
{
    using AutoFixture;
    using Commission.Criteria;
    using Commission.MetaModels;
    using Data;
    using Moq;

    public class GetAllCommGroupsCriteriaTests
    {
        private readonly Fixture _fixture;
        private readonly BasePlayerCommissionUpdateQuery _query;
        private readonly GetAllCommGroupsCriteria _sut;

        public GetAllCommGroupsCriteriaTests()
        {
            _fixture = new Fixture();
            _query = _fixture.Create<BasePlayerCommissionUpdateQuery>();
            _sut = new GetAllCommGroupsCriteria(_query);
        }

        [Fact]
        public void Constructor_ShouldInitializeLicenseeId_WhenCalled()
        {
            // Assert
            Assert.Equal(_sut.LicenseeId, _query.LicenseeId);
        }

        [Fact]
        public void SPName_ShouldReturnCorrectValue_WhenCalled()
        {
            // Arrange
            const string expectedSpName = "Lrf_LicenseeGroupCommSel";

            // Assert
            Assert.Equal(expectedSpName, _sut.SPName);
        }

        [Fact]
        public void SetParameterValues_ShouldSetLicenseeIdParameter_WhenCalled()
        {
            // Arrange
            var db = new Mock<IObjectDb>();

            // Act
            _sut.SetParameterValues(db.Object);

            // Assert
            db.Verify(x => x.SetParameterValue("LicenseeID", _query.LicenseeId), Times.Once);
        }
    }
}