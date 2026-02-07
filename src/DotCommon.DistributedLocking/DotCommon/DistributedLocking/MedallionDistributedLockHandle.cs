using System;
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
        private bool _disposed;

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
            if (_disposed)
            {
                return;
            }

            _handle.Dispose();
            _disposed = true;
        }

        /// <summary>
        /// Releases the lock asynchronously.
        /// </summary>
        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                return;
            }

            await _handle.DisposeAsync();
            _disposed = true;
        }
    }
}
