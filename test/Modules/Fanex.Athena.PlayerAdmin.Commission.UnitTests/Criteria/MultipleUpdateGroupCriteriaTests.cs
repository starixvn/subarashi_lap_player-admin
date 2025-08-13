namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Criteria
{
    using AutoFixture;
    using Commission.Criteria;
    using Commission.MetaModels;
    using Commission.ViewModels;
    using Data;
    using Moq;

    public class MultipleUpdateGroupCriteriaTests
    {
        private readonly Fixture _fixture;
        private readonly MultipleUpdateGroupQuery _query;
        private readonly MultipleUpdateGroupCriteria _sut;

        public MultipleUpdateGroupCriteriaTests()
        {
            _fixture = new Fixture();
            _query = _fixture.Create<MultipleUpdateGroupQuery>();
            _sut = new MultipleUpdateGroupCriteria(_query);
        }

        [Fact]
        public void Constructor_ShouldInitializeCommValues_WhenCalled()
        {
            // Assert
            Assert.Equal(_query.CommValues, _sut.CommValues);
        }

        [Fact]
        public void GetSPName_ShouldReturnCorrectValue_WhenCalled()
        {
            // Act
            var result = _sut.GetSPName();

            // Assert
            Assert.Equal("Lrf_LicenseeCustCommUpd", result);
        }

        [Fact]
        public void SetParameterValues_ShouldCallBaseAndSetDiscountSettings_WhenCalled()
        {
            // Arrange
            var mockDb = new Mock<IObjectDb>();
            var db = mockDb.Object;
            var commValues = new List<PlayerCommissionSettingViewModel>
            {
                new() { Sporttype = 1, Bettype = 1, Discount = 0.05m },
                new() { Sporttype = 2, Bettype = 2, Discount = 0.10m }
            };
            var query = new MultipleUpdateGroupQuery { CommValues = commValues };
            query.LicenseeId = 123;
            query.UserId = 456;
            query.Ip = "192.168.1.1";
            query.Language = "en";
            query.CustIds = "1,2,3";

            var sut = new MultipleUpdateGroupCriteria(query);

            // Act
            sut.SetParameterValues(db);

            // Assert
            mockDb.Verify(x => x.SetParameterValue("LicenseeId", query.LicenseeId), Times.Once);
            mockDb.Verify(x => x.SetParameterValue("UserId", query.UserId), Times.Once);
            mockDb.Verify(x => x.SetParameterValue("Ip", query.Ip), Times.Once);
            mockDb.Verify(x => x.SetParameterValue("langkey", query.Language), Times.Once);
            mockDb.Verify(x => x.SetParameterValue("strCustIDOriginal", query.CustIds), Times.Once);
            mockDb.Verify(x => x.SetParameterValue("ErrCode", null), Times.Once);
            mockDb.Verify(x => x.SetParameterValue("ErrMsg", null), Times.Once);
            mockDb.Verify(x => x.SetParameterValue("DiscountSettings", It.IsAny<string>()), Times.Once);
        }

        [Theory]
        [InlineData(1, 2, 0.05)]
        [InlineData(3, 4, 0.10)]
        public void BuildDiscountSettingsXmlString_ShouldGenerateValidXml_WhenCommValuesProvided(int sportType, int betType, decimal discount)
        {
            // Arrange
            var commValues = new List<PlayerCommissionSettingViewModel>
            {
                new() { Sporttype = sportType, Bettype = betType, Discount = discount }
            };
            var query = new MultipleUpdateGroupQuery { CommValues = commValues };
            var sut = new MultipleUpdateGroupCriteria(query);
            var mockDb = new Mock<IObjectDb>();
            var db = mockDb.Object;

            // Act
            sut.SetParameterValues(db);

            // Assert
            mockDb.Verify(x => x.SetParameterValue("DiscountSettings",
                It.Is<string>(xml => xml.Contains($"SportType=\"{sportType}\"") &&
                                     xml.Contains($"BetType=\"{betType}\"") &&
                                     xml.Contains($"Discount=\"{discount}\"") &&
                                     xml.StartsWith("<Root>") &&
                                     xml.EndsWith("</Root>"))), Times.Once);
        }
    }
}