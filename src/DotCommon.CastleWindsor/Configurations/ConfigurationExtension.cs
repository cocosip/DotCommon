using Castle.Windsor;
using DotCommon.CastleWindsor;
using DotCommon.Dependency;

namespace DotCommon.Configurations
{
    public static class ConfigurationExtension
    {
        /// <summary>使用CastleWindsor
        /// </summary>
        public static Configuration UseCastleWindsorContainer(this Configuration configuration, IWindsorContainer container)
        {
            var iocContainer = new CastleWindsorIocContainer(container);
            IocManager.SetContainer(iocContainer);
            return configuration;
        }
    }
}
