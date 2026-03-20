using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace DotCommon.DistributedLocking
{
    /// <summary>
    /// Local (in-process) implementation of <see cref="IDistributedLock"/>.
    /// Uses <see cref="SemaphoreSlim"/> internally.
    /// This implementation is suitable for single-instance applications or development/testing scenarios.
    /// </summary>
    public class LocalDistributedLock : IDistributedLock
    {
        private readonly ConcurrentDictionary<string, SemaphoreSlim> _semaphores;
        private readonly IDistributedLockKeyNormalizer _keyNormalizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDistributedLock"/> class.
        /// </summary>
        /// <param name="keyNormalizer">The key normalizer</param>
        public LocalDistributedLock(IDistributedLockKeyNormalizer keyNormalizer)
        {
            _keyNormalizer = keyNormalizer;
            _semaphores = new ConcurrentDictionary<string, SemaphoreSlim>();
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
            var semaphore = _semaphores.GetOrAdd(normalizedKey, _ => new SemaphoreSlim(1, 1));

            bool acquired;
            if (timeout.HasValue)
            {
                acquired = await semaphore.WaitAsync(timeout.Value, cancellationToken);
            }
            else
            {
                // If no timeout is specified, wait indefinitely
                await semaphore.WaitAsync(cancellationToken);
                acquired = true;
            }

            return acquired ? new LocalDistributedLockHandle(semaphore) : null;
        }
    }
}
