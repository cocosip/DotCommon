using DotCommon.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace DotCommon.AspNetCore.Mvc
{
    /// <summary>DotCommon应用构建扩展
    /// </summary>
    public static class DotCommonApplicationBuilderExtensions
    {
        /// <summary>配置DotCommon
        /// </summary>
        public static IApplicationBuilder ConfigureDotCommon(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.ConfigureDotCommon();
            return builder;
        }



    }
}
