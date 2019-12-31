using Microsoft.Extensions.Logging;
using System;

namespace DotCommon.Logging
{
    /// <summary>EmptyLogger
    /// </summary>
    public class EmptyLogger<T> : ILogger<T>
    {
        /// <summary>
        /// </summary>
        public IDisposable BeginScope<TState>(TState state)
        {
            return default(IDisposable);
        }

        /// <summary>
        /// </summary>
        public bool IsEnabled(LogLevel logLevel)
        {
            return false;
        }

        /// <summary>
        /// </summary>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }
    }
}
