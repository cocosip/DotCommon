using Microsoft.Extensions.Options;

namespace DotCommon.Caching
{
    public class DistributedCacheKeyNormalizer : IDistributedCacheKeyNormalizer
    {
        protected DotCommonDistributedCacheOptions DistributedCacheOptions { get; }

        public DistributedCacheKeyNormalizer(
            IOptions<DotCommonDistributedCacheOptions> distributedCacheOptions)
        {
            DistributedCacheOptions = distributedCacheOptions.Value;
        }

        public virtual string NormalizeKey(DistributedCacheKeyNormalizeArgs args)
        {
            var normalizedKey = $"c:{args.CacheName},k:{DistributedCacheOptions.KeyPrefix}{args.Key}";

            return normalizedKey;
        }
    }
}
