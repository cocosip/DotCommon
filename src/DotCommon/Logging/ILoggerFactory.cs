using System;

namespace DotCommon.Logging
{
    public interface ILoggerFactory
    {
        ILogger CreateLogger(string name);
        ILogger CreateLogger(Type type);
        ILogger CreateLogger<T>();
    }
}
