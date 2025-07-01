using System.Collections.Generic;
using Fanex.Athena.PlayerAdmin.Core.Services;
using Fanex.Athena.PlayerAdmin.Core.Settings;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    public class FunctionUMServiceTests
    {
        [Fact]
        public void GetUnderMaintenanceInfo_WhenFunctionExists_ReturnsCorrectInfo()
        {
            // Arrange
            var functionType = FunctionUMType.Commission;
            var expectedUM = new FunctionUM
            {
                Type = functionType,
                IsUM = true,
                TimeFrom = "10:00",
                TimeTo = "12:00"
            };
            var setting = new FunctionUMSetting
            {
                Functions = new List<FunctionUM> { expectedUM }
            };
            var options = Mock.Of<IOptions<FunctionUMSetting>>(o => o.Value == setting);
            var service = new FunctionUMService(options);

            // Act
            var (isUM, timeFrom, timeTo) = service.GetUnderMaintenanceInfo(functionType);

            // Assert
            Assert.True(isUM);
            Assert.Equal("10:00", timeFrom);
            Assert.Equal("12:00", timeTo);
        }

        [Fact]
        public void GetUnderMaintenanceInfo_WhenFunctionDoesNotExist_ReturnsDefaults()
        {
            // Arrange
            var setting = new FunctionUMSetting
            {
                Functions = new List<FunctionUM>()
            };
            var options = Mock.Of<IOptions<FunctionUMSetting>>(o => o.Value == setting);
            var service = new FunctionUMService(options);

            // Act
            var (isUM, timeFrom, timeTo) = service.GetUnderMaintenanceInfo(FunctionUMType.BetSetting);

            // Assert
            Assert.False(isUM);
            Assert.Null(timeFrom);
            Assert.Null(timeTo);
        }
    }
} 