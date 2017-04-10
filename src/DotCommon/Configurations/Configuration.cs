#if NET45

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotCommon.Components;
using DotCommon.Logging;
using DotCommon.Scheduling;
using DotCommon.Serializing;

namespace DotCommon.Configurations
{
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

        public Configuration SetDefault<TService, TImplementer>(string serviceName = null,
            LifeStyle life = LifeStyle.Singleton)
            where TService : class
            where TImplementer : class, TService
        {
            ContainerManager.Register<TService, TImplementer>(serviceName, life);
            return this;
        }

        public Configuration SetDefault<TService, TImplementer>(TImplementer instance, string serviceName = null)
            where TService : class
            where TImplementer : class, TService
        {
            ContainerManager.RegisterInstance<TService, TImplementer>(instance, serviceName);
            return this;
        }

        public Configuration RegisterCommonComponents()
        {
            SetDefault<ILoggerFactory, EmptyLoggerFactory>();
            SetDefault<IJsonSerializer, JsonSerializer>();
            SetDefault<IXmlSerializer, XmlSerializer>();
            SetDefault<IBinarySerializer, BinarySerializer>();
            SetDefault<IScheduleService, ScheduleService>(null, LifeStyle.Transient);
            // SetDefault<IMessageFramer, LengthPrefixMessageFramer>(null, LifeStyle.Transient);
            return this;
        }

        public Configuration RegisterUnhandledExceptionHandler()
        {
            var logger = ContainerManager.Resolve<ILoggerFactory>().CreateLogger(GetType().FullName);
            AppDomain.CurrentDomain.UnhandledException +=
                (sender, e) => logger.LogError("Unhandled exception: {0}", e.ExceptionObject);
            return this;
        }

        /// <summary>根据条件注册
        /// </summary>
        public Configuration RegisterByPredicate(Func<Type, bool> predicate, params Assembly[] assemblies)
        {
            var registeredTypies = new List<Type>();
            foreach (var assembly in assemblies)
            {

                foreach (var type in assembly.GetTypes().Where(predicate))
                {
                    if (!registeredTypies.Contains(type))
                    {
                        RegisterComponentType(type);
                    }
                    else
                    {
                        registeredTypies.Add(type);
                    }
                }
            }
            return this;
        }


        #region 注册包含特性标签的组件
        public Configuration RegisterBusinessComponents(params Assembly[] assemblies)
        {
            var registeredTypies = new List<Type>();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(TypeUtil.IsComponent))
                {
                    if (!registeredTypies.Contains(type))
                    {
                        RegisterComponentType(type);
                    }
                    else
                    {
                        registeredTypies.Add(type);
                    }
                }
            }
            return this;
        }

        private void RegisterComponentType(Type type)
        {
            var life = ParseComponentLife(type);
            ContainerManager.RegisterType(type, null, life);
            foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
            {
                ContainerManager.RegisterType(interfaceType, type, null, life);
            }
        }
        private static LifeStyle ParseComponentLife(Type type)
        {
            var attributes = type.GetCustomAttributes<ComponentAttribute>(false);
            var componentAttributes = attributes as IList<ComponentAttribute> ?? attributes.ToList();
            if (componentAttributes.Any())
            {
                return componentAttributes.First().LifeStyle;
            }
            return LifeStyle.Singleton;
        }
        #endregion
    }
}
#endif