using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Layout;
using log4net.Repository;
using System;
using System.IO;
using System.Reflection;

namespace DotCommon.Logging
{
    public class Log4NetLoggerFactory : ILoggerFactory
    {
        private readonly ILoggerRepository _loggerRepository;
        public Log4NetLoggerFactory(string configFile)
        {

            _loggerRepository = LogManager.CreateRepository(
               Assembly.GetEntryAssembly(),
               typeof(log4net.Repository.Hierarchy.Hierarchy));

            var file = new FileInfo(configFile);
            if (!file.Exists)
            {
#if NET45
                file = new FileInfo(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configFile));
#else
                file = new FileInfo(Path.Combine(AppContext.BaseDirectory, configFile));
#endif
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
            return new Log4NetLogger(LogManager.GetLogger(type));
        }
    }
}
