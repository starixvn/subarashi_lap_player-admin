using Fanex.Athena.PlayerAdmin.API.DTO;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fanex.Athena.PlayerAdmin.API.UnitTests.DTO
{
    public class RiskLevelTests
    {
        [Fact]
        public void RiskLevel_Properties_CanBeSetAndRetrieved()
        {
            // Arrange
            var riskLevel = new RiskLevel
            {
                Id = "1",
                Key = "VIP",
                Name = "Very Important Player",
                CustomerClassList = new List<string> { "Class1", "Class2" }
            };

            // Assert
            Assert.Equal("1", riskLevel.Id);
            Assert.Equal("VIP", riskLevel.Key);
            Assert.Equal("Very Important Player", riskLevel.Name);
            Assert.Equal(2, riskLevel.CustomerClassList.Count());
            Assert.Contains("Class1", riskLevel.CustomerClassList);
            Assert.Contains("Class2", riskLevel.CustomerClassList);
        }

        [Fact]
        public void RiskLevel_UncategorizedConstant_HasCorrectValue()
        {
            // Assert
            Assert.Equal("Uncategorized", RiskLevel.Uncategorized);
        }

        [Theory]
        [InlineData("VIP", 1)]
        [InlineData("vip", 1)] // Case insensitive
        [InlineData("Healthy", 2)]
        [InlineData("healthy", 2)] // Case insensitive
        [InlineData("New", 3)]
        [InlineData("new", 3)] // Case insensitive
        [InlineData("Moderate", 4)]
        [InlineData("moderate", 4)] // Case insensitive
        [InlineData("Suspicious", 5)]
        [InlineData("suspicious", 5)] // Case insensitive
        [InlineData("Dangerous", 6)]
        [InlineData("dangerous", 6)] // Case insensitive
        [InlineData("Problems", 7)]
        [InlineData("problems", 7)] // Case insensitive
        [InlineData("Robot", 8)]
        [InlineData("robot", 8)] // Case insensitive
        [InlineData("Uncategorized", 999)]
        [InlineData("uncategorized", 999)] // Case insensitive
        public void GetComparerValue_WithValidRiskLevelKeys_ReturnsCorrectValues(string key, int expectedValue)
        {
            // Arrange
            var riskLevel = new RiskLevel { Key = key };

            // Act
            var result = RiskLevel.GetComparerValue(riskLevel);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData("Unknown")]
        [InlineData("Invalid")]
        [InlineData("")]
        public void GetComparerValue_WithInvalidRiskLevelKeys_ReturnsDefaultValue(string key)
        {
            // Arrange
            var riskLevel = new RiskLevel { Key = key };

            // Act
            var result = RiskLevel.GetComparerValue(riskLevel);

            // Assert
            Assert.Equal(99, result); // Default value for unknown keys
        }

        [Fact]
        public void GetComparerValue_WithNullRiskLevel_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<NullReferenceException>(() => RiskLevel.GetComparerValue(null));
        }
    }

    public class RiskLevelComparerTests
    {
        private readonly RiskLevelComparer _comparer;

        public RiskLevelComparerTests()
        {
            _comparer = new RiskLevelComparer();
        }

        [Theory]
        [InlineData("VIP", "Healthy", -1)] // VIP (1) < Healthy (2)
        [InlineData("Healthy", "New", -1)] // Healthy (2) < New (3)
        [InlineData("New", "Moderate", -1)] // New (3) < Moderate (4)
        [InlineData("Moderate", "Suspicious", -1)] // Moderate (4) < Suspicious (5)
        [InlineData("Suspicious", "Dangerous", -1)] // Suspicious (5) < Dangerous (6)
        [InlineData("Dangerous", "Problems", -1)] // Dangerous (6) < Problems (7)
        [InlineData("Problems", "Robot", -1)] // Problems (7) < Robot (8)
        [InlineData("Robot", "Uncategorized", -1)] // Robot (8) < Uncategorized (999)
        public void Compare_WithDifferentRiskLevels_ReturnsCorrectOrder(string key1, string key2, int expectedResult)
        {
            // Arrange
            var riskLevel1 = new RiskLevel { Key = key1 };
            var riskLevel2 = new RiskLevel { Key = key2 };

            // Act
            var result = _comparer.Compare(riskLevel1, riskLevel2);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("VIP", "VIP", 0)] // Same risk level
        [InlineData("Healthy", "Healthy", 0)] // Same risk level
        [InlineData("New", "New", 0)] // Same risk level
        [InlineData("Moderate", "Moderate", 0)] // Same risk level
        [InlineData("Suspicious", "Suspicious", 0)] // Same risk level
        [InlineData("Dangerous", "Dangerous", 0)] // Same risk level
        [InlineData("Problems", "Problems", 0)] // Same risk level
        [InlineData("Robot", "Robot", 0)] // Same risk level
        [InlineData("Uncategorized", "Uncategorized", 0)] // Same risk level
        public void Compare_WithSameRiskLevels_ReturnsZero(string key1, string key2, int expectedResult)
        {
            // Arrange
            var riskLevel1 = new RiskLevel { Key = key1 };
            var riskLevel2 = new RiskLevel { Key = key2 };

            // Act
            var result = _comparer.Compare(riskLevel1, riskLevel2);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData("Unknown", "VIP", 1)] // Unknown (99) > VIP (1)
        [InlineData("Invalid", "Healthy", 1)] // Invalid (99) > Healthy (2)
        [InlineData("VIP", "Unknown", -1)] // VIP (1) < Unknown (99)
        [InlineData("Healthy", "Invalid", -1)] // Healthy (2) < Invalid (99)
        public void Compare_WithInvalidRiskLevels_ReturnsCorrectOrder(string key1, string key2, int expectedResult)
        {
            // Arrange
            var riskLevel1 = new RiskLevel { Key = key1 };
            var riskLevel2 = new RiskLevel { Key = key2 };

            // Act
            var result = _comparer.Compare(riskLevel1, riskLevel2);

            // Assert
            Assert.Equal(expectedResult, result);
        }

        [Fact]
        public void Compare_WithNullValues_ThrowsException()
        {
            // Arrange
            var riskLevel = new RiskLevel { Key = "VIP" };

            // Act & Assert
            Assert.Throws<NullReferenceException>(() => _comparer.Compare(null, riskLevel));
            Assert.Throws<NullReferenceException>(() => _comparer.Compare(riskLevel, null));
            Assert.Throws<NullReferenceException>(() => _comparer.Compare(null, null));
        }

        [Fact]
        public void Compare_WithCaseInsensitiveKeys_WorksCorrectly()
        {
            // Arrange
            var riskLevel1 = new RiskLevel { Key = "VIP" };
            var riskLevel2 = new RiskLevel { Key = "vip" };

            // Act
            var result = _comparer.Compare(riskLevel1, riskLevel2);

            // Assert
            Assert.Equal(0, result); // Should be equal due to case insensitivity
        }

        [Fact]
        public void Compare_CanSortRiskLevelsCorrectly()
        {
            // Arrange
            var riskLevels = new List<RiskLevel>
            {
                new RiskLevel { Key = "Problems" },
                new RiskLevel { Key = "VIP" },
                new RiskLevel { Key = "Dangerous" },
                new RiskLevel { Key = "Healthy" },
                new RiskLevel { Key = "Robot" },
                new RiskLevel { Key = "New" },
                new RiskLevel { Key = "Suspicious" },
                new RiskLevel { Key = "Moderate" },
                new RiskLevel { Key = "Uncategorized" }
            };

            // Act
            riskLevels.Sort(_comparer);

            // Assert
            var expectedOrder = new[] { "VIP", "Healthy", "New", "Moderate", "Suspicious", "Dangerous", "Problems", "Robot", "Uncategorized" };
            var actualOrder = riskLevels.Select(r => r.Key).ToArray();
            
            Assert.Equal(expectedOrder, actualOrder);
        }
    }
} 