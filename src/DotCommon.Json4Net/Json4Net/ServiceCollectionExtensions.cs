using DotCommon.Serializing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace DotCommon.Json4Net
{

    /// <summary>
    /// ServiceCollection扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// 添加Json4Net序列化
        /// </summary>
        public static IServiceCollection AddJson4Net(this IServiceCollection services, Action<JsonSerializerSettings> configure = null)
        {
            //默认配置
            static void defaultConfigure(JsonSerializerSettings c)
            {
                c.Converters = new List<JsonConverter> { new IsoDateTimeConverter() };
                c.ContractResolver = new CustomContractResolver();
                c.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            }

            services
                .Configure<JsonSerializerSettings>(defaultConfigure)
                .AddTransient<IJsonSerializer, NewtonsoftJsonSerializer>();

            if (configure != null)
            {
                services.Configure<JsonSerializerSettings>(configure);
            }
            return services;
        }
    }
}
