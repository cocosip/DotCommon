using log4net;
using log4net.Core;
using Microsoft.Extensions.Logging;
using System;


namespace DotCommon.Log4Net
{
    /// <summary>Log4Net日志记录
    /// </summary>
    public class Log4NetLogger : Microsoft.Extensions.Logging.ILogger
    {
        private readonly log4net.ILog _log;

        /// <summary>日志名
        /// </summary>
        public string Name => _log.Logger.Name;

        /// <summary>Ctor
        /// </summary>
        public Log4NetLogger() : this(new Log4NetProviderOptions())
        {

        }

        /// <summary>Ctor
        /// </summary>
        public Log4NetLogger(Log4NetProviderOptions options)
        {
            _log = LogManager.GetLogger(options.LoggerRepositoryName, options.Name);
        }

        /// <summary>BeginScope
        /// </summary>
        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>判断是否开启该级别的记录
        /// </summary>
        /// <param name="logLevel">日志级别</param>
        /// <returns></returns>
        public bool IsEnabled(LogLevel logLevel)
        {
            var convertLogLevel = ConvertLogLevel(logLevel);
            return _log.Logger.IsEnabledFor(convertLogLevel);
        }

        /// <summary>日志记录
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="logLevel">日志级别</param>
        /// <param name="eventId">事件Id</param>
        /// <param name="state">状态</param>
        /// <param name="exception">异常</param>
        /// <param name="formatter">格式化器</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            var message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {

                switch (logLevel)
                {
                    case LogLevel.Critical:
                        _log.Fatal(message, exception);
                        break;
                    case LogLevel.Debug:
                        _log.Debug(message, exception);
                        break;
                    case LogLevel.Error:
                        _log.Error(message, exception);
                        break;
                    case LogLevel.Information:
                        _log.Info(message, exception);
                        break;
                    case LogLevel.Warning:
                        _log.Warn(message, exception);
                        break;
                    default:
                        _log.Info(message, exception);
                        break;
                }
            }
        }

        /// <summary>将Microsoft.Extensions.Logging的日志级别转换成Log4Net日志级别
        /// </summary>
        /// <param name="logLevel">Microsoft.Extensions.Logging日志级别</param>
        /// <returns></returns>
        public Level ConvertLogLevel(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Trace:
                    return Level.Trace;
                case LogLevel.Debug:
                    return Level.Debug;
                case LogLevel.Information:
                    return Level.Info;
                case LogLevel.Warning:
                    return Level.Warn;
                case LogLevel.Error:
                    return Level.Error;
                case LogLevel.Critical:
                    return Level.Fatal;
                case LogLevel.None:
                    return Level.Off;
                default:
                    return Level.Debug;
            }
        }



    }
}
