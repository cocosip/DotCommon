using System.Threading;

namespace DotCommon.Threading
{
    /// <summary>
    /// NullCancellationTokenProvider
    /// </summary>
    public class NullCancellationTokenProvider : ICancellationTokenProvider
    {
        /// <summary>
        /// Instance
        /// </summary>
        public static NullCancellationTokenProvider Instance { get; } = new NullCancellationTokenProvider();

        /// <summary>
        /// CancellationToken
        /// </summary>
        public CancellationToken Token { get; } = default;

        private NullCancellationTokenProvider()
        {

        }
    }
}
