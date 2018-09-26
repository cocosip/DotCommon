using log4net;
using log4net.Core;
using Microsoft.Extensions.Logging;
using System;


namespace DotCommon.Log4Net
{
    public class Log4NetLogger : Microsoft.Extensions.Logging.ILogger
    {
        private readonly log4net.ILog _log;
        private readonly Log4NetProviderOptions _options;
        public string Name => _log.Logger.Name;
        public Log4NetLogger() : this(new Log4NetProviderOptions())
        {

        }
        public Log4NetLogger(Log4NetProviderOptions options)
        {
            _options = options;
            _log = LogManager.GetLogger(options.LoggerRepositoryName, options.Name);
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return null;
        }

        /// <summary>判断是否开启记录
        /// </summary>
        public bool IsEnabled(LogLevel logLevel)
        {
            var convertLogLevel = ConvertLogLevel(logLevel);
            return _log.Logger.IsEnabledFor(convertLogLevel);
        }


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
