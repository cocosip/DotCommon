using DotCommon.AspNetCore.Mvc.Cors;
using DotCommon.AspNetCore.Mvc.Formatters;
using DotCommon.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DotCommon.AspNetCore.Mvc
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>添加通配符跨域
        /// </summary>
        public static IServiceCollection AddWildcardCors(this IServiceCollection services, string origins, Action<CorsPolicyBuilder> configure = null)
        {
            services.Add(ServiceDescriptor.Transient<ICorsService, WildcardCorsService>());
            services.Configure<CorsOptions>(options => options.AddPolicy(WildcardCorsService.WildcardCorsPolicyName, builder =>
            {
                var originArray = origins.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray();
                if (configure == null)
                {
                    builder.WithOrigins(originArray)
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                }
                else
                {
                    configure(builder);
                }
            }));

            return services;
        }

        /// <summary>AddRawRequestBodyFormatter
        /// </summary>
        public static IServiceCollection AddRawRequestBodyFormatter(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(o => o.InputFormatters.Insert(0, new RawRequestBodyFormatter()));
            return services;
        }



    }
}
