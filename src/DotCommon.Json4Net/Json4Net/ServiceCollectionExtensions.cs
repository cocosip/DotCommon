using DotCommon.Serializing;
using DotCommon.Utility;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.Json4Net
{

    /// <summary>ServiceCollection扩展方法
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>添加Json4Net序列化
        /// </summary>
        public static IServiceCollection AddJson4Net(this IServiceCollection services)
        {
            services.AddTransient<IJsonSerializer, NewtonsoftJsonSerializer>();
            return services;
        }
    }
}
