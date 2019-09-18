using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotCommon.Caching
{
    /// <summary>分布式缓存
    /// </summary>
    /// <typeparam name="TCacheItem"></typeparam>
    public interface IDistributedCache<TCacheItem>
         where TCacheItem : class
    {
        /// <summary>根据缓存Key获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        TCacheItem Get(string key);

        /// <summary>根据缓存Key异步获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        Task<TCacheItem> GetAsync(string key, CancellationToken token = default);

        /// <summary>获取或者添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="factory">缓存添加操作</param>
        /// <returns></returns>
        TCacheItem GetOrAdd(string key, Func<TCacheItem> factory);

        /// <summary>异步获取或者添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="factory">缓存添加操作</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        Task<TCacheItem> GetOrAddAsync(string key, Func<Task<TCacheItem>> factory, CancellationToken token = default);

        /// <summary>设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存值</param>
        /// <param name="options">缓存配置设置</param>
        void Set(string key, TCacheItem value, DistributedCacheEntryOptions options = null);


        /// <summary>异步设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存值</param>
        /// <param name="options">缓存配置设置</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions options = null, CancellationToken token = default);

        /// <summary>根据缓存Key刷新缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        void Refresh(string key);

        /// <summary>根据缓存Key异步刷新缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        Task RefreshAsync(string key, CancellationToken token = default);

        /// <summary>根据缓存Key移除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        void Remove(string key);

        /// <summary>根据缓存Key异步移除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        Task RemoveAsync(string key, CancellationToken token = default);
    }
}
