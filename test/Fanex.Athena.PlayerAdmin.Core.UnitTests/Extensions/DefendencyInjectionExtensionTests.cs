using Fanex.Athena.PlayerAdmin.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Extensions
{
    public class DefendencyInjectionExtensionTests
    {
        [Test]
        public void AddCore_WhenCall_NotThrowException()
        {
            //Arrange
            var service = Substitute.For<IServiceCollection>();

            //Assert
            Assert.DoesNotThrow(() => service.AddCore());
        }
    }
}
