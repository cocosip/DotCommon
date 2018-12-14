using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.AspNetCore.Mvc.Formatters
{
    public static class RawRequestBodyFormatterExtensions
    {
        /// <summary>AddRawRequestBodyFormatter
        /// </summary>
        public static IServiceCollection AddRawRequestBodyFormatter(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(o => o.InputFormatters.Insert(0, new RawRequestBodyFormatter()));
            return services;
        }
    }
}
