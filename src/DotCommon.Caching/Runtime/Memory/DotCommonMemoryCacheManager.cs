using DotCommon.Dependency;
using DotCommon.Runtime.Caching.Configuration;

namespace DotCommon.Runtime.Caching.Memory
{
    public class DotCommonMemoryCacheManager : CacheManagerBase
    {
        public DotCommonMemoryCacheManager(ICachingConfiguration configuration) : base(configuration)
        {
        }

        protected override ICache CreateCacheImplementation(string name)
        {
            return IocManager.GetContainer().Resolve<DotCommonMemoryCache>(name);
        }
    }
}
