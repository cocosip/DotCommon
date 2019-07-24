using AutoMapper;
using DotCommon.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Reflection;
using IObjectMapper = DotCommon.ObjectMapping.IObjectMapper;

namespace DotCommon.AutoMapper
{
    public static class ServiceCollectionExtensions
    {

        /// <summary>注册DotCommon的AutoMapper自动映射
        /// </summary>
        public static IServiceCollection AddDotCommonAutoMapper(this IServiceCollection services)
        {
            //IObjectMapper 映射
            var configuration = new AutoMapperConfiguration();
            services
             .AddSingleton<IAutoMapperConfiguration>(configuration)
             .AddTransient<IObjectMapper, AutoMapperObjectMapper>();
            return services;
        }

        /// <summary>注册AutoMapper
        /// </summary>
        public static IServiceCollection BuildAutoMapper(this IServiceCollection services)
        {
            if (services.FirstOrDefault(x => x.ServiceType == typeof(IConfigurationProvider)) != null)
            {
                throw new ArgumentOutOfRangeException("AutoMapper已经被注册");
            }

            var autoMapperConfiguration = services.GetSingletonInstance<IAutoMapperConfiguration>();

            Action<IMapperConfigurationExpression> configuror = configuration =>
            {
                foreach (var configurator in autoMapperConfiguration.Configurators)
                {
                    configurator(configuration);
                }
            };
            var config = new MapperConfiguration(configuror);

            services.AddSingleton<IConfigurationProvider>(config);
            services.AddSingleton<IMapper>(config.CreateMapper());
            return services;
        }


        /// <summary>添加配置信息
        /// </summary>
        public static IServiceCollection AddAutoMapperConfigurator(this IServiceCollection services, Action<IMapperConfigurationExpression> configurator)
        {
            var autoMapperConfiguration = services.GetSingletonInstance<IAutoMapperConfiguration>();
            autoMapperConfiguration.Configurators.Add(configurator);
            return services;
        }

        /// <summary>添加Assembly映射
        /// </summary>
        public static IServiceCollection AddAssemblyAutoMaps(this IServiceCollection services, params Assembly[] assemblies)
        {

            Action<IMapperConfigurationExpression> configurator = cfg =>
            {
                cfg.CreateAssemblyAutoMaps(assemblies);
            };
            return services.AddAutoMapperConfigurator(configurator);
        }


    }
}
