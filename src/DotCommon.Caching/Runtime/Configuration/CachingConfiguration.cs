using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.Runtime.Caching.Configuration
{
    public  class CachingConfiguration : ICachingConfiguration
    {
        public IReadOnlyList<ICacheConfigurator> Configurators
        {
            get { return _configurators.ToList(); }
        }
        private readonly List<ICacheConfigurator> _configurators;

        public CachingConfiguration()
        {
            _configurators = new List<ICacheConfigurator>();
        }

        public void ConfigureAll(Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(initAction));
        }

        public void Configure(string cacheName, Action<ICache> initAction)
        {
            _configurators.Add(new CacheConfigurator(cacheName, initAction));
        }
    }
}
