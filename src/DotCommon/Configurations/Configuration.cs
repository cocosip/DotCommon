using DotCommon.Dependency;
using DotCommon.Logging;
using DotCommon.Requests;
using DotCommon.Runtime;
using DotCommon.Runtime.Remoting;
using DotCommon.Scheduling;
using DotCommon.Serializing;

namespace DotCommon.Configurations
{
    /// <summary>配置
    /// </summary>
    public class Configuration
    {
        /// <summary>Provides the singleton access instance.
        /// </summary>
        public static Configuration Instance { get; private set; }
        private Configuration() { }

        public static Configuration Create()
        {
            Instance = new Configuration();
            return Instance;
        }

        public Configuration RegisterCommonComponent()
        {
            var container = IocManager.GetContainer();
            //模拟请求
            container.Register<IRequestClient, RequestClient>();
            //Json序列化(默认)
            container.Register<IJsonSerializer, DefaultJsonSerializer>(DependencyLifeStyle.Transient);
            //Xml序列化
            container.Register<IXmlSerializer, DefaultXmlSerializer>(DependencyLifeStyle.Transient);
            //二进制序列化
            container.Register<IBinarySerializer, DefaultBinarySerializer>(DependencyLifeStyle.Transient);

            //日志
            container.Register<ILoggerFactory, EmptyLoggerFactory>(DependencyLifeStyle.Singleton);
            //container.Register<ILogger, EmptyLogger>(DependencyLifeStyle.Transient);
            //container.Register<ILogger, EmptyLogger>(DependencyLifeStyle.Transient);

            //定时器
            container.Register<IScheduleService, ScheduleService>();

            //IAmbientDataContext
#if NET45
            container.Register<IAmbientDataContext, CallContextAmbientDataContext>();
#else
            container.Register<IAmbientDataContext, AsyncLocalAmbientDataContext>();
#endif
            container.Register(typeof(IAmbientScopeProvider<>), typeof(DataContextAmbientScopeProvider<>), DependencyLifeStyle.Transient);

            return this;
        }

    }
}
