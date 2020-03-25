using DotCommon.Reflecting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DotCommon.DependencyInjection
{
    /// <summary>ServiceCollection公共扩展方法
    /// </summary>
    public static class ServiceCollectionCommonExtensions
    {
        /// <summary>如果没有注册就进行注册
        /// </summary>
        public static IServiceCollection WhenNull<T>(this IServiceCollection services, Action<IServiceCollection> action)
        {
            var serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(T));
            if (serviceDescriptor == null)
            {
                action(services);
            }
            return services;
        }

        /// <summary>如果没有注册就进行注册
        /// </summary>
        public static IServiceCollection WhenNull(this IServiceCollection services, Func<ServiceDescriptor, bool> predicate, Action<IServiceCollection> action)
        {
            var serviceDescriptor = services.FirstOrDefault(predicate);
            if (serviceDescriptor == null)
            {
                action(services);
            }
            return services;
        }

        /// <summary>替换现有的服务
        /// </summary>
        /// <typeparam name="TService">服务接口类型</typeparam>
        /// <typeparam name="TImplementation">实现类型</typeparam>
        /// <param name="services"></param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IServiceCollection Replace<TService, TImplementation>(
           this IServiceCollection services,
           ServiceLifetime lifetime) where TService : class where TImplementation : class, TService
        {
            var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == typeof(TService));
            if (descriptorToRemove != null)
            {
                services.Remove(descriptorToRemove);
            }
            var descriptorToAdd = new ServiceDescriptor(typeof(TService), typeof(TImplementation), lifetime);
            services.Add(descriptorToAdd);
            return services;
        }

        /// <summary>替换现有的服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="serviceType">接口类型</param>
        /// <param name="implType">实现类型</param>
        /// <param name="lifetime"></param>
        /// <returns></returns>
        public static IServiceCollection Replace(
           this IServiceCollection services, Type serviceType, Type implType,
           ServiceLifetime lifetime)
        {
            //判断服务类型是否为泛型
            if (implType.IsSubclassOf(serviceType) || serviceType.IsAssignableFrom(implType) || ReflectionUtil.IsAssignableToGenericType(implType, serviceType))
            {
                var descriptorToRemove = services.FirstOrDefault(d => d.ServiceType == serviceType);
                if (descriptorToRemove != null)
                {
                    services.Remove(descriptorToRemove);
                }
                var descriptorToAdd = new ServiceDescriptor(serviceType, implType, lifetime);
                services.Add(descriptorToAdd);
                return services;
            }
            throw new ArgumentException($"Type {implType} is not implment type {serviceType}!");
        }

        /// <summary>获取注册的Singleton对象的实例
        /// </summary>
        public static T GetSingletonInstanceOrNull<T>(this IServiceCollection services)
        {
            return (T)services
                .FirstOrDefault(d => d.ServiceType == typeof(T))?
                .ImplementationInstance;
        }

        /// <summary>获取注册的Singleton对象的实例
        /// </summary>
        public static T GetSingletonInstance<T>(this IServiceCollection services)
        {
            var service = services.GetSingletonInstanceOrNull<T>();
            if (service == null)
            {
                throw new InvalidOperationException("Could not find singleton service: " + typeof(T).AssemblyQualifiedName);
            }
            return service;
        }

        /// <summary>
        /// Resolves a dependency using given <see cref="IServiceCollection"/>.
        /// This method should be used only after dependency injection registration phase completed.
        /// </summary>
        internal static T GetService<T>(this IServiceCollection services)
        {
            return services
                .GetSingletonInstance<IDotCommonApplication>()
                .ServiceProvider
                .GetService<T>();
        }

        /// <summary>
        /// Resolves a dependency using given <see cref="IServiceCollection"/>.
        /// This method should be used only after dependency injection registration phase completed.
        /// </summary>
        internal static object GetService(this IServiceCollection services, Type type)
        {
            return services
                .GetSingletonInstance<IDotCommonApplication>()
                .ServiceProvider
                .GetService(type);
        }

        /// <summary>
        /// Resolves a dependency using given <see cref="IServiceCollection"/>.
        /// Throws exception if service is not registered.
        /// This method should be used only after dependency injection registration phase completed.
        /// </summary>
        internal static T GetRequiredService<T>(this IServiceCollection services)
        {
            return services
                .GetSingletonInstance<IDotCommonApplication>()
                .ServiceProvider
                .GetRequiredService<T>();
        }

        /// <summary>
        /// Resolves a dependency using given <see cref="IServiceCollection"/>.
        /// Throws exception if service is not registered.
        /// This method should be used only after dependency injection registration phase completed.
        /// </summary>
        internal static object GetRequiredService(this IServiceCollection services, Type type)
        {
            return services
                .GetSingletonInstance<IDotCommonApplication>()
                .ServiceProvider
                .GetRequiredService(type);
        }

        /// <summary>
        /// Returns a <see cref="Lazy{T}"/> to resolve a service from given <see cref="IServiceCollection"/>
        /// once dependency injection registration phase completed.
        /// </summary>
        public static Lazy<T> GetServiceLazy<T>(this IServiceCollection services)
        {
            return new Lazy<T>(services.GetService<T>, true);
        }

        /// <summary>
        /// Returns a <see cref="Lazy{T}"/> to resolve a service from given <see cref="IServiceCollection"/>
        /// once dependency injection registration phase completed.
        /// </summary>
        public static Lazy<object> GetServiceLazy(this IServiceCollection services, Type type)
        {
            return new Lazy<object>(() => services.GetService(type), true);
        }

        /// <summary>
        /// Returns a <see cref="Lazy{T}"/> to resolve a service from given <see cref="IServiceCollection"/>
        /// once dependency injection registration phase completed.
        /// </summary>
        public static Lazy<T> GetRequiredServiceLazy<T>(this IServiceCollection services)
        {
            return new Lazy<T>(services.GetRequiredService<T>, true);
        }

        /// <summary>
        /// Returns a <see cref="Lazy{T}"/> to resolve a service from given <see cref="IServiceCollection"/>
        /// once dependency injection registration phase completed.
        /// </summary>
        public static Lazy<object> GetRequiredServiceLazy(this IServiceCollection services, Type type)
        {
            return new Lazy<object>(() => services.GetRequiredService(type), true);
        }

    }
}
