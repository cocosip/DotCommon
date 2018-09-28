using System.Threading;

namespace DotCommon.Threading
{
    public class NullCancellationTokenProvider : ICancellationTokenProvider
    {
        public static NullCancellationTokenProvider Instance { get; } = new NullCancellationTokenProvider();

        public CancellationToken Token { get; } = default;

        private NullCancellationTokenProvider()
        {

        }
    }
}
