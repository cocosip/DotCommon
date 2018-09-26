using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotCommon.Caching
{
    public interface IDistributedCache<TCacheItem>
         where TCacheItem : class
    {
        TCacheItem Get(
            string key
        );

        Task<TCacheItem> GetAsync(string key, CancellationToken token = default);

        TCacheItem GetOrAdd(string key, Func<TCacheItem> factory);

        Task<TCacheItem> GetOrAddAsync(string key, Func<Task<TCacheItem>> factory, CancellationToken token = default);

        void Set(string key, TCacheItem value, DistributedCacheEntryOptions options = null);

        Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions options = null, CancellationToken token = default);

        void Refresh(string key);

        Task RefreshAsync(string key, CancellationToken token = default);

        void Remove(string key);

        Task RemoveAsync(string key, CancellationToken token = default);
    }
}
