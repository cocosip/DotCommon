using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository;
using System;
using System.IO;

namespace DotCommon.Logging
{
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        private readonly ILoggerRepository _loggerRepository;
        public Log4NetLoggerFactory(string configFile, string loggerRepositoryName = "DotCommonRepository")
        {
            _loggerRepository = LogManager.CreateRepository(loggerRepositoryName);
            var file = new FileInfo(configFile);
            if (!file.Exists)
            {
                file = new FileInfo(Path.Combine(AppContext.BaseDirectory, configFile));
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

        public ILogger Create(string name)
        {
            return new Log4NetLogger(LogManager.GetLogger(_loggerRepository.Name, name));
        }
        public ILogger Create(Type type)
        {
            return new Log4NetLogger(LogManager.GetLogger(_loggerRepository.Name, type));
        }

        public ILog CreateILog(Type type)
        {
            return LogManager.GetLogger(_loggerRepository.Name, type);
        }

    }
}
