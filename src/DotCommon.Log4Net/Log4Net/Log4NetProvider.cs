using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.IO;

namespace DotCommon.Log4Net
{
    /// <summary>Log4Net日志管道
    /// </summary>
    public class Log4NetProvider : ILoggerProvider
    {
        private readonly ILoggerRepository _loggerRepository;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>();
        private readonly Log4NetProviderOptions _options;

        /// <summary>Ctor
        /// </summary>
        public Log4NetProvider() : this(new Log4NetProviderOptions())
        {

        }

        /// <summary>Ctor
        /// </summary>
        public Log4NetProvider(Log4NetProviderOptions options)
        {
            _options = options;
            _loggerRepository = LogManager.CreateRepository(options.LoggerRepositoryName);
            var file = new FileInfo(options.Log4NetConfigFile);
            if (!file.Exists)
            {
                file = new FileInfo(Path.Combine(AppContext.BaseDirectory, options.Log4NetConfigFile));
            }
            if (file.Exists)
            {
                XmlConfigurator.ConfigureAndWatch(_loggerRepository, file);
            }
            else
            {
                BasicConfigurator.Configure(_loggerRepository, new ConsoleAppender { Layout = new PatternLayout() });
            }
        }

        /// <summary>Ctor
        /// </summary>
        public Log4NetProvider(string configFile) : this(new Log4NetProviderOptions(configFile))
        {

        }

        /// <summary>创建日志记录对象
        /// </summary>
        public ILogger CreateLogger() => CreateLogger(_options.Name);

        /// <summary>创建日志记录对象
        /// </summary>
        public ILogger CreateLogger(string categoryName)
        {
            return _loggers.GetOrAdd(categoryName, CreateLoggerImplementation);
        }

        private Log4NetLogger CreateLoggerImplementation(string name)
        {
            var options = new Log4NetProviderOptions
            {
                Name = name,
                LoggerRepositoryName = _loggerRepository.Name
            };

            return new Log4NetLogger(options);
        }

        /// <summary>释放
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>释放
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            _loggers.Clear();
        }


    }
}
