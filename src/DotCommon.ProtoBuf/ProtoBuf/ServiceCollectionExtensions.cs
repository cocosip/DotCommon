using DotCommon.Serializing;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.ProtoBuf
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProtoBuf(this IServiceCollection services)
        {
            services.AddTransient<IBinarySerializer, ProtocolBufSerializer>();
            return services;
        }
    }
}
