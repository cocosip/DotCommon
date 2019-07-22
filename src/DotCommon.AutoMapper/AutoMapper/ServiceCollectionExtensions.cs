using AutoMapper;
using DotCommon.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
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
            services
             .AddTransient<IObjectMapper, AutoMapperObjectMapper>();
            return services;
        }


    }
}
