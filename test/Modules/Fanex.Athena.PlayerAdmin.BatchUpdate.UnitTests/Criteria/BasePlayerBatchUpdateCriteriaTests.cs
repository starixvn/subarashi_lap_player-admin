namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests.Criteria
{
    using AutoFixture;
    using BatchUpdate.Criteria;
    using BatchUpdate.MetaModels;
    using Data;
    using Moq;

    public class BasePlayerBatchUpdateCriteriaTests
    {
        private readonly string _custId;
        private readonly Fixture _fixture;
        private readonly BasePlayerBatchUpdateApiMetaModel _metaModel;
        private readonly BasePlayerBatchUpdateCriteria _sut;

        public BasePlayerBatchUpdateCriteriaTests()
        {
            _fixture = new Fixture();
            _metaModel = _fixture.Create<BasePlayerBatchUpdateApiMetaModel>();
            _custId = _fixture.Create<string>();
            _sut = new BasePlayerBatchUpdateCriteria(_metaModel, _custId);
        }

        [Fact]
        public void Constructor_ShouldInitializeProperties_WhenCalled()
        {
            // Assert
            Assert.Equal(_metaModel.LicenseeId, _sut.LicenseeId);
            Assert.Equal(_metaModel.SiteName, _sut.SiteName);
            Assert.Equal(_custId, _sut.CustId);
            Assert.Equal(_metaModel.UserId, _sut.UserId);
            Assert.Equal(_metaModel.Ip, _sut.Ip);
            Assert.Equal(_metaModel.Language, _sut.Language);
        }

        [Fact]
        public void GetSPName_ShouldReturnEmptyString_WhenCalled()
        {
            // Act
            var result = _sut.GetSPName();

            // Assert
            Assert.Equal(string.Empty, result);
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
        }
    }
}