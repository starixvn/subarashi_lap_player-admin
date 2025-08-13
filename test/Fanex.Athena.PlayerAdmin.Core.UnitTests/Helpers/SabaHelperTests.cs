namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Helpers
{
    using Core.Helpers;
    using DataDomains;

    public class SabaHelperTests
    {
        [Fact]
        public void IsHideSaba_ReturnsTrue_ForJollysports()
        {
            Assert.True(SabaHelper.IsHideSaba(SpecialPlatform.Jollysports));
        }

        [Fact]
        public void IsHideSaba_ReturnsTrue_ForJollysportCV()
        {
            Assert.True(SabaHelper.IsHideSaba(SpecialPlatform.JollysportCV));
        }

        [Fact]
        public void IsHideSaba_ReturnsFalse_ForNormal()
        {
            Assert.False(SabaHelper.IsHideSaba(SpecialPlatform.Normal));
        }

        [Fact]
        public void IsHideSaba_ReturnsFalse_ForOther()
        {
            Assert.False(SabaHelper.IsHideSaba(-1));
        }
    }
}