using System.Threading;

namespace DotCommon.Threading
{
    /// <summary>空的CancellationTokenProvider
    /// </summary>
    public class NullCancellationTokenProvider : ICancellationTokenProvider
    {
        /// <summary>CancellationTokenProvider实例
        /// </summary>
        public static NullCancellationTokenProvider Instance { get; } = new NullCancellationTokenProvider();

        /// <summary>CancellationToken
        /// </summary>
        public CancellationToken Token { get; } = default;

        private NullCancellationTokenProvider()
        {

        }
    }
}
