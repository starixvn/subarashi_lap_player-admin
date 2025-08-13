namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Criteria
{
    using AutoFixture;
    using Commission.Criteria;
    using Commission.MetaModels;
    using Data;
    using Moq;

    public class MultipleMoveGroupCriteriaTests
    {
        private readonly Fixture _fixture;
        private readonly MultipleMoveGroupQuery _query;
        private readonly MultipleMoveGroupCriteria _sut;

        public MultipleMoveGroupCriteriaTests()
        {
            _fixture = new Fixture();
            _query = _fixture.Create<MultipleMoveGroupQuery>();
            _sut = new MultipleMoveGroupCriteria(_query);
        }

        [Fact]
        public void Constructor_ShouldInitializeGroupCommId_WhenCalled()
        {
            // Assert
            Assert.Equal(_sut.GroupCommId, _query.GroupCommId);
        }

        [Fact]
        public void GetSPName_ShouldReturnCorrectValue_WhenCalled()
        {
            // Act
            const string expected = "Lrf_CustomerMoveGroupComm";
            var result = _sut.GetSPName();

            // Assert
            Assert.Equal(expected, result);
        }

        [Fact]
        public void SetParameterValues_ShouldCallBaseAndSetGroupDiscountId_WhenCalled()
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
            db.Verify(x => x.SetParameterValue("GroupDiscountID", _query.GroupCommId), Times.Once);
        }
    }
}