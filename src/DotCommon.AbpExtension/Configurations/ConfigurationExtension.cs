using Castle.Windsor;
using DotCommon.AbpExtension;
using DotCommon.Configurations;
using DotCommon.Dependency;

namespace DotCommon.Configurations
{
    public static class ConfigurationExtension
    {
        /// <summary>使用Abp CastleWindsor
        /// </summary>
        public static Configuration UseAbpContainer(this Configuration configuration, IWindsorContainer container)
        {
            var iocContainer = new AbpIocContainer(container);
            IocManager.SetContainer(iocContainer);
            return configuration;
        }
    }
}
