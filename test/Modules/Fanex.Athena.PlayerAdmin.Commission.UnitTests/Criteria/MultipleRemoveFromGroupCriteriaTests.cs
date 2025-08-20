namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Criteria
{
    using AutoFixture;
    using Commission.Criteria;
    using Commission.MetaModels;
    using Data;
    using Moq;

    public class MultipleRemoveFromGroupCriteriaTests
    {
        private readonly Fixture _fixture;
        private readonly MultipleRemoveFromGroupQuery _query;
        private readonly MultipleRemoveFromGroupCriteria _sut;

        public MultipleRemoveFromGroupCriteriaTests()
        {
            _fixture = new Fixture();
            _query = _fixture.Create<MultipleRemoveFromGroupQuery>();
            _sut = new MultipleRemoveFromGroupCriteria(_query);
        }

        [Fact]
        public void Constructor_ShouldInitializeIsRemovePlayerComm_WhenCalled()
        {
            // Assert
            Assert.Equal(_sut.IsRemovePlayerComm, _query.IsRemovePlayerComm);
        }

        [Fact]
        public void GetSPName_ShouldReturnCorrectValue_WhenCalled()
        {
            // Act
            var result = _sut.GetSPName();
            const string expected = "Lrf_CustomerRemoveGroupComm";

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SetParameterValues_ShouldCallBaseAndSetIsRemovePlayerComm_WhenCalled()
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
            db.Verify(x => x.SetParameterValue("IsRemovePlayercomm", _query.IsRemovePlayerComm), Times.Once);
        }
    }
}