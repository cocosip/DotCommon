﻿using DotCommon.Dependency;
using DotCommon.Logging;
using DotCommon.Requests;
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

            //定时器
            container.Register<IScheduleService, ScheduleService>();

            //后台任务管理器
            container.Register<DotCommonTimer>(DependencyLifeStyle.Transient);
            container.Register<IBackgroundWorkerManager, BackgroundWorkerManager>();

            //Use(var ctx= ...)
            container.Register<IAmbientDataContext, AsyncLocalAmbientDataContext>();
            container.Register(typeof(IAmbientScopeProvider<>), typeof(DataContextAmbientScopeProvider<>), DependencyLifeStyle.Transient);

            return this;
        }

        /// <summary>注册定时任务
        /// </summary>
        public Configuration RegisterBackgroundWorkers(List<Assembly> assemblies)
        {
            var container = IocManager.GetContainer();

            var allTypies = assemblies.SelectMany(x => x.GetTypes());
            foreach (var type in allTypies)
            {
                if (typeof(IBackgroundWorker).IsAssignableFrom(type) && typeof(PeriodicBackgroundWorkerBase).IsAssignableFrom(type) && !type.GetTypeInfo().IsAbstract)
                {
                    container.Register(type, DependencyLifeStyle.Singleton);
                }
            }
            return this;
        }



    }
}
