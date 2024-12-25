using Microsoft.Extensions.Caching.Distributed;
using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using DotCommon.Threading;
using DotCommon.Serializing;

namespace DotCommon.Caching
{
    /// <summary>分布式缓存
    /// </summary>
    /// <typeparam name="TCacheItem"></typeparam>
    public class DistributedCache<TCacheItem> : IDistributedCache<TCacheItem>
       where TCacheItem : class
    {
        /// <summary>缓存名
        /// </summary>
        protected string CacheName { get; set; }

        /// <summary>分布式缓存
        /// </summary>
        protected IDistributedCache Cache { get; }

        /// <summary>CancellationTokenProvider
        /// </summary>
        protected ICancellationTokenProvider CancellationTokenProvider { get; }

        /// <summary>对象序列化
        /// </summary>
        protected IObjectSerializer ObjectSerializer { get; }

        /// <summary>同步锁
        /// </summary>
        protected AsyncLock AsyncLock { get; } = new AsyncLock();

        /// <summary>Ctor
        /// </summary>
        public DistributedCache(
            IDistributedCache cache,
            ICancellationTokenProvider cancellationTokenProvider,
            IObjectSerializer objectSerializer)
        {
            Cache = cache;
            CancellationTokenProvider = cancellationTokenProvider;
            ObjectSerializer = objectSerializer;

            SetDefaultOptions();
        }


        /// <summary>根据缓存Key获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public virtual TCacheItem Get(string key)
        {
            var cachedBytes = Cache.Get(NormalizeKey(key));
            if (cachedBytes == null)
            {
                return null;
            }

            return ObjectSerializer.Deserialize<TCacheItem>(cachedBytes);
        }

        /// <summary>根据缓存Key异步获取缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        public virtual async Task<TCacheItem> GetAsync(string key, CancellationToken token = default)
        {
            var cachedBytes = await Cache.GetAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
            if (cachedBytes == null)
            {
                return null;
            }

            return ObjectSerializer.Deserialize<TCacheItem>(cachedBytes);
        }

        /// <summary>获取或者添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="factory">缓存添加操作</param>
        /// <returns></returns>
        public TCacheItem GetOrAdd(string key, Func<TCacheItem> factory)
        {
            var value = Get(key);
            if (value != null)
            {
                return value;
            }

            using (AsyncLock.Lock())
            {
                value = Get(key);
                if (value != null)
                {
                    return value;
                }

                value = factory();
                Set(key, value);

            }

            return value;
        }

        /// <summary>异步获取或者添加缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="factory">缓存添加操作</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        public async Task<TCacheItem> GetOrAddAsync(string key, Func<Task<TCacheItem>> factory, CancellationToken token = default)
        {
            var value = await GetAsync(key, token);
            if (value != null)
            {
                return value;
            }

            using (await AsyncLock.LockAsync(token))
            {
                value = await GetAsync(key, token);
                if (value != null)
                {
                    return value;
                }

                value = await factory();
                await SetAsync(key, value, token: token);
            }

            return value;
        }

        /// <summary>设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存值</param>
        /// <param name="options">缓存配置设置</param>
        public virtual void Set(string key, TCacheItem value, DistributedCacheEntryOptions options = null)
        {
            Cache.Set(
                NormalizeKey(key),
                ObjectSerializer.Serialize(value),
                options ?? new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(20) }
            );
        }

        /// <summary>异步设置缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="value">缓存值</param>
        /// <param name="options">缓存配置设置</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        public virtual Task SetAsync(string key, TCacheItem value, DistributedCacheEntryOptions options = null, CancellationToken token = default)
        {
            return Cache.SetAsync(
                NormalizeKey(key),
                ObjectSerializer.Serialize(value),
                options ?? new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromMinutes(20) },
                CancellationTokenProvider.FallbackToProvider(token)
            );
        }

        /// <summary>根据缓存Key刷新缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        public virtual void Refresh(string key)
        {
            Cache.Refresh(NormalizeKey(key));
        }

        /// <summary>根据缓存Key异步刷新缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        public virtual Task RefreshAsync(string key, CancellationToken token = default)
        {
            return Cache.RefreshAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
        }

        /// <summary>根据缓存Key移除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        public virtual void Remove(string key)
        {
            Cache.Remove(NormalizeKey(key));
        }

        /// <summary>根据缓存Key异步移除缓存
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="token">CancellationToken</param>
        /// <returns></returns>
        public virtual Task RemoveAsync(string key, CancellationToken token = default)
        {
            return Cache.RemoveAsync(NormalizeKey(key), CancellationTokenProvider.FallbackToProvider(token));
        }

        /// <summary>格式化缓存Key
        /// </summary>
        protected virtual string NormalizeKey(string key)
        {
            var normalizedKey = "c:" + CacheName + ",k:" + key;
            return normalizedKey;
        }

        /// <summary>设置默认的配置
        /// </summary>
        protected virtual void SetDefaultOptions()
        {
            //CacheName
            var cacheNameAttribute = typeof(TCacheItem)
                .GetCustomAttributes(true)
                .OfType<CacheNameAttribute>()
                .FirstOrDefault();

            CacheName = cacheNameAttribute != null ? cacheNameAttribute.Name : typeof(TCacheItem).Name;

        }
    }

}