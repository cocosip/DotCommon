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

        /// <summary>注册DotCommon
        /// </summary>
        public static IServiceCollection AddDotCommon(this IServiceCollection services)
        {
            //添加DotCommonApplication
            var application = new DotCommonApplication(services);

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

    }
}
