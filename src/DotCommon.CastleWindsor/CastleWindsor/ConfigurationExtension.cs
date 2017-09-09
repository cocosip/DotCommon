using Castle.Windsor;
using DotCommon.Configurations;

namespace DotCommon.CastleWindsor
{
    public static class ConfigurationExtension
    {
        /// <summary>使用CastleWindsor
        /// </summary>
        public static Configuration UseCastleWindsor(this Configuration configuration, IWindsorContainer container)
        {
            var iocContainer = new CastleIocContainer(container);
            return configuration;
        }
    }
}
