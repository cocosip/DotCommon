using DotCommon.Serializing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace DotCommon.TextJson
{
    /// <summary>
    /// ServiceCollection扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加 System.Text.Json 序列化
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configure">序列化参数配置</param>
        /// <returns></returns>
        public static IServiceCollection AddTextJson(this IServiceCollection services, Action<JsonSerializerOptions> configure = null)
        {
            //默认配置
            static void defaultConfigure(JsonSerializerOptions c)
            {
                c.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
            }

            services
                .Configure<JsonSerializerOptions>(defaultConfigure)
                .AddTransient<IJsonSerializer, TextJsonSerializer>();

            if (configure != null)
            {
                services.Configure<JsonSerializerOptions>(configure);
            }

            return services;
        }
    }
}
