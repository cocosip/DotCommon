using DotCommon.Caching;
using DotCommon.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.Test.Caching
{
    public class CacheTestProvider
    {
        protected IServiceProvider Provider { get; set; }
        public CacheTestProvider()
        {
            IServiceCollection services = new ServiceCollection();
            services
                .AddCommonComponents()
                .AddGenericsMemoryCache();
            Provider = services.BuildServiceProvider();
        }
    }
}
