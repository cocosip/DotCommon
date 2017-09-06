using System;

namespace DotCommon.Runtime.Caching.Configuration
{
    public interface ICacheConfigurator
    {
        string CacheName { get; }

        Action<ICache> InitAction { get; }
    }
}
