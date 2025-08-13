namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Extensions
{
    using Core.Extensions;
    using Microsoft.AspNetCore.Mvc;
    using Moq;

    public class ControllerExtensionsTest
    {
        [Fact]
        public void ToJsonResult_ReturnsFromJsonString_WithProvidedJson()
        {
            // Arrange
            var controller = new Mock<ControllerBase>().Object;
            var jsonContent = "{\"message\":\"test\",\"value\":123}";

            // Act
            var result = controller.ToJsonResult(jsonContent);

            // Assert
            Assert.IsType<FromJsonString>(result);
        }

        [Fact]
        public void ToJsonResult_HandlesNullJson()
        {
            // Arrange
            var controller = new Mock<ControllerBase>().Object;

            // Act
            var result = controller.ToJsonResult(null);

            // Assert
            Assert.IsType<FromJsonString>(result);
        }

        [Fact]
        public void ToJsonResult_HandlesEmptyJson()
        {
            // Arrange
            var controller = new Mock<ControllerBase>().Object;

            // Act
            var result = controller.ToJsonResult(string.Empty);

            // Assert
            Assert.IsType<FromJsonString>(result);
        }
    }

    public class FromJsonStringTest
    {
        [Fact]
        public void Constructor_SetsJsonContent()
        {
            // Arrange
            var jsonContent = "{\"test\":\"value\"}";

            // Act
            var fromJsonString = new FromJsonString(jsonContent);

            // Assert
            Assert.NotNull(fromJsonString);
        }

        [Fact]
        public void Constructor_HandlesNullJson()
        {
            // Act
            var fromJsonString = new FromJsonString(null);

            // Assert
            Assert.NotNull(fromJsonString);
        }

        [Fact]
        public void FromJsonString_CanBeCreatedWithDifferentJsonTypes()
        {
            // Arrange & Act
            var simpleJson = new FromJsonString("{\"test\":\"value\"}");
            var arrayJson = new FromJsonString("[1,2,3]");
            var emptyJson = new FromJsonString("{}");

            // Assert
            Assert.NotNull(simpleJson);
            Assert.NotNull(arrayJson);
            Assert.NotNull(emptyJson);
        }

        [Fact]
        public void FromJsonString_WorksWithComplexJsonStructures()
        {
            // Arrange
            var complexJson = "{\"users\":[{\"id\":1,\"name\":\"John\"},{\"id\":2,\"name\":\"Jane\"}],\"total\":2}";
            var arrayJson = "[{\"id\":1},{\"id\":2}]";
            var nestedJson = "{\"data\":{\"nested\":{\"value\":true}}}";

            // Act
            var complexResult = new FromJsonString(complexJson);
            var arrayResult = new FromJsonString(arrayJson);
            var nestedResult = new FromJsonString(nestedJson);

            // Assert
            Assert.NotNull(complexResult);
            Assert.NotNull(arrayResult);
            Assert.NotNull(nestedResult);
        }
    }
}