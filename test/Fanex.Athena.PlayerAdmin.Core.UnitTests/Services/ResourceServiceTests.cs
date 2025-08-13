namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    using System.Globalization;
    using Core.Services;
    using Moq;
    using RemotingRef;

    public class ResourceServiceTests
    {
        private readonly Mock<IRemotingRefService> _mockRemotingRefService;
        private readonly ResourceService _service;

        public ResourceServiceTests()
        {
            _mockRemotingRefService = new Mock<IRemotingRefService>();
            _service = new ResourceService(_mockRemotingRefService.Object);
        }

        [Fact]
        public void GetBettype_InvalidIds_ReturnsEmptyString()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            Assert.Equal(string.Empty, _service.GetBettype(null));
            Assert.Equal(string.Empty, _service.GetBettype(""));
            Assert.Equal(string.Empty, _service.GetBettype("0"));
            Assert.Equal(string.Empty, _service.GetBettype(0));
        }

        [Fact]
        public void GetBettype_ValidId_CallsRemotingRefAndReturnsValue()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            _mockRemotingRefService
                .Setup(x => x.GetBetTypeName("123", "en-US"))
                .Returns("BetTypeName");

            var result = _service.GetBettype(123);

            Assert.Equal("BetTypeName", result);
            _mockRemotingRefService.Verify(x => x.GetBetTypeName("123", "en-US"), Times.Once);
        }

        [Fact]
        public void GettResourceData_InvalidIds_ReturnsEmptyString()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            Assert.Equal(string.Empty, _service.GettResourceData(null, 1));
            Assert.Equal(string.Empty, _service.GettResourceData(1, null));
            Assert.Equal(string.Empty, _service.GettResourceData("0", 2));
            Assert.Equal(string.Empty, _service.GettResourceData(3, "0"));
        }

        [Fact]
        public void GettResourceData_ValidIds_CallsRemotingRefAndReturnsValue()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            _mockRemotingRefService
                .Setup(x => x.GetResourceDataByRefIdAndResourceId("10", "20", "en-US", string.Empty))
                .Returns("ResourceData");

            var result = _service.GettResourceData(10, 20);

            Assert.Equal("ResourceData", result);
            _mockRemotingRefService.Verify(x => x.GetResourceDataByRefIdAndResourceId("10", "20", "en-US", string.Empty), Times.Once);
        }

        [Fact]
        public void GetSportName_InvalidId_ReturnsEmptyString()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            Assert.Equal(string.Empty, _service.GetSportName(null));
            Assert.Equal(string.Empty, _service.GetSportName(""));
            Assert.Equal(string.Empty, _service.GetSportName("0"));
        }

        [Fact]
        public void GetSportName_ValidIdWithLanguage_UsesProvidedLanguage()
        {
            _mockRemotingRefService
                .Setup(x => x.GetSportName("5", "zh-TW", string.Empty))
                .Returns("SportNameTW");

            var result = _service.GetSportName(5, "zh-TW");

            Assert.Equal("SportNameTW", result);
            _mockRemotingRefService.Verify(x => x.GetSportName("5", "zh-TW", string.Empty), Times.Once);
        }

        [Fact]
        public void GetSportName_ValidIdWithoutLanguage_UsesCurrentUICulture()
        {
            CultureInfo.CurrentUICulture = new CultureInfo("en-US");
            _mockRemotingRefService
                .Setup(x => x.GetSportName("7", "en-US", string.Empty))
                .Returns("SportNameUS");

            var result = _service.GetSportName(7);

            Assert.Equal("SportNameUS", result);
            _mockRemotingRefService.Verify(x => x.GetSportName("7", "en-US", string.Empty), Times.Once);
        }
    }
}

