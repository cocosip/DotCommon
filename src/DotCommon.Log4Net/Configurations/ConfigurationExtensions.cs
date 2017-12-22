using DotCommon.Dependency;
using DotCommon.Extensions;
using DotCommon.Logging;

namespace DotCommon.Configurations
{
    public static class ConfigurationExtensions
    {
        /// <summary>使用Log4Net日志记录
        /// </summary>
        public static Configuration UseLog4Net(this Configuration configuration, string configFile = "")
        {
            var container = IocManager.GetContainer();
            if (configFile.IsNullOrEmpty())
            {
                configFile = "log4net.config";
            }
            container.Register<ILoggerFactory, Log4NetLoggerFactory>(new Log4NetLoggerFactory(configFile), isDefault: true);
            container.Register<ILogger, Log4NetLogger>(DependencyLifeStyle.Transient, isDefault: true);
            return configuration;
        }
    }
}
