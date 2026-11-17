using System;
using System.Threading;
using System.Threading.Tasks;

namespace DotCommon.DistributedLocking
{
    /// <summary>
    /// Local (in-process) implementation of <see cref="IDistributedLockHandle"/>.
    /// </summary>
    internal class LocalDistributedLockHandle : IDistributedLockHandle
    {
        private readonly SemaphoreSlim _semaphore;
        private int _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalDistributedLockHandle"/> class.
        /// </summary>
        /// <param name="semaphore">The semaphore to release when disposed</param>
        public LocalDistributedLockHandle(SemaphoreSlim semaphore)
        {
            _semaphore = semaphore;
        }

        /// <summary>
        /// Releases the lock synchronously.
        /// </summary>
        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
            {
                return;
            }

            _semaphore.Release();
        }

        /// <summary>
        /// Releases the lock asynchronously.
        /// </summary>
        public ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
            {
                return default;
            }

            _semaphore.Release();
            return default;
        }
    }
}
