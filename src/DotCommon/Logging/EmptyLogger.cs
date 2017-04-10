#if NET45
using System;

namespace DotCommon.Logging
{
    /// <summary>An empty logger which log nothing.
    /// </summary>
    public class EmptyLogger : ILogger
    {
        #region ILogger Members

        public bool IsDebugEnabled => false;

        public void LogDebug(object message)
        {
        }
        public void LogDebug(string format, params object[] args)
        {
        }
        public void LogDebug(object message, Exception exception)
        {
        }
        public void LogInformation(object message)
        {
        }
        public void LogInformation(string format, params object[] args)
        {
        }
        public void LogInformation(object message, Exception exception)
        {
        }
        public void LogError(object message)
        {
        }
        public void LogError(string format, params object[] args)
        {
        }
        public void LogError(object message, Exception exception)
        {
        }
        public void LogWarning(object message)
        {
        }
        public void LogWarning(string format, params object[] args)
        {
        }
        public void LogWarning(object message, Exception exception)
        {

        }

        #endregion
    }
}
#endif