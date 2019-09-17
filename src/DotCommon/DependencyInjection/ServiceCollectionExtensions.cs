using DotCommon.ObjectMapping;
using DotCommon.Scheduling;
using DotCommon.Serializing;
using DotCommon.Threading;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DotCommon.DependencyInjection
{
    /// <summary>ServiceCollection扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>注册DotCommon
        /// </summary>
        public static IServiceCollection AddDotCommon(this IServiceCollection services)
        {
            //添加DotCommonApplication
            var application = new DotCommonApplication(services);
            services.TryAddObjectAccessor<IServiceProvider>();
            services.AddSingleton<IDotCommonApplication>(application);

            //json序列化
            services
                .AddTransient<IJsonSerializer, DefaultJsonSerializer>()
                .AddTransient<IXmlSerializer, DefaultXmlSerializer>()
                .AddTransient<IBinarySerializer, DefaultBinarySerializer>()
                .AddTransient<IObjectSerializer, DefaultObjectSerializer>()
                .AddSingleton<IScheduleService, ScheduleService>()
                .AddSingleton<IObjectMapper, NullObjectMapper>()
                //生命周期管理
                .AddSingleton<ICancellationTokenProvider>(NullCancellationTokenProvider.Instance)
                .AddSingleton<IAmbientDataContext, AsyncLocalAmbientDataContext>()
                .AddSingleton(typeof(IAmbientScopeProvider<>), typeof(AmbientDataContextAmbientScopeProvider<>))
                ;
            return services;
        }

    }
}
