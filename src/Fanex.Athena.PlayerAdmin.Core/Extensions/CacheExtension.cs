namespace Fanex.Athena.PlayerAdmin.Core.Extensions
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;

    public static class CacheExtension
    {
        public static async Task<T> TryGetOrAddFromCache<T>(this IMemoryCache cache, string cacheKey, TimeSpan cacheTime, Func<Task<T>> func)
        {
            if (!cache.TryGetValue(cacheKey, out T data))
            {
                data = await func.Invoke();
                if (data is not null)
                {
                    cache.Set(cacheKey, data, cacheTime);
                }
            }
            return data;
        }

        public static T TryGetOrAddFromCacheWithoutExpired<T>(this IMemoryCache cache, string cacheKey, T dataSource)
        {
            if (dataSource is not null)
            {
                cache.Set(cacheKey, dataSource, new MemoryCacheEntryOptions { Priority = CacheItemPriority.NeverRemove });
                return dataSource;
            }
            cache.TryGetValue(cacheKey, out T data);
            return data;
        }
    }
}