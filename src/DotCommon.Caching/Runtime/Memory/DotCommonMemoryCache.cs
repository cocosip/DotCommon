﻿#if !NETSTANDARD2_0
using System;
using System.Runtime.Caching;
namespace DotCommon.Runtime.Caching.Memory
{
    public class DotCommonMemoryCache : CacheBase
    {
        private MemoryCache _memoryCache;
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="name">Unique name of the cache</param>
        public DotCommonMemoryCache(string name)
            : base(name)
        {
            _memoryCache = new MemoryCache(Name);
        }

        public override object GetOrDefault(string key)
        {
            return _memoryCache.Get(key);
        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new ArgumentException("Can not insert null values to the cache!");
            }

            var cachePolicy = new CacheItemPolicy();

            if (absoluteExpireTime != null)
            {
                cachePolicy.AbsoluteExpiration = DateTimeOffset.Now.Add(absoluteExpireTime.Value);
            }
            else if (slidingExpireTime != null)
            {
                cachePolicy.SlidingExpiration = slidingExpireTime.Value;
            }
            else if (DefaultAbsoluteExpireTime != null)
            {
                cachePolicy.AbsoluteExpiration = DateTimeOffset.Now.Add(DefaultAbsoluteExpireTime.Value);
            }
            else
            {
                cachePolicy.SlidingExpiration = DefaultSlidingExpireTime;
            }

            _memoryCache.Set(key, value, cachePolicy);
        }

        public override void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public override void Clear()
        {
            _memoryCache.Dispose();
            _memoryCache = new MemoryCache(Name);
        }

        public override void Dispose()
        {
            _memoryCache.Dispose();
            base.Dispose();
        }
    }
}
#endif