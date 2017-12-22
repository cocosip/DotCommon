using DotCommon.Dependency;
using DotCommon.Serializing;

namespace DotCommon.Configurations
{
    public static class ConfigurationExtensions
    {
        /// <summary>使用Json4Net序列化
        /// </summary>
        public static Configuration UseJson4Net(this Configuration configuration)
        {
            var container = IocManager.GetContainer();
            container.Register<IJsonSerializer, NewtonsoftJsonSerializer>(DependencyLifeStyle.Transient, isDefault: true);
            return configuration;
        }
    }
}
