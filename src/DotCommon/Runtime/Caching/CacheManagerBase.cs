using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using DotCommon.Runtime.Caching.Configuration;

namespace DotCommon.Runtime.Caching
{
    public abstract class CacheManagerBase : ICacheManager
    {
        protected readonly ConcurrentDictionary<string, ICache> Caches;
        protected readonly ICachingConfiguration Configuration;
        protected CacheManagerBase(ICachingConfiguration configuration)
        {
            Configuration = configuration;
            Caches = new ConcurrentDictionary<string, ICache>();
        }

        public IReadOnlyList<ICache> GetAllCaches()
        {
            return Caches.Values.ToList();
        }

        public virtual ICache GetCache(string name)
        {
            return Caches.GetOrAdd(name, (cacheName) =>
            {
                var cache = CreateCacheImplementation(cacheName);
                //主要是配置一些缓存的基本信息
                var configurators = Configuration.Configurators.Where(c => c.CacheName == null || c.CacheName == cacheName);
                foreach (var configurator in configurators)
                {
                    configurator.InitAction?.Invoke(cache);
                }
                return cache;
            });
        }

        public virtual void Dispose()
        {
            foreach (var cache in Caches)
            {
                //释放,不通过容器释放
                cache.Value.Dispose();
                //在容器中释放该注入
                // IocManager.GetContainer().Release(cache.Value);
            }

            Caches.Clear();
        }

        /// <summary>
        /// Used to create actual cache implementation.
        /// </summary>
        /// <param name="name">Name of the cache</param>
        /// <returns>Cache object</returns>
        protected abstract ICache CreateCacheImplementation(string name);
    }
}
