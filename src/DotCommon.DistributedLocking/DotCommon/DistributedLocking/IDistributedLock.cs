using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotCommon.DistributedLocking
{
    /// <summary>
    /// Interface for distributed locking service.
    /// </summary>
    public interface IDistributedLock
    {
        /// <summary>
        /// Tries to acquire a named lock.
        /// Returns a disposable lock handle if successful, null otherwise.
        /// </summary>
        /// <param name="name">The name of the lock</param>
        /// <param name="timeout">Timeout for acquiring the lock. Use null for no timeout.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A disposable lock handle if acquired successfully, null otherwise</returns>
        Task<IDistributedLockHandle?> TryAcquireAsync(
            string name,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default
        );
    }
}
