
using Fanex.Athena.PlayerAdmin.Commission.Extensions;
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.PlayerAdmin.Commission.UnitTests.Extensions
{
    public class DefendencyInjectionExtensionTests
    {
        [Test]
        public void AddCommission_WhenCall_NotThrowException()
        {
            //Arrange
            var service = Substitute.For<IServiceCollection>();

            //Assert
            Assert.DoesNotThrow(() => service.AddCommission());
        }
    }
}
