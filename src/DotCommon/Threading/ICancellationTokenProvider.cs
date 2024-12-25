using System.Threading;

namespace DotCommon.Threading
{
    /// <summary>
    /// ICancellationTokenProvider
    /// </summary>
    public interface ICancellationTokenProvider
    {
        /// <summary>
        /// CancellationToken
        /// </summary>
        CancellationToken Token { get; }
    }
}
