using Microsoft.Extensions.Logging;
using System;

namespace DotCommon.Log4Net
{
    /// <summary>Log4Net日志管道扩展
    /// </summary>
    public static class Log4NetProviderExtensions
    {
        /// <summary>创建日志记录对象
        /// </summary>
        public static ILogger CreateLogger<T>(this ILoggerProvider provider) where T : class
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (!provider.GetType().IsAssignableFrom(typeof(Log4NetProvider)))
            {
                throw new ArgumentOutOfRangeException(nameof(provider), "The ILoggerProvider should be of type Log4NetProvider.");
            }

            return provider.CreateLogger(typeof(T).FullName);
        }
    }
}
