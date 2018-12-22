using DotCommon.DependencyInjection;
using Microsoft.AspNetCore.Builder;

namespace DotCommon.AspNetCore.Mvc
{
    public static class DotCommonApplicationBuilderExtensions
    {
        /// <summary>配置DotCommon
        /// </summary>
        public static IApplicationBuilder UseDotCommon(this IApplicationBuilder builder)
        {
            builder.ApplicationServices.UseDotCommon();
            return builder;
        }



    }
}
