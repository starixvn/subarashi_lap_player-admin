namespace Fanex.Athena.PlayerAdmin.Core.UnitTests.Extensions
{
    using Core.Extensions;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;

    public class CacheExtensionTest : IDisposable
    {
        private readonly IMemoryCache _cache;

        public CacheExtensionTest()
        {
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            _cache = serviceProvider.GetRequiredService<IMemoryCache>();
        }

        public void Dispose()
        {
            _cache?.Dispose();
        }

        [Fact]
        public async Task TryGetOrAddFromCache_ReturnsExistingValue_WhenCacheHit()
        {
            // Arrange
            var cacheKey = "test-key";
            var cachedValue = "cached-data";
            var cacheTime = TimeSpan.FromMinutes(5);
            var funcCalled = false;

            // Pre-populate cache
            _cache.Set(cacheKey, cachedValue, cacheTime);

            // Act
            var result = await _cache.TryGetOrAddFromCache(cacheKey, cacheTime, () =>
            {
                funcCalled = true;
                return Task.FromResult("new-data");
            });

            // Assert
            Assert.Equal(cachedValue, result);
            Assert.False(funcCalled);
        }

        [Fact]
        public async Task TryGetOrAddFromCache_CallsFunctionAndCaches_WhenCacheMiss()
        {
            // Arrange
            var cacheKey = "test-key-miss";
            var newValue = "new-data";
            var cacheTime = TimeSpan.FromMinutes(5);
            var funcCalled = false;

            // Act
            var result = await _cache.TryGetOrAddFromCache(cacheKey, cacheTime, () =>
            {
                funcCalled = true;
                return Task.FromResult(newValue);
            });

            // Assert
            Assert.Equal(newValue, result);
            Assert.True(funcCalled);

            // Verify it was cached by trying to get it again
            var cachedResult = _cache.Get<string>(cacheKey);
            Assert.Equal(newValue, cachedResult);
        }

        [Fact]
        public async Task TryGetOrAddFromCache_DoesNotCache_WhenFunctionReturnsNull()
        {
            // Arrange
            var cacheKey = "test-key-null";
            var cacheTime = TimeSpan.FromMinutes(5);

            // Act
            var result = await _cache.TryGetOrAddFromCache(cacheKey, cacheTime, () => { return Task.FromResult<string>(null); });

            // Assert
            Assert.Null(result);

            // Verify it was not cached
            var cachedResult = _cache.Get<string>(cacheKey);
            Assert.Null(cachedResult);
        }

        [Fact]
        public async Task TryGetOrAddFromCache_HandlesException_WhenFunctionThrows()
        {
            // Arrange
            var cacheKey = "test-key-exception";
            var cacheTime = TimeSpan.FromMinutes(5);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _cache.TryGetOrAddFromCache<string>(cacheKey, cacheTime, () => { throw new InvalidOperationException("Test exception"); }));

            // Verify nothing was cached
            var cachedResult = _cache.Get<string>(cacheKey);
            Assert.Null(cachedResult);
        }

        [Fact]
        public void TryGetOrAddFromCacheWithoutExpired_SetsCache_WhenDataSourceIsNotNull()
        {
            // Arrange
            var cacheKey = "test-key-no-expire";
            var dataSource = "test-data";
            var expectedOptions = new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove };

            // Act
            var result = _cache.TryGetOrAddFromCacheWithoutExpired(cacheKey, dataSource);

            // Assert
            Assert.Equal(dataSource, result);

            // Verify it was cached
            var cachedResult = _cache.Get<string>(cacheKey);
            Assert.Equal(dataSource, cachedResult);
        }

        [Fact]
        public void TryGetOrAddFromCacheWithoutExpired_ReturnsFromCache_WhenDataSourceIsNull()
        {
            // Arrange
            var cacheKey = "test-key-cached";
            var cachedValue = "cached-data";

            // Pre-populate cache
            _cache.Set(cacheKey, cachedValue);

            // Act
            var result = _cache.TryGetOrAddFromCacheWithoutExpired<string>(cacheKey, null);

            // Assert
            Assert.Equal(cachedValue, result);
        }

        [Fact]
        public void TryGetOrAddFromCacheWithoutExpired_ReturnsDefault_WhenDataSourceIsNullAndCacheMiss()
        {
            // Arrange
            var cacheKey = "test-key-miss-null";

            // Act
            var result = _cache.TryGetOrAddFromCacheWithoutExpired<string>(cacheKey, null);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task TryGetOrAddFromCache_WorksWithDifferentTypes()
        {
            // Arrange
            var cacheKey = "int-key";
            var intValue = 42;
            var cacheTime = TimeSpan.FromMinutes(5);

            // Act
            var result = await _cache.TryGetOrAddFromCache(cacheKey, cacheTime, () => { return Task.FromResult(intValue); });

            // Assert
            Assert.Equal(intValue, result);

            // Verify it was cached
            var cachedResult = _cache.Get<int>(cacheKey);
            Assert.Equal(intValue, cachedResult);
        }

        [Fact]
        public void TryGetOrAddFromCacheWithoutExpired_WorksWithValueTypes()
        {
            // Arrange
            var cacheKey = "int-key-value";
            var intValue = 42;

            // Act
            var result = _cache.TryGetOrAddFromCacheWithoutExpired(cacheKey, intValue);

            // Assert
            Assert.Equal(intValue, result);

            // Verify it was cached
            var cachedResult = _cache.Get<int>(cacheKey);
            Assert.Equal(intValue, cachedResult);
        }

        [Fact]
        public async Task TryGetOrAddFromCache_WithComplexObject()
        {
            // Arrange
            var cacheKey = "object-key";
            var complexObject = new { Name = "Test", Value = 123 };
            var cacheTime = TimeSpan.FromMinutes(10);

            // Act
            var result = await _cache.TryGetOrAddFromCache(cacheKey, cacheTime, () => { return Task.FromResult(complexObject); });

            // Assert
            Assert.Equal(complexObject, result);

            // Verify it was cached
            var cachedResult = _cache.Get(cacheKey);
            Assert.Equal(complexObject, cachedResult);
        }

        [Fact]
        public void TryGetOrAddFromCacheWithoutExpired_WithComplexObject()
        {
            // Arrange
            var cacheKey = "object-key-complex";
            var complexObject = new { Name = "Test", Value = 123 };

            // Act
            var result = _cache.TryGetOrAddFromCacheWithoutExpired(cacheKey, complexObject);

            // Assert
            Assert.Equal(complexObject, result);

            // Verify it was cached
            var cachedResult = _cache.Get(cacheKey);
            Assert.Equal(complexObject, cachedResult);
        }
    }
}