using DotCommon.ObjectMapping;
using DotCommon.Scheduling;
using DotCommon.Serializing;
using DotCommon.Threading;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.DependencyInjection
{
    /// <summary>
    /// 依赖注入扩展类
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 注册DotCommon
        /// </summary>
        public static IServiceCollection AddDotCommon(this IServiceCollection services)
        {
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
