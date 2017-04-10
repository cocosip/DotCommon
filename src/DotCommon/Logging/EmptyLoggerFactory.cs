#if NET45
using System;
namespace DotCommon.Logging
{
    /// <summary>An empty implementation of ILoggerFactory.
    /// </summary>
    public class EmptyLoggerFactory : ILoggerFactory
    {
        private static readonly EmptyLogger Logger = new EmptyLogger();

        public ILogger CreateLogger(string name)
        {
            return Logger;
        }

        public ILogger CreateLogger(Type type)
        {
            return Logger;
        }

        public ILogger CreateLogger<T>()
        {
            return Logger;
        }
    }
}
#endif