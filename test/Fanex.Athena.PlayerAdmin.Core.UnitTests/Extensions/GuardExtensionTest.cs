namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using Core.Extensions;
    using Xunit;

    public class GuardExtensionTest
    {
        [Fact]
        public void IsPresent_ReturnsTrue_WhenCollectionHasElements()
        {
            // Arrange
            var collection = new List<string> { "item1", "item2", "item3" };

            // Act
            var result = collection.IsPresent();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsPresent_ReturnsFalse_WhenCollectionIsEmpty()
        {
            // Arrange
            var collection = new List<string>();

            // Act
            var result = collection.IsPresent();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPresent_ReturnsFalse_WhenCollectionIsNull()
        {
            // Arrange
            List<string> collection = null;

            // Act
            var result = collection.IsPresent();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPresent_ReturnsTrue_WhenArrayHasElements()
        {
            // Arrange
            var array = new[] { 1, 2, 3 };

            // Act
            var result = array.IsPresent();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsPresent_ReturnsFalse_WhenArrayIsEmpty()
        {
            // Arrange
            var array = new int[0];

            // Act
            var result = array.IsPresent();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPresent_ReturnsFalse_WhenArrayIsNull()
        {
            // Arrange
            int[] array = null;

            // Act
            var result = array.IsPresent();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPresent_ReturnsTrue_WhenEnumerableHasElements()
        {
            // Arrange
            var enumerable = Enumerable.Range(1, 5);

            // Act
            var result = enumerable.IsPresent();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsPresent_ReturnsFalse_WhenEnumerableIsEmpty()
        {
            // Arrange
            var enumerable = Enumerable.Empty<int>();

            // Act
            var result = enumerable.IsPresent();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPresent_ReturnsTrue_WhenHashSetHasElements()
        {
            // Arrange
            var hashSet = new HashSet<string> { "a", "b", "c" };

            // Act
            var result = hashSet.IsPresent();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsPresent_ReturnsFalse_WhenHashSetIsEmpty()
        {
            // Arrange
            var hashSet = new HashSet<string>();

            // Act
            var result = hashSet.IsPresent();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPresent_ReturnsTrue_WhenDictionaryHasElements()
        {
            // Arrange
            var dictionary = new Dictionary<string, int> { { "key1", 1 }, { "key2", 2 } };

            // Act
            var result = dictionary.IsPresent();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void IsPresent_ReturnsFalse_WhenDictionaryIsEmpty()
        {
            // Arrange
            var dictionary = new Dictionary<string, int>();

            // Act
            var result = dictionary.IsPresent();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void IsPresent_WorksWithDifferentTypes()
        {
            // Arrange
            var stringList = new List<string> { "test" };
            var intList = new List<int> { 1, 2, 3 };
            var objectList = new List<object> { new() };
            var customList = new List<CustomTestClass> { new() };

            // Act & Assert
            Assert.True(stringList.IsPresent());
            Assert.True(intList.IsPresent());
            Assert.True(objectList.IsPresent());
            Assert.True(customList.IsPresent());
        }

        [Fact]
        public void IsPresent_WorksWithLinqQueries()
        {
            // Arrange
            var numbers = new[] { 1, 2, 3, 4, 5 };
            var evenNumbers = numbers.Where(x => x % 2 == 0);
            var largeNumbers = numbers.Where(x => x > 10);

            // Act & Assert
            Assert.True(evenNumbers.IsPresent()); // Has 2, 4
            Assert.False(largeNumbers.IsPresent()); // No numbers > 10
        }

        [Fact]
        public void IsPresent_WorksWithYieldedEnumerable()
        {
            // Arrange
            var yieldedEnumerable = GetTestEnumerable();
            var emptyYieldedEnumerable = GetEmptyEnumerable();

            // Act & Assert
            Assert.True(yieldedEnumerable.IsPresent());
            Assert.False(emptyYieldedEnumerable.IsPresent());
        }

        private static IEnumerable<int> GetTestEnumerable()
        {
            yield return 1;
            yield return 2;
            yield return 3;
        }

        private static IEnumerable<int> GetEmptyEnumerable()
        {
            yield break;
        }

        private class CustomTestClass
        {
            public string Name { get; set; } = "Test";
        }
    }
}