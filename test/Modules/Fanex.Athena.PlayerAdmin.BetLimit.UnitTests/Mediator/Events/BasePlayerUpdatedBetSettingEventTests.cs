namespace Fanex.Athena.PlayerAdmin.BetLimit.UnitTests.Mediator.Events
{
    using AutoFixture;
    using BetLimit.Mediator;
    using Core.Constants;
    using Newtonsoft.Json;

    public class BasePlayerUpdatedBetSettingEventTests
    {
        private readonly IFixture _fixture;

        public BasePlayerUpdatedBetSettingEventTests()
        {
            _fixture = new Fixture();
        }

        [Fact]
        public void Constructor_ShouldInitializeWithDefaultValues_WhenCreated()
        {
            // Act
            var testEvent = new TestPlayerUpdatedBetSettingEvent();

            // Assert
            Assert.Null(testEvent.Detail);
            Assert.Null(testEvent.SiteName);
            Assert.Equal(0, testEvent.LicenseeCustId);
            Assert.Equal(0, testEvent.UserId);
            Assert.Equal(0, testEvent.CustID);
        }

        [Fact]
        public void Detail_ShouldBeSettableAndGettable_WhenSet()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent();
            var expectedDetail = _fixture.Create<TestDetail>();

            // Act
            testEvent.Detail = expectedDetail;

            // Assert
            Assert.Equal(expectedDetail, testEvent.Detail);
        }

        [Fact]
        public void ActionID_ShouldReturnUpdateBetLimitConstant_WhenAccessed()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent();

            // Act
            var result = testEvent.ActionID;

            // Assert
            Assert.Equal(LogActionId.UpdateBetLimit, result);
        }

        [Fact]
        public void TypeId_ShouldReturnUserControlLogTypeFunction_WhenAccessed()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent();

            // Act
            var result = testEvent.TypeId;

            // Assert
            Assert.Equal(UserControlLogType.Function, result);
        }

        [Fact]
        public void Doing_ShouldReturnFormattedString_WhenAllPropertiesAreSet()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent
            {
                SiteName = "TestSite",
                LicenseeCustId = 12345,
                Detail = new TestDetail { Name = "TestDetail", Value = 100 }
            };
            var expectedJson = JsonConvert.SerializeObject(testEvent.Detail);
            var expectedDoing = $"LAP - TestSite - 12345 - {expectedJson}";

            // Act
            var result = testEvent.Doing;

            // Assert
            Assert.Equal(expectedDoing, result);
        }

        [Fact]
        public void Doing_ShouldHandleNullSiteName_WhenSiteNameIsNull()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent
            {
                SiteName = null,
                LicenseeCustId = 12345,
                Detail = new TestDetail { Name = "TestDetail", Value = 100 }
            };
            var expectedJson = JsonConvert.SerializeObject(testEvent.Detail);
            var expectedDoing = $"LAP -  - 12345 - {expectedJson}";

            // Act
            var result = testEvent.Doing;

            // Assert
            Assert.Equal(expectedDoing, result);
        }

        [Fact]
        public void Doing_ShouldHandleEmptySiteName_WhenSiteNameIsEmpty()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent
            {
                SiteName = string.Empty,
                LicenseeCustId = 12345,
                Detail = new TestDetail { Name = "TestDetail", Value = 100 }
            };
            var expectedJson = JsonConvert.SerializeObject(testEvent.Detail);
            var expectedDoing = $"LAP -  - 12345 - {expectedJson}";

            // Act
            var result = testEvent.Doing;

            // Assert
            Assert.Equal(expectedDoing, result);
        }

        [Fact]
        public void Doing_ShouldHandleZeroLicenseeCustId_WhenLicenseeCustIdIsZero()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent
            {
                SiteName = "TestSite",
                LicenseeCustId = 0,
                Detail = new TestDetail { Name = "TestDetail", Value = 100 }
            };
            var expectedJson = JsonConvert.SerializeObject(testEvent.Detail);
            var expectedDoing = $"LAP - TestSite - 0 - {expectedJson}";

            // Act
            var result = testEvent.Doing;

            // Assert
            Assert.Equal(expectedDoing, result);
        }

        [Fact]
        public void Doing_ShouldHandleNullDetail_WhenDetailIsNull()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent
            {
                SiteName = "TestSite",
                LicenseeCustId = 12345,
                Detail = null
            };
            var expectedDoing = "LAP - TestSite - 12345 - null";

            // Act
            var result = testEvent.Doing;

            // Assert
            Assert.Equal(expectedDoing, result);
        }

        [Fact]
        public void Doing_ShouldSerializeComplexDetail_WhenDetailIsComplexObject()
        {
            // Arrange
            var complexDetail = new ComplexTestDetail
            {
                Id = 1,
                Name = "Complex Detail",
                NestedObject = new TestDetail { Name = "Nested", Value = 50 },
                ListProperty = new List<string> { "Item1", "Item2", "Item3" }
            };
            var testEvent = new TestPlayerUpdatedBetSettingEvent<ComplexTestDetail>
            {
                SiteName = "TestSite",
                LicenseeCustId = 12345,
                Detail = complexDetail
            };
            var expectedJson = JsonConvert.SerializeObject(complexDetail);
            var expectedDoing = $"LAP - TestSite - 12345 - {expectedJson}";

            // Act
            var result = testEvent.Doing;

            // Assert
            Assert.Equal(expectedDoing, result);
        }

        [Theory]
        [InlineData("Site1", 100, "TestValue")]
        [InlineData("Site2", 200, "AnotherValue")]
        [InlineData("Site3", 300, null)]
        [InlineData("", 0, "")]
        public void Doing_ShouldFormatCorrectly_WithVariousInputs(string siteName, int licenseeCustId, string? detailValue)
        {
            // Arrange
            var detail = detailValue != null ? new TestDetail { Name = detailValue, Value = 42 } : null;
            var testEvent = new TestPlayerUpdatedBetSettingEvent
            {
                SiteName = siteName,
                LicenseeCustId = licenseeCustId,
                Detail = detail
            };
            var expectedJson = JsonConvert.SerializeObject(detail);
            var expectedDoing = $"LAP - {siteName} - {licenseeCustId} - {expectedJson}";

            // Act
            var result = testEvent.Doing;

            // Assert
            Assert.Equal(expectedDoing, result);
        }

        [Fact]
        public void Doing_ShouldHandleSpecialCharactersInSiteName_WhenSiteNameContainsSpecialCharacters()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent
            {
                SiteName = "Test-Site_123!@#",
                LicenseeCustId = 12345,
                Detail = new TestDetail { Name = "Test", Value = 100 }
            };
            var expectedJson = JsonConvert.SerializeObject(testEvent.Detail);
            var expectedDoing = $"LAP - Test-Site_123!@# - 12345 - {expectedJson}";

            // Act
            var result = testEvent.Doing;

            // Assert
            Assert.Equal(expectedDoing, result);
        }

        [Fact]
        public void Doing_ShouldHandleNegativeLicenseeCustId_WhenLicenseeCustIdIsNegative()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent
            {
                SiteName = "TestSite",
                LicenseeCustId = -1,
                Detail = new TestDetail { Name = "Test", Value = 100 }
            };
            var expectedJson = JsonConvert.SerializeObject(testEvent.Detail);
            var expectedDoing = $"LAP - TestSite - -1 - {expectedJson}";

            // Act
            var result = testEvent.Doing;

            // Assert
            Assert.Equal(expectedDoing, result);
        }

        [Fact]
        public void Doing_ShouldProduceValidJson_WhenDetailContainsQuotes()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent
            {
                SiteName = "TestSite",
                LicenseeCustId = 12345,
                Detail = new TestDetail { Name = "Test \"quoted\" value", Value = 100 }
            };

            // Act
            var result = testEvent.Doing;

            // Assert
            Assert.Contains("LAP - TestSite - 12345 -", result);
            Assert.Contains("Test \\\"quoted\\\" value", result); // JSON escapes quotes
        }

        [Fact]
        public void InheritedProperties_ShouldBeSettableAndGettable_WhenSet()
        {
            // Arrange
            var testEvent = new TestPlayerUpdatedBetSettingEvent();
            const string siteName = "InheritedTestSite";
            const int licenseeCustId = 54321;
            const int userId = 98765;
            const int custId = 11111;

            // Act
            testEvent.SiteName = siteName;
            testEvent.LicenseeCustId = licenseeCustId;
            testEvent.UserId = userId;
            testEvent.CustID = custId;

            // Assert
            Assert.Equal(siteName, testEvent.SiteName);
            Assert.Equal(licenseeCustId, testEvent.LicenseeCustId);
            Assert.Equal(userId, testEvent.UserId);
            Assert.Equal(custId, testEvent.CustID);
        }

        [Fact]
        public void Detail_ShouldAllowDifferentGenericTypes_WhenDifferentTypesAreUsed()
        {
            // Arrange & Act
            var stringEvent = new TestPlayerUpdatedBetSettingEvent<string>();
            var intEvent = new TestPlayerUpdatedBetSettingEvent<int>();
            var objectEvent = new TestPlayerUpdatedBetSettingEvent<TestDetail>();

            stringEvent.Detail = "String Detail";
            intEvent.Detail = 42;
            objectEvent.Detail = new TestDetail { Name = "Object Detail", Value = 100 };

            // Assert
            Assert.Equal("String Detail", stringEvent.Detail);
            Assert.Equal(42, intEvent.Detail);
            Assert.NotNull(objectEvent.Detail);
            Assert.Equal("Object Detail", objectEvent.Detail.Name);
            Assert.Equal(100, objectEvent.Detail.Value);
        }

        [Fact]
        public void Doing_ShouldSerializeDifferentGenericTypes_WhenCalled()
        {
            // Arrange
            var stringEvent = new TestPlayerUpdatedBetSettingEvent<string>
            {
                SiteName = "TestSite",
                LicenseeCustId = 123,
                Detail = "Test String"
            };

            var intEvent = new TestPlayerUpdatedBetSettingEvent<int>
            {
                SiteName = "TestSite",
                LicenseeCustId = 123,
                Detail = 456
            };

            // Act
            var stringResult = stringEvent.Doing;
            var intResult = intEvent.Doing;

            // Assert
            Assert.Contains("\"Test String\"", stringResult);
            Assert.Contains("456", intResult);
            Assert.Contains("LAP - TestSite - 123", stringResult);
            Assert.Contains("LAP - TestSite - 123", intResult);
        }

        // Test helper classes
        private class TestPlayerUpdatedBetSettingEvent : BasePlayerUpdatedBetSettingEvent<TestDetail?>
        {
        }

        private class TestPlayerUpdatedBetSettingEvent<T> : BasePlayerUpdatedBetSettingEvent<T>
        {
        }

        private class TestDetail
        {
            public string Name { get; set; } = string.Empty;
            public int Value { get; set; }
        }

        private class ComplexTestDetail
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public TestDetail? NestedObject { get; set; }
            public List<string> ListProperty { get; set; } = new();
        }
    }
}