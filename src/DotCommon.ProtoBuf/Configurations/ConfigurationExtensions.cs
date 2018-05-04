using DotCommon.Dependency;
using DotCommon.Serializing;

namespace DotCommon.Configurations
{
    public static class ConfigurationExtensions
    {
        /// <summary>使用ProtoBuf
        /// </summary>
        public static Configuration UseProtoBuf(this Configuration configuration)
        {
            var container = IocManager.GetContainer();
            container.Register<IBinarySerializer, ProtocolBufSerializer>(DependencyLifeStyle.Transient);
            return configuration;
        }


    }
}
