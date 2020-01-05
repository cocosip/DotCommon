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
        public static IServiceCollection AddServiceWhenNull<T>(this IServiceCollection services, ServiceLifetime lifetime, Action<IServiceCollection> action)
        {
            var serviceDescriptor = services.FirstOrDefault(d => d.ServiceType == typeof(T) && d.Lifetime == lifetime);
            if (serviceDescriptor == null)
            {
                action(services);
            }
            return services;
        }

        /// <summary>如果没有注册就进行注册
        /// </summary>
        public static IServiceCollection AddServiceWhenNull(this IServiceCollection services, Func<ServiceDescriptor, bool> predicate, Action<IServiceCollection> action)
        {
            var serviceDescriptor = services.FirstOrDefault(predicate);
            if (serviceDescriptor == null)
            {
                action(services);
            }
            return services;
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
