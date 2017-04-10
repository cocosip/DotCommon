#if NET45

using System;
namespace DotCommon.Logging
{
    /// <summary>Represents a logger interface.
    /// </summary>
    public interface ILogger
    {
        /// <summary>Represents whether the debug log level is enabled.
        /// </summary>
        bool IsDebugEnabled { get; }
        /// <summary>Write a debug level log message.
        /// </summary>
        /// <param name="message"></param>
        void LogDebug(object message);
        /// <summary>Write a debug level log message.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void LogDebug(string format, params object[] args);
        /// <summary>Write a debug level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void LogDebug(object message, Exception exception);

        /// <summary>Write a info level log message.
        /// </summary>
        /// <param name="message"></param>
        void LogInformation(object message);
        /// <summary>Write a info level log message.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void LogInformation(string format, params object[] args);
        /// <summary>Write a info level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void LogInformation(object message, Exception exception);

        /// <summary>Write an error level log message.
        /// </summary>
        /// <param name="message"></param>
        void LogError(object message);
        /// <summary>Write an error level log message.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void LogError(string format, params object[] args);
        /// <summary>Write an error level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void LogError(object message, Exception exception);

        /// <summary>Write a warnning level log message.
        /// </summary>
        /// <param name="message"></param>
        void LogWarning(object message);
        /// <summary>Write a warnning level log message.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        void LogWarning(string format, params object[] args);
        /// <summary>Write a warnning level log message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="exception"></param>
        void LogWarning(object message, Exception exception);
 
    }
}
#endif