using DotCommon.Serializing;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.ProtoBuf
{

    /// <summary>依赖注入扩展
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>Protobuf序列化
        /// </summary>
        public static IServiceCollection AddProtoBuf(this IServiceCollection services)
        {
            services.AddTransient<IBinarySerializer, ProtocolBufSerializer>();
            return services;
        }
    }
}
