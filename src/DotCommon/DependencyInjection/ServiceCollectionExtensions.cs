using DotCommon.Http;
using DotCommon.Scheduling;
using DotCommon.Serializing;
using DotCommon.Threading;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DotCommon.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>注册通用组件
        /// </summary>
        public static IServiceCollection AddCommonComponents(this IServiceCollection services)
        {
            //http请求
            services.AddTransient<IHttpClient, HttpClient>();
            //json序列化
            services.AddTransient<IJsonSerializer, DefaultJsonSerializer>();
            //xml序列化
            services.AddTransient<IXmlSerializer, DefaultXmlSerializer>();
            //二进制序列化
            services.AddTransient<IBinarySerializer, DefaultBinarySerializer>();
            //对象序列化
            services.AddTransient<IObjectSerializer, DefaultObjectSerializer>();
            //定时器
            services.AddSingleton<IScheduleService, ScheduleService>();

            //生命周期管理
            services.AddSingleton<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance);
            services.AddSingleton<IAmbientDataContext, AsyncLocalAmbientDataContext>();
            services.AddSingleton(typeof(IAmbientScopeProvider<>), typeof(AmbientDataContextAmbientScopeProvider<>));
            return services;
        }

        /// <summary>如果没有注册就进行注册
        /// </summary>
        public static IServiceCollection AddServiceIfNotRegistered<T>(this IServiceCollection services, ServiceLifetime lifetime, Action<IServiceCollection> action)
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
        public static IServiceCollection AddServiceIfNotRegistered<T>(this IServiceCollection services, Func<ServiceDescriptor, bool> predicate, Action<IServiceCollection> action)
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
                .FirstOrDefault(d => d.ServiceType == typeof(T)) ?
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


    }
}
