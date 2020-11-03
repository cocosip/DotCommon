using System.Threading;

namespace DotCommon.Threading
{
    public static class CancellationTokenProviderExtensions
    {
        /// <summary>
        /// 失败回调操作
        /// </summary>
        public static CancellationToken FallbackToProvider(this ICancellationTokenProvider provider, CancellationToken prefferedValue = default)
        {
            return prefferedValue == default || prefferedValue == CancellationToken.None
                ? provider.Token
                : prefferedValue;
        }
    }
}
