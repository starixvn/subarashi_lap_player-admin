using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Extensions
{
    public class MiddlewareExtensionsTests
    {
        [Theory]
        [InlineData("Mozilla/5.0 (iPhone; CPU iPhone OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Mobile/15E148 Safari/604.1", true)]
        [InlineData("Mozilla/5.0 (iPad; CPU OS 12_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) CriOS/69.0.3497.105 Mobile/15E148 Safari/605.1", true)]
        [InlineData("Mozilla/5.0 (Macintosh; Intel Mac OS X 10_14) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/12.0 Safari/605.1.15", true)]
        [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/58.0.3029.110 Safari/537.36", true)]
        [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36", true)]
        [InlineData("Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/70.0.3538.77 Safari/537.36", false)]
        [InlineData("Mozilla/5.0 (iPhone; CPU iPhone OS 13_0 like Mac OS X) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/13.0 Mobile/15E148 Safari/604.1", false)]
        [InlineData(null, false)]
        [InlineData("", false)]
        public void DisallowsSameSiteNone_UserAgentScenarios_ReturnsExpected(string userAgent, bool expected)
        {
            // Act
            var result = MiddlewareExtensions.DisallowsSameSiteNone(userAgent);

            // Assert
            Assert.Equal(expected, result);
        }
    }
} 