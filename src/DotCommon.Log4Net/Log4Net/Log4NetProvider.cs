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
    public class Log4NetProvider : ILoggerProvider, IDisposable
    {
        private readonly ILoggerRepository _loggerRepository;
        private readonly ConcurrentDictionary<string, Log4NetLogger> _loggers = new ConcurrentDictionary<string, Log4NetLogger>();
        private readonly Log4NetProviderOptions _options;

        public Log4NetProvider() : this(new Log4NetProviderOptions())
        {

        }
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

        public Log4NetProvider(string configFile) : this(new Log4NetProviderOptions(configFile))
        {

        }

        public ILogger CreateLogger() => CreateLogger(_options.Name);
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

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Dispose();
                }
                disposedValue = true;
            }
        }

        // ~Log4NetProvider() {
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            //GC.SuppressFinalize(this);
        }
        #endregion


    }
}
