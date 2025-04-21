using System;
using System.Threading;

namespace DotCommon.Threading
{
    public interface ICancellationTokenProvider
    {
        CancellationToken Token { get; }
        IDisposable Use(CancellationToken cancellationToken);
    }
}
