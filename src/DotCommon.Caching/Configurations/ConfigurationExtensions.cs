using DotCommon.Dependency;
using DotCommon.Runtime.Caching;
using DotCommon.Runtime.Caching.Configuration;
using DotCommon.Runtime.Caching.Memory;
using System;

namespace DotCommon.Configurations
{
    public static class ConfigurationExtensions
    {

        /// <summary>缓存配置
        /// </summary>
        public static Configuration UseMemoryCache(this Configuration configuration)
        {
            var container = IocManager.GetContainer();
            //缓存
            container.Register<ICachingConfiguration, CachingConfiguration>();
            container.Register<ICacheManager, DotCommonMemoryCacheManager>();
            container.Register<DotCommonMemoryCache>(DependencyLifeStyle.Transient);
            return configuration;
        }

        /// <summary>缓存配置
        /// </summary>
        public static Configuration CacheConfigure(this Configuration configuration,string cacheName, Action<ICache> initAction)
        {
            var container = IocManager.GetContainer();
            container.Resolve<ICachingConfiguration>().Configure(cacheName, initAction);
            return configuration;
        }

        /// <summary>缓存配置
        /// </summary>
        public static Configuration CacheConfigureAll(this Configuration configuration, Action<ICache> initAction)
        {
            var container = IocManager.GetContainer();
            container.Resolve<ICachingConfiguration>().ConfigureAll(initAction);
            return configuration;
        }

    }
}
