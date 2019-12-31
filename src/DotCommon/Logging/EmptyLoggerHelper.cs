using Microsoft.Extensions.Logging;

namespace DotCommon.Logging
{
    /// <summary>EmptyLoggerHelper
    /// </summary>
    public static class EmptyLoggerHelper
    {
        /// <summary>Get logger
        /// </summary>
        public static ILogger<T> GetLogger<T>()
        {
            return new EmptyLogger<T>();
        }
    }
}
