using Microsoft.Extensions.Logging;
using System;

namespace DotCommon.Logging
{
    /// <summary>日志扩展
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>根据日志级别记录
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="logLevel">日志级别</param>
        /// <param name="message">日志信息</param>
        public static void LogWithLevel(this ILogger logger, LogLevel logLevel, string message)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    logger.LogCritical(message);
                    break;
                case LogLevel.Error:
                    logger.LogError(message);
                    break;
                case LogLevel.Warning:
                    logger.LogWarning(message);
                    break;
                case LogLevel.Information:
                    logger.LogInformation(message);
                    break;
                case LogLevel.Trace:
                    logger.LogTrace(message);
                    break;
                default: // LogLevel.Debug || LogLevel.None
                    logger.LogDebug(message);
                    break;
            }
        }

        /// <summary>根据日志级别记录
        /// </summary>
        /// <param name="logger">Logger</param>
        /// <param name="logLevel">日志级别</param>
        /// <param name="message">日志信息</param>
        /// <param name="exception">异常信息</param>
        public static void LogWithLevel(this ILogger logger, LogLevel logLevel, string message, Exception exception)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    logger.LogCritical(exception, message);
                    break;
                case LogLevel.Error:
                    logger.LogError(exception, message);
                    break;
                case LogLevel.Warning:
                    logger.LogWarning(exception, message);
                    break;
                case LogLevel.Information:
                    logger.LogInformation(exception, message);
                    break;
                case LogLevel.Trace:
                    logger.LogTrace(exception, message);
                    break;
                default: // LogLevel.Debug || LogLevel.None
                    logger.LogDebug(message);
                    break;
            }
        }


    }
}
