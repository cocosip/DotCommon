using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.AspNetCore.Mvc.Cors
{
    /// <summary>通配符跨域
    /// </summary>
    public class WildcardCorsService : CorsService
    {
        /// <summary>通配符跨域代理名
        /// </summary>
        public const string WildcardCorsPolicyName = "WildcardCors";

        /// <summary>Ctor
        /// </summary>
        public WildcardCorsService(IOptions<CorsOptions> options, ILoggerFactory loggerFactory) : base(options, loggerFactory)
        {

        }

        /// <summary>EvaluateRequest
        /// </summary>
        public override void EvaluateRequest(HttpContext context, CorsPolicy policy, CorsResult result)
        {
            var origin = context.Request.Headers[CorsConstants.Origin];
            EvaluateOriginForWildcard(policy.Origins, origin);
            base.EvaluateRequest(context, policy, result);
        }

        /// <summary>EvaluatePreflightRequest
        /// </summary>
        public override void EvaluatePreflightRequest(HttpContext context, CorsPolicy policy, CorsResult result)
        {
            var origin = context.Request.Headers[CorsConstants.Origin];
            EvaluateOriginForWildcard(policy.Origins, origin);
            base.EvaluatePreflightRequest(context, policy, result);
        }

        private void EvaluateOriginForWildcard(IList<string> origins, string origin)
        {
            //只在没有匹配的origin的情况下进行操作
            if (!origins.Contains(origin))
            {
                //查询所有以星号开头的origin
                var wildcardDomains = origins.Where(o => o.StartsWith("*"));
                if (wildcardDomains.Any())
                {
                    //遍历以星号开头的origin
                    foreach (var wildcardDomain in wildcardDomains)
                    {
                        //如果以.cnblogs.com结尾
                        if (origin.EndsWith(wildcardDomain.Substring(1))
                            //或者以//cnblogs.com结尾，针对http://cnblogs.com
                            || origin.EndsWith("//" + wildcardDomain.Substring(2)) || origin == "*")
                        {
                            origins.Add(origin);
                            break;
                        }
                    }
                }
            }
        }
    }
}
