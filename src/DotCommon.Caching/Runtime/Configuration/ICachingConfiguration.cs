using System;
using System.Collections.Generic;

namespace DotCommon.Runtime.Caching.Configuration
{
    public interface ICachingConfiguration
    {
        /// <summary>
        /// Gets the ABP configuration object.
        /// </summary>
        //IAbpStartupConfiguration AbpConfiguration { get; }

        IReadOnlyList<ICacheConfigurator> Configurators { get; }

        void ConfigureAll(Action<ICache> initAction);

        void Configure(string cacheName, Action<ICache> initAction);
    }
}
