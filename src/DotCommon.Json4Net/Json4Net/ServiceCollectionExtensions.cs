using DotCommon.Serializing;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.Json4Net
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddJson4Net(this IServiceCollection services)
        {
            services.AddTransient<IJsonSerializer, NewtonsoftJsonSerializer>();
            return services;
        }
    }
}
