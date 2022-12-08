
using Microsoft.Extensions.DependencyInjection;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.PlayerAdmin.BatchUpdate.UnitTests
{
    public class DefendencyInjectionExtensionTests
    {
        [Test]
        public void AddAPI_WhenCall_NotThrowException()
        {
            //Arrange
            var service = Substitute.For<IServiceCollection>();

            //Assert
            Assert.DoesNotThrow(() => service.AddBatchUpdate());
        }
    }
}
