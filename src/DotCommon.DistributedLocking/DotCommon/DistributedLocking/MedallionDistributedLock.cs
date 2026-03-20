using System;
using System.Threading;
using System.Threading.Tasks;
using Medallion.Threading;

namespace DotCommon.DistributedLocking
{
    /// <summary>
    /// Medallion.Threading adapter implementation of <see cref="IDistributedLock"/>.
    /// Supports various distributed backends (Redis, SQL Server, PostgreSQL, MySQL, etc.)
    /// through <see cref="IDistributedLockProvider"/>.
    /// </summary>
    public class MedallionDistributedLock : IDistributedLock
    {
        private readonly IDistributedLockProvider _lockProvider;
        private readonly IDistributedLockKeyNormalizer _keyNormalizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedallionDistributedLock"/> class.
        /// </summary>
        /// <param name="lockProvider">The Medallion lock provider (e.g., Redis, SQL Server)</param>
        /// <param name="keyNormalizer">The key normalizer</param>
        public MedallionDistributedLock(
            IDistributedLockProvider lockProvider,
            IDistributedLockKeyNormalizer keyNormalizer)
        {
            _lockProvider = lockProvider;
            _keyNormalizer = keyNormalizer;
        }

        /// <summary>
        /// Tries to acquire a named lock.
        /// Returns a disposable lock handle if successful, null otherwise.
        /// </summary>
        /// <param name="name">The name of the lock</param>
        /// <param name="timeout">Timeout for acquiring the lock. Use null for no timeout.</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>A disposable lock handle if acquired successfully, null otherwise</returns>
        public async Task<IDistributedLockHandle?> TryAcquireAsync(
            string name,
            TimeSpan? timeout = null,
            CancellationToken cancellationToken = default)
        {
            DotCommon.Check.NotNullOrWhiteSpace(name, nameof(name));

            var normalizedKey = _keyNormalizer.NormalizeKey(name);
            var distributedLock = _lockProvider.CreateLock(normalizedKey);

            IDistributedSynchronizationHandle? handle;
            if (timeout.HasValue)
            {
                handle = await distributedLock.TryAcquireAsync(timeout.Value, cancellationToken);
            }
            else
            {
                // If no timeout is specified, use a very long timeout (effectively infinite)
                handle = await distributedLock.TryAcquireAsync(TimeSpan.MaxValue, cancellationToken);
            }

            return handle != null ? new MedallionDistributedLockHandle(handle) : null;
        }
    }
}
