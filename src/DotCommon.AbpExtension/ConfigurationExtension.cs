using Castle.Windsor;
using DotCommon.Configurations;

namespace DotCommon.AbpExtension
{
    public static class ConfigurationExtension
    {
        /// <summary>使用Abp CastleWindsor
        /// </summary>
        public static Configuration UseAbpContainer(this Configuration configuration, IWindsorContainer container)
        {
            var iocContainer = new AbpIocContainer(container);
            return configuration;
        }
    }
}
