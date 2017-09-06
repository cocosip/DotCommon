using System;
using System.Collections.Generic;

namespace DotCommon.Runtime.Caching
{
    public interface ICacheManager : IDisposable
    {
        IReadOnlyList<ICache> GetAllCaches();

        ICache GetCache(string name);
    }
}
