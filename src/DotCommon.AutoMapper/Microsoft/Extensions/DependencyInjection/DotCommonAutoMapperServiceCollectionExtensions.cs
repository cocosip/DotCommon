using System;
using AutoMapper;
using AutoMapper.Internal;
using DotCommon.AutoMapper;
using DotCommon.ObjectMapping;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DotCommonAutoMapperServiceCollectionExtensions
    {
        public static IServiceCollection AddDotCommonAutoMapper(this IServiceCollection services)
        {
            services.AddSingleton<IConfigurationProvider>(sp =>
            {
                using (var scope = sp.CreateScope())
                {
                    var options = scope.ServiceProvider.GetRequiredService<IOptions<DotCommonAutoMapperOptions>>().Value;

                    var mapperConfigurationExpression = sp.GetRequiredService<IOptions<MapperConfigurationExpression>>().Value;
                    var autoMapperConfigurationContext = new DotCommonAutoMapperConfigurationContext(mapperConfigurationExpression, scope.ServiceProvider);

                    foreach (var configurator in options.Configurators)
                    {
                        configurator(autoMapperConfigurationContext);
                    }
                    var mapperConfiguration = new MapperConfiguration(mapperConfigurationExpression);

                    foreach (var profileType in options.ValidatingProfiles)
                    {
                        mapperConfiguration.Internal().AssertConfigurationIsValid(((Profile)Activator.CreateInstance(profileType)!).ProfileName);
                    }

                    return mapperConfiguration;
                }
            });

            services.AddTransient<IMapper>(sp => sp.GetRequiredService<IConfigurationProvider>().CreateMapper(sp.GetService));
            services.AddTransient<MapperAccessor>(sp => new MapperAccessor()
            {
                Mapper = sp.GetRequiredService<IMapper>()
            });
            services.AddTransient<IMapperAccessor>(provider => provider.GetRequiredService<MapperAccessor>());
            return services;
        }

        public static IServiceCollection AddAutoMapperObjectMapper(this IServiceCollection services)
        {
            return services.Replace(
                ServiceDescriptor.Transient<IAutoObjectMappingProvider, AutoMapperAutoObjectMappingProvider>()
            );
        }

        public static IServiceCollection AddAutoMapperObjectMapper<TContext>(this IServiceCollection services)
        {
            return services.Replace(
                ServiceDescriptor.Transient<IAutoObjectMappingProvider<TContext>, AutoMapperAutoObjectMappingProvider<TContext>>()
            );
        }
    }
}
