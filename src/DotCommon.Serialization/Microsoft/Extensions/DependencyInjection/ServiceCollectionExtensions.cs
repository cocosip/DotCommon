using DotCommon.Serialization;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDotCommonSerialization(this IServiceCollection services)
        {
            services.AddTransient<IObjectSerializer, DefaultObjectSerializer>();
            return services;
        }

    }
}