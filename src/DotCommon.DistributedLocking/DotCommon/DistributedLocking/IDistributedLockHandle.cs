using System;
using System.Threading.Tasks;

namespace DotCommon.DistributedLocking
{
    /// <summary>
    /// Represents a handle to a distributed lock that should be disposed to release the lock.
    /// </summary>
    public interface IDistributedLockHandle : IAsyncDisposable, IDisposable
    {
        // Marker interface - disposal releases the lock
    }
}
