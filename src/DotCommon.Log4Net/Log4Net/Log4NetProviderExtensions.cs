using Microsoft.Extensions.Logging;
using System;

namespace DotCommon.Log4Net
{
    public static class Log4NetProviderExtensions
    {
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
