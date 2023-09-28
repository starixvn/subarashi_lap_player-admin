using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Fanex.Athena.PlayerAdmin.Core.Helpers
{
    public static class LockHelper
    {
        private static readonly ConcurrentDictionary<string, object> _lockObjects = new ConcurrentDictionary<string, object>();
        private static readonly ConcurrentDictionary<string, SemaphoreSlim> _lockAsyncObjects = new ConcurrentDictionary<string, SemaphoreSlim>();

        public static TResult Lock<TResult>(string key, Func<TResult> doing)
            where TResult : class
        {
            var lockObject = _lockObjects.GetOrAdd(key, _ => new object());
            lock (lockObject)
            {
                return doing?.Invoke();
            }
        }

        public static async Task<TResult> LockAsync<TResult>(string key, Func<Task<TResult>> doing)
           where TResult : class
        {
            var lockAsyncObject = _lockAsyncObjects.GetOrAdd(key, _ => new SemaphoreSlim(1, 1));
            await lockAsyncObject.WaitAsync();

            try
            {
                if (doing != null)
                {
                    var result = await doing.Invoke();
                    return result;
                }
                return default;
            }
            finally
            {
                lockAsyncObject.Release();
            }
        }
    }
}