using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.RemotingRef;
using NSubstitute;
using NUnit.Framework;
using System.Globalization;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    public class ResourceServiceTests
    {
        private IRemotingRefService _remotingRefService;

        [SetUp]
        public void SetUp()
        {
            _remotingRefService = Substitute.For<IRemotingRefService>();
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("0")]
        public void GetBettype_WhenNotValidBetTypeId_ReturnEmptyString(string bettypeId)
        {
            //Arrange
            var service = CreateService();

            //Act
            var result = service.GetBettype(bettypeId);

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetBettype_WhenValidBettype_CallRelevantMethod()
        {
            //Arrange
            var bettype = "1";
            var service = CreateService();

            //Act
            service.GetBettype(bettype);

            //Assert
            _remotingRefService.Received(1).GetBetTypeName(bettype, CultureInfo.CurrentUICulture.Name);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("0")]
        public void GetSportName_WhenNotValidBetTypeId_ReturnEmptyString(string sportId)
        {
            //Arrange
            var service = CreateService();

            //Act
            var result = service.GetSportName(sportId);

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void GetSportName_WhenValidBettype_CallRelevantMethod()
        {
            //Arrange
            var sportId = "1";
            var language = "en-US";
            var service = CreateService();

            //Act
            service.GetSportName(sportId, language);

            //Assert
            _remotingRefService.Received(1).GetSportName(sportId, language, string.Empty);
        }

        private ResourceService CreateService()
        {
            return new ResourceService(_remotingRefService);
        }
    }
}
