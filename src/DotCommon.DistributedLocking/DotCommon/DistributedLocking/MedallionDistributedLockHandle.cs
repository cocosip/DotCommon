using System;
using System.Threading;
using System.Threading.Tasks;
using Medallion.Threading;

namespace DotCommon.DistributedLocking
{
    /// <summary>
    /// Medallion.Threading adapter implementation of <see cref="IDistributedLockHandle"/>.
    /// Wraps <see cref="IDistributedSynchronizationHandle"/>.
    /// </summary>
    internal class MedallionDistributedLockHandle : IDistributedLockHandle
    {
        private readonly IDistributedSynchronizationHandle _handle;
        private int _disposed;

        /// <summary>
        /// Initializes a new instance of the <see cref="MedallionDistributedLockHandle"/> class.
        /// </summary>
        /// <param name="handle">The Medallion lock handle to wrap</param>
        public MedallionDistributedLockHandle(IDistributedSynchronizationHandle handle)
        {
            _handle = handle;
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

            _handle.Dispose();
        }

        /// <summary>
        /// Releases the lock asynchronously.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (Interlocked.Exchange(ref _disposed, 1) != 0)
            {
                return;
            }

            await _handle.DisposeAsync();
        }
    }
}
