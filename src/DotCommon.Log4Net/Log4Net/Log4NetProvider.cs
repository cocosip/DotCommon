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
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~Log4NetProvider() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            //GC.SuppressFinalize(this);
        }
        #endregion


    }
}
