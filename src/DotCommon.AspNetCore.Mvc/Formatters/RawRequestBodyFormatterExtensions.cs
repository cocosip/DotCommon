using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.AspNetCore.Mvc.Formatters
{
    /// <summary>
    /// RawRequestBody formatter extensions
    /// </summary>
    public static class RawRequestBodyFormatterExtensions
    {
        /// <summary>
        /// Add RawRequestBody Formatter
        /// </summary>
        public static IServiceCollection AddRawRequestBodyFormatter(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(o => o.InputFormatters.Insert(0, new RawRequestBodyFormatter()));
            return services;
        }
    }
}
