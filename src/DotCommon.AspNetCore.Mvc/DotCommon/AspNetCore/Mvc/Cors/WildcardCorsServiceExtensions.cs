using System;
using System.Linq;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.AspNetCore.Mvc.Cors
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/> to add wildcard CORS services.
    /// </summary>
    public static class WildcardCorsServiceExtensions
    {
        /// <summary>
        /// Adds wildcard CORS services to the specified <see cref="IServiceCollection"/>.
        /// This allows for origins with wildcard subdomains (e.g., "*.example.com").
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add services to.</param>
        /// <param name="origins">A comma-separated string of origins, which can include wildcard subdomains (e.g., "http://localhost:5000,*.example.com").</param>
        /// <param name="configure">An optional action to configure the <see cref="CorsPolicyBuilder"/>.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
        public static IServiceCollection AddWildcardCors(this IServiceCollection services, string origins, Action<CorsPolicyBuilder>? configure = null)
        {
            services.Add(ServiceDescriptor.Transient<ICorsService, WildcardCorsService>());
            services.Configure<CorsOptions>(options => options.AddPolicy(WildcardCorsService.WildcardCorsPolicyName, builder =>
            {
                var originArray = origins.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray();

                // Always apply the origins from the input string
                builder.WithOrigins(originArray);

                // Apply additional configuration if provided
                configure?.Invoke(builder);

                // If no custom configuration is provided, set up a default policy
                if (configure == null)
                {
                    builder.AllowAnyHeader()
                           .AllowAnyMethod()
                           .AllowCredentials(); // Allow credentials by default for more flexibility
                }
            }));

            return services;
        }
    }
}