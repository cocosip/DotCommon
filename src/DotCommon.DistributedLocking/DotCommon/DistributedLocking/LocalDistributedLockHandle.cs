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
        private bool _disposed;

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
            if (_disposed)
            {
                return;
            }

            _semaphore.Release();
            _disposed = true;
        }

        /// <summary>
        /// Releases the lock asynchronously.
        /// </summary>
        public ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return default;
            }

            _semaphore.Release();
            _disposed = true;
            return default;
        }
    }
}
