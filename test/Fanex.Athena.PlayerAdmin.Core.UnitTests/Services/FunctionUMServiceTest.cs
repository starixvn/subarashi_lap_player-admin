namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Services
{
    using System;
    using System.Collections.Generic;
    using Core.Services;
    using Microsoft.Extensions.Options;
    using Moq;
    using Settings;
    using Xunit;

    public class FunctionUMServiceTest
    {
        private readonly Mock<IOptions<FunctionUMSetting>> _mockOptions;

        public FunctionUMServiceTest()
        {
            _mockOptions = new Mock<IOptions<FunctionUMSetting>>();
        }

        [Fact]
        public void GetUnderMaintenanceInfo_ReturnsCorrectInfo_WhenFunctionExists()
        {
            // Arrange
            var setting = new FunctionUMSetting
            {
                Functions = new List<FunctionUM>
                {
                    new()
                    {
                        Type = FunctionUMType.Commission,
                        IsUM = true,
                        TimeFrom = "2023-01-01 10:00",
                        TimeTo = "2023-01-01 12:00"
                    },
                    new()
                    {
                        Type = FunctionUMType.BetSetting,
                        IsUM = false,
                        TimeFrom = "2023-01-02 14:00",
                        TimeTo = "2023-01-02 16:00"
                    }
                }
            };
            _mockOptions.Setup(x => x.Value).Returns(setting);
            var service = new FunctionUMService(_mockOptions.Object);

            // Act
            var result = service.GetUnderMaintenanceInfo(FunctionUMType.Commission);

            // Assert
            Assert.True(result.isUM);
            Assert.Equal("2023-01-01 10:00", result.timeFrom);
            Assert.Equal("2023-01-01 12:00", result.timeTo);
        }

        [Fact]
        public void GetUnderMaintenanceInfo_ReturnsCorrectInfo_WhenFunctionExistsButNotUnderMaintenance()
        {
            // Arrange
            var setting = new FunctionUMSetting
            {
                Functions = new List<FunctionUM>
                {
                    new()
                    {
                        Type = FunctionUMType.BetSetting,
                        IsUM = false,
                        TimeFrom = "2023-01-02 14:00",
                        TimeTo = "2023-01-02 16:00"
                    }
                }
            };
            _mockOptions.Setup(x => x.Value).Returns(setting);
            var service = new FunctionUMService(_mockOptions.Object);

            // Act
            var result = service.GetUnderMaintenanceInfo(FunctionUMType.BetSetting);

            // Assert
            Assert.False(result.isUM);
            Assert.Equal("2023-01-02 14:00", result.timeFrom);
            Assert.Equal("2023-01-02 16:00", result.timeTo);
        }

        [Fact]
        public void GetUnderMaintenanceInfo_ReturnsDefaultValues_WhenFunctionNotFound()
        {
            // Arrange
            var setting = new FunctionUMSetting
            {
                Functions = new List<FunctionUM>
                {
                    new()
                    {
                        Type = FunctionUMType.Commission,
                        IsUM = true,
                        TimeFrom = "2023-01-01 10:00",
                        TimeTo = "2023-01-01 12:00"
                    }
                }
            };
            _mockOptions.Setup(x => x.Value).Returns(setting);
            var service = new FunctionUMService(_mockOptions.Object);

            // Act
            var result = service.GetUnderMaintenanceInfo(FunctionUMType.BetSetting);

            // Assert
            Assert.False(result.isUM);
            Assert.Null(result.timeFrom);
            Assert.Null(result.timeTo);
        }

        [Fact]
        public void GetUnderMaintenanceInfo_ReturnsDefaultValues_WhenFunctionsListIsEmpty()
        {
            // Arrange
            var setting = new FunctionUMSetting
            {
                Functions = new List<FunctionUM>()
            };
            _mockOptions.Setup(x => x.Value).Returns(setting);
            var service = new FunctionUMService(_mockOptions.Object);

            // Act
            var result = service.GetUnderMaintenanceInfo(FunctionUMType.Commission);

            // Assert
            Assert.False(result.isUM);
            Assert.Null(result.timeFrom);
            Assert.Null(result.timeTo);
        }

        [Fact]
        public void GetUnderMaintenanceInfo_ThrowsArgumentNullException_WhenFunctionsListIsNull()
        {
            // Arrange
            var setting = new FunctionUMSetting
            {
                Functions = null
            };
            _mockOptions.Setup(x => x.Value).Returns(setting);
            var service = new FunctionUMService(_mockOptions.Object);

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => service.GetUnderMaintenanceInfo(FunctionUMType.Commission));
        }

        [Fact]
        public void GetUnderMaintenanceInfo_HandlesMultipleFunctionsOfSameType()
        {
            // Arrange
            var setting = new FunctionUMSetting
            {
                Functions = new List<FunctionUM>
                {
                    new()
                    {
                        Type = FunctionUMType.Commission,
                        IsUM = false,
                        TimeFrom = "2023-01-01 08:00",
                        TimeTo = "2023-01-01 10:00"
                    },
                    new()
                    {
                        Type = FunctionUMType.Commission,
                        IsUM = true,
                        TimeFrom = "2023-01-01 10:00",
                        TimeTo = "2023-01-01 12:00"
                    }
                }
            };
            _mockOptions.Setup(x => x.Value).Returns(setting);
            var service = new FunctionUMService(_mockOptions.Object);

            // Act - Should return the first match
            var result = service.GetUnderMaintenanceInfo(FunctionUMType.Commission);

            // Assert
            Assert.False(result.isUM);
            Assert.Equal("2023-01-01 08:00", result.timeFrom);
            Assert.Equal("2023-01-01 10:00", result.timeTo);
        }

        [Fact]
        public void GetUnderMaintenanceInfo_WithNullTimeValues_ReturnsNullTimes()
        {
            // Arrange
            var setting = new FunctionUMSetting
            {
                Functions = new List<FunctionUM>
                {
                    new()
                    {
                        Type = FunctionUMType.Commission,
                        IsUM = true,
                        TimeFrom = null,
                        TimeTo = null
                    }
                }
            };
            _mockOptions.Setup(x => x.Value).Returns(setting);
            var service = new FunctionUMService(_mockOptions.Object);

            // Act
            var result = service.GetUnderMaintenanceInfo(FunctionUMType.Commission);

            // Assert
            Assert.True(result.isUM);
            Assert.Null(result.timeFrom);
            Assert.Null(result.timeTo);
        }

        [Fact]
        public void GetUnderMaintenanceInfo_WithEmptyTimeValues_ReturnsEmptyTimes()
        {
            // Arrange
            var setting = new FunctionUMSetting
            {
                Functions = new List<FunctionUM>
                {
                    new()
                    {
                        Type = FunctionUMType.Commission,
                        IsUM = true,
                        TimeFrom = "",
                        TimeTo = ""
                    }
                }
            };
            _mockOptions.Setup(x => x.Value).Returns(setting);
            var service = new FunctionUMService(_mockOptions.Object);

            // Act
            var result = service.GetUnderMaintenanceInfo(FunctionUMType.Commission);

            // Assert
            Assert.True(result.isUM);
            Assert.Equal("", result.timeFrom);
            Assert.Equal("", result.timeTo);
        }

        [Fact]
        public void Constructor_WithValidOptions_InitializesCorrectly()
        {
            // Arrange
            var setting = new FunctionUMSetting { Functions = new List<FunctionUM>() };
            var options = new Mock<IOptions<FunctionUMSetting>>();
            options.Setup(x => x.Value).Returns(setting);

            // Act
            var service = new FunctionUMService(options.Object);

            // Assert
            Assert.NotNull(service);
        }

        [Fact]
        public void Constructor_WithNullOptions_ThrowsNullReferenceException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => new FunctionUMService(null));
        }

        [Theory]
        [InlineData(FunctionUMType.Commission)]
        [InlineData(FunctionUMType.BetSetting)]
        public void GetUnderMaintenanceInfo_WorksWithAllFunctionUMTypes(FunctionUMType functionType)
        {
            // Arrange
            var setting = new FunctionUMSetting
            {
                Functions = new List<FunctionUM>
                {
                    new()
                    {
                        Type = functionType,
                        IsUM = true,
                        TimeFrom = "10:00",
                        TimeTo = "12:00"
                    }
                }
            };
            _mockOptions.Setup(x => x.Value).Returns(setting);
            var service = new FunctionUMService(_mockOptions.Object);

            // Act
            var result = service.GetUnderMaintenanceInfo(functionType);

            // Assert
            Assert.True(result.isUM);
            Assert.Equal("10:00", result.timeFrom);
            Assert.Equal("12:00", result.timeTo);
        }
    }
}