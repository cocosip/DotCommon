using System;
using System.Threading.Tasks;

namespace DotCommon.Runtime.Caching
{
    public interface ITypedCache<TKey, TValue> : IDisposable
    {
        string Name { get; }

        TimeSpan DefaultSlidingExpireTime { get; set; }

        ICache InternalCache { get; }

        TValue Get(TKey key, Func<TKey, TValue> factory);

        Task<TValue> GetAsync(TKey key, Func<TKey, Task<TValue>> factory);

        TValue GetOrDefault(TKey key);

        Task<TValue> GetOrDefaultAsync(TKey key);
        void Set(TKey key, TValue value, TimeSpan? slidingExpireTime = null);
        Task SetAsync(TKey key, TValue value, TimeSpan? slidingExpireTime = null);
        void Remove(TKey key);
        Task RemoveAsync(TKey key);

        void Clear();

        Task ClearAsync();
    }
}
