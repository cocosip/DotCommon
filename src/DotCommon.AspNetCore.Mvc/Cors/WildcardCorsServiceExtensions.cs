using DotCommon.Extensions;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DotCommon.AspNetCore.Mvc.Cors
{
    /// <summary>通配符跨域服务扩展
    /// </summary>
    public static class WildcardCorsServiceExtensions
    {
        /// <summary>添加通配符跨域
        /// </summary>
        public static IServiceCollection AddWildcardCors(this IServiceCollection services, string origins, Action<CorsPolicyBuilder> configure = null)
        {
            services.Add(ServiceDescriptor.Transient<ICorsService, WildcardCorsService>());
            services.Configure<CorsOptions>(options => options.AddPolicy(WildcardCorsService.WildcardCorsPolicyName, builder =>
            {
                var originArray = origins.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray();
                configure?.Invoke(builder);
                if (configure == null)
                {
                    builder.WithOrigins(originArray)
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }
            }));

            return services;
        }
    }
}
