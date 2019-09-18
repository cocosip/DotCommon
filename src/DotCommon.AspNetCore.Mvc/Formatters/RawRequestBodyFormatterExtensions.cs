using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace DotCommon.AspNetCore.Mvc.Formatters
{
    /// <summary>RawRequestBody格式化扩展
    /// </summary>
    public static class RawRequestBodyFormatterExtensions
    {
        /// <summary>添加RawRequestBody格式化
        /// </summary>
        public static IServiceCollection AddRawRequestBodyFormatter(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(o => o.InputFormatters.Insert(0, new RawRequestBodyFormatter()));
            return services;
        }
    }
}
