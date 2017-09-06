using System;
using System.Threading.Tasks;

namespace DotCommon.Runtime.Caching
{
    public interface ICache : IDisposable
    {
        string Name { get; }

        TimeSpan DefaultSlidingExpireTime { get; set; }

        TimeSpan? DefaultAbsoluteExpireTime { get; set; }

        object Get(string key, Func<string, object> factory);

        Task<object> GetAsync(string key, Func<string, Task<object>> factory);

        object GetOrDefault(string key);

        Task<object> GetOrDefaultAsync(string key);

        void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        Task SetAsync(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        void Remove(string key);

        Task RemoveAsync(string key);

        void Clear();

        Task ClearAsync();
    }
}
