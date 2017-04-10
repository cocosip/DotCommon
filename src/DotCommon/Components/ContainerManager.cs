using System;
using DotCommon.Components;
#if !NET45
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotCommon.Scheduling;
using DotCommon.Serializing;
using Microsoft.Extensions.DependencyInjection;
#endif
namespace DotCommon.Components
{
    /// <summary>Represents an object container.
    /// </summary>
    public class ContainerManager
    {
        /// <summary>Represents the current object container.
        /// </summary>
        public static IObjectContainer Current { get; private set; }

        /// <summary>Set the object container.
        /// </summary>
        /// <param name="container"></param>
        public static void SetContainer(IObjectContainer container)
        {
            Current = container;
        }

        public static TService Resolve<TService>() where TService : class
        {
            return Current.Resolve<TService>();
        }

        public static object Resolve(Type serviceType)
        {
            return Current.Resolve(serviceType);
        }

#if NET45

        public static void RegisterType(Type implementationType, string serviceName = null,
            LifeStyle life = LifeStyle.Singleton)
        {
            Current.RegisterType(implementationType, serviceName, life);
        }

        public static void RegisterType(Type serviceType, Type implementationType, string serviceName = null,
            LifeStyle life = LifeStyle.Singleton)
        {
            Current.RegisterType(serviceType, implementationType, serviceName, life);
        }

        public static void Register<TService, TImplementer>(string serviceName = null,
            LifeStyle life = LifeStyle.Singleton)
            where TService : class
            where TImplementer : class, TService
        {
            Current.Register<TService, TImplementer>(serviceName, life);
        }

        public static void RegisterInstance<TService, TImplementer>(TImplementer instance, string serviceName = null)
            where TService : class
            where TImplementer : class, TService
        {
            Current.RegisterInstance<TService, TImplementer>(instance, serviceName);
        }


        public static bool TryResolve<TService>(out TService instance) where TService : class
        {
            return Current.TryResolve<TService>(out instance);
        }

        public static bool TryResolve(Type serviceType, out object instance)
        {
            return Current.TryResolve(serviceType, out instance);
        }

        public static TService ResolveNamed<TService>(string serviceName) where TService : class
        {
            return Current.ResolveNamed<TService>(serviceName);
        }

        public static object ResolveNamed(string serviceName, Type serviceType)
        {
            return Current.ResolveNamed(serviceName, serviceType);
        }

        public static bool TryResolveNamed(string serviceName, Type serviceType, out object instance)
        {
            return Current.TryResolveNamed(serviceName, serviceType, out instance);
        }
#else
        public static void RegisterType<TService, TImplementer>(LifeStyle life = LifeStyle.Singleton)
            where TService : class where TImplementer : class, TService
        {
            Current.RegisterType<TService, TImplementer>(life);
        }

        public static void RegisterType(Type serviceType, Type implementationType,
            LifeStyle life = LifeStyle.Singleton)
        {
            Current.RegisterType(serviceType, implementationType, life);
        }

        public static IServiceProvider GetProvider()
        {
            return Current.Provider;
        }

        public static void RegisterLogging()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddLogging();
        }
             /// <summary>注册通用业务
        /// </summary>
        public static void RegisterCommonComponents()
        {
            RegisterType<IScheduleService, ScheduleService>(LifeStyle.Transient);
            RegisterType<IJsonSerializer, JsonSerializer>();
            RegisterType<IXmlSerializer, XmlSerializer>();
        }

        /// <summary>注册所有包含Component标签的依赖
        /// </summary>
        public static void RegisterBusinessComponents(params Assembly[] assemblies)
        {
            var registedTypes = new List<Type>();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes().Where(TypeUtil.IsComponent))
                {
                    if (!registedTypes.Contains(type))
                    {
                        RegisterComponentType(type);
                        registedTypes.Add(type);
                    }
                }
            }
        }

        /// <summary>注册某个类型
        /// </summary>
        private static void RegisterComponentType(Type type)
        {
            var life = ParseLife(type);
            foreach (var interfaceType in type.GetTypeInfo().GetInterfaces())
            {
                RegisterType(interfaceType, type, life);
            }
        }

        private static LifeStyle ParseLife(Type type)
        {
            var attribute = type.GetTypeInfo().GetCustomAttribute<ComponentAttribute>(false);
            return attribute?.LifeStyle ?? LifeStyle.Singleton;
        }
#endif
    }

}
