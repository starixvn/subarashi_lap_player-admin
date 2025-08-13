namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Criteria
{
    using AutoFixture;
    using BatchUpdate.Criteria;
    using BatchUpdate.MetaModels;
    using Data;
    using Moq;

    public class UpdateSpecialLevelCriteriaTests
    {
        private readonly string _custId;
        private readonly Fixture _fixture;
        private readonly UpdateSpecialLevelApiMetaModel _metaModel;
        private readonly UpdateSpecialLevelCriteria _sut;

        public UpdateSpecialLevelCriteriaTests()
        {
            _fixture = new Fixture();
            _metaModel = _fixture.Create<UpdateSpecialLevelApiMetaModel>();
            _custId = _fixture.Create<string>();
            _sut = new UpdateSpecialLevelCriteria(_metaModel, _custId);
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenCalled()
        {
            // Assert
            Assert.Equal(_metaModel.SpecialLevelId, _sut.SpecialLevelId);
            Assert.Equal(_metaModel.LicenseeId, _sut.LicenseeId);
            Assert.Equal(_custId, _sut.CustId);
        }

        [Fact]
        public void GetSPName_ShouldReturnCorrectStoredProcedure_WhenCalled()
        {
            // Act
            var result = _sut.GetSPName();

            // Assert
            Assert.Equal("Lrf_CustomerUpdSpecialLevel", result);
        }

        [Fact]
        public void SetParameterValues_ShouldSetAllParameters_WhenCalled()
        {
            // Arrange
            var dbMock = new Mock<IObjectDb>();

            // Act
            _sut.SetParameterValues(dbMock.Object);

            // Assert
            dbMock.Verify(x => x.SetParameterValue("CustId", _custId), Times.Once);
            dbMock.Verify(x => x.SetParameterValue("Ip", _metaModel.Ip), Times.Once);
            dbMock.Verify(x => x.SetParameterValue("langkey", _metaModel.Language), Times.Once);
            dbMock.Verify(x => x.SetParameterValue("ErrCode", null), Times.Once);
            dbMock.Verify(x => x.SetParameterValue("ErrMsg", null), Times.Once);
            dbMock.Verify(x => x.SetParameterValue("UserId", _metaModel.UserId), Times.Once);
            dbMock.Verify(x => x.SetParameterValue("SpecialLevel", _metaModel.SpecialLevelId), Times.Once);
        }
    }
}