using DotCommon.Dependency;
using DotCommon.Extensions;
using DotCommon.Http;
using DotCommon.Logging;
using DotCommon.Runtime;
using DotCommon.Runtime.Remoting;
using DotCommon.Scheduling;
using DotCommon.Serializing;
using DotCommon.Threading.BackgroundWorkers;
using DotCommon.Threading.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DotCommon.Configurations
{
    /// <summary>配置
    /// </summary>
    public class Configuration
    {
        public static Configuration Instance { get; private set; }
        public StartupConfiguration Startup { get; set; } = new StartupConfiguration();
        private Configuration() { }

        public static Configuration Create()
        {
            Instance = new Configuration();
            return Instance;
        }

        public Configuration RegisterCommonComponent()
        {
            var container = IocManager.GetContainer();
            //Http请求
            container.Register<IHttpClient, DefaultHttpClient>(DependencyLifeStyle.Transient);
            //Json序列化(默认)
            container.Register<IJsonSerializer, DefaultJsonSerializer>(DependencyLifeStyle.Transient);
            //Xml序列化
            container.Register<IXmlSerializer, DefaultXmlSerializer>(DependencyLifeStyle.Transient);
            //二进制序列化
            container.Register<IBinarySerializer, DefaultBinarySerializer>(DependencyLifeStyle.Transient);

            //日志
            container.Register<ILoggerFactory, EmptyLoggerFactory>(DependencyLifeStyle.Singleton);
            //container.Register<ILogger, EmptyLogger>(DependencyLifeStyle.Transient);

            //定时器
            container.Register<IScheduleService, ScheduleService>();

            //后台定时任务
            container.Register<DotCommonTimer>(DependencyLifeStyle.Transient);
            container.Register<IBackgroundWorkerManager, BackgroundWorkerManager>();

            //Use(var ctx= ...)
            container.Register<IAmbientDataContext, AsyncLocalAmbientDataContext>();
            container.Register(typeof(IAmbientScopeProvider<>), typeof(DataContextAmbientScopeProvider<>), DependencyLifeStyle.Transient);

            return this;
        }

        /// <summary>注册定时任务
        /// </summary>
        public Configuration RegisterPeriodicBackgroundWorkers(List<Assembly> assemblies)
        {
            var container = IocManager.GetContainer();
            var allTypies = assemblies.SelectMany(x => x.GetTypes());
            foreach (var type in allTypies)
            {
                if (typeof(IBackgroundWorker).IsAssignableFrom(type) && typeof(PeriodicBackgroundWorkerBase).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                {
                    container.Register(type, DependencyLifeStyle.Singleton);
                    //后台工作任务
                    Startup.BackgroundWorker.AddWorkerType(type);
                }
            }
            return this;
        }

        /// <summary>将全部的BackgroundWorkers添加到管理选项中,并且开始运行
        /// </summary>
        public Configuration BackgroundWorkersAttechAndRun()
        {
            var backgroundWorkTypies = Startup.BackgroundWorker.GetWorkerTypies();
            var container = IocManager.GetContainer();

            var manager = container.Resolve<IBackgroundWorkerManager>();
            foreach (var backgroundWorkType in backgroundWorkTypies)
            {
                var backgroundWorker = container.Resolve(backgroundWorkType).As<IBackgroundWorker>();
                manager.Add(backgroundWorker);
            }
            manager.Start();
            return this;
        }

    }
}
