﻿using DotCommon.Dependency;
using DotCommon.Extensions;
using DotCommon.Logging;
using DotCommon.Requests;
using DotCommon.Runtime;
using DotCommon.Runtime.Caching;
using DotCommon.Runtime.Caching.Configuration;
using DotCommon.Runtime.Caching.Memory;
using DotCommon.Runtime.Remoting;
using DotCommon.Scheduling;
using DotCommon.Serializing;
using System;

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
            container.Register<ILogger, EmptyLogger>(DependencyLifeStyle.Transient);
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

            //缓存
            container.Register<ICachingConfiguration, CachingConfiguration>();
            container.Register<ICacheManager, DotCommonMemoryCacheManager>();
            container.Register<DotCommonMemoryCache>(DependencyLifeStyle.Transient);
            return this;
        }

        #region 使用Json4Net
        /// <summary>使用Json4Net
        /// </summary>
        public Configuration UseJson4Net()
        {
            var container = IocManager.GetContainer();
            container.Register<IJsonSerializer, NewtonsoftJsonSerializer>(DependencyLifeStyle.Transient);
            return this;
        }

        #endregion

        #region 使用Protobuf.net
        /// <summary>使用ProtoBuf
        /// </summary>
        public Configuration UseProtoBuf()
        {
            var container = IocManager.GetContainer();
            container.Register<IBinarySerializer, ProtocolBufSerializer>(DependencyLifeStyle.Transient);
            return this;
        }
        #endregion



        #region Log4Net配置
        public Configuration UseLog4Net(string configFile = "")
        {
            var container = IocManager.GetContainer();
            if (configFile.IsNullOrEmpty())
            {
                configFile = "log4net.config";
            }
            container.Register<ILoggerFactory, Log4NetLoggerFactory>(new Log4NetLoggerFactory(configFile));
            return this;
        }
        #endregion


        /// <summary>缓存配置
        /// </summary>
        public Configuration CacheConfigure(string cacheName, Action<ICache> initAction)
        {
            var container = IocManager.GetContainer();
            container.Resolve<ICachingConfiguration>().Configure(cacheName, initAction);
            return this;
        }

        /// <summary>缓存配置
        /// </summary>
        public void CacheConfigureAll(Action<ICache> initAction)
        {
            var container = IocManager.GetContainer();
            container.Resolve<ICachingConfiguration>().ConfigureAll(initAction);
        }


    }
}