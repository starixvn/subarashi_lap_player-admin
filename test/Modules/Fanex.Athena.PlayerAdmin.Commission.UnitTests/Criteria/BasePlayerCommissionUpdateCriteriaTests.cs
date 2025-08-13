namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Criteria
{
    using AutoFixture;
    using Commission.Criteria;
    using Commission.MetaModels;
    using Data;
    using Moq;

    public class BasePlayerCommissionUpdateCriteriaTests
    {
        private readonly Fixture _fixture;
        private readonly BasePlayerCommissionUpdateQuery _query;
        private readonly BasePlayerCommissionUpdateCriteria _sut;

        public BasePlayerCommissionUpdateCriteriaTests()
        {
            _fixture = new Fixture();
            _query = _fixture.Create<BasePlayerCommissionUpdateQuery>();
            _sut = new BasePlayerCommissionUpdateCriteria(_query);
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenCalled()
        {
            // Assert
            Assert.Equal(_sut.LicenseeId, _query.LicenseeId);
            Assert.Equal(_sut.CustIds, _query.CustIds);
            Assert.Equal(_sut.UserId, _query.UserId);
            Assert.Equal(_sut.IP, _query.Ip);
            Assert.Equal(_sut.Language, _query.Language);
        }

        [Fact]
        public void GetSPName_ShouldReturnEmptyString_WhenCalled()
        {
            // Act
            var result = _sut.GetSPName();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void SetParameterValues_ShouldSetAllParameters_WhenCalled()
        {
            // Arrange
            var db = new Mock<IObjectDb>();

            // Act
            _sut.SetParameterValues(db.Object);

            // Assert
            db.Verify(x => x.SetParameterValue("LicenseeId", _query.LicenseeId), Times.Once);
            db.Verify(x => x.SetParameterValue("UserId", _query.UserId), Times.Once);
            db.Verify(x => x.SetParameterValue("Ip", _query.Ip), Times.Once);
            db.Verify(x => x.SetParameterValue("langkey", _query.Language), Times.Once);
            db.Verify(x => x.SetParameterValue("strCustIDOriginal", _query.CustIds), Times.Once);
            db.Verify(x => x.SetParameterValue("ErrCode", null), Times.Once);
            db.Verify(x => x.SetParameterValue("ErrMsg", null), Times.Once);
        }
    }
}