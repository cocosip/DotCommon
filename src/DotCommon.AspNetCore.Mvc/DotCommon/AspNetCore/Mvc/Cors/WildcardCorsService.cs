using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.AspNetCore.Mvc.Cors
{
    /// <summary>
    /// A custom <see cref="CorsService"/> that supports wildcard origins (e.g., "*.example.com").
    /// </summary>
    public class WildcardCorsService : CorsService
    {
        /// <summary>
        /// The name of the wildcard CORS policy.
        /// </summary>
        public const string WildcardCorsPolicyName = "WildcardCors";

        /// <summary>
        /// Initializes a new instance of the <see cref="WildcardCorsService"/> class.
        /// </summary>
        /// <param name="options">The <see cref="IOptions{CorsOptions}"/>.</param>
        /// <param name="loggerFactory">The <see cref="ILoggerFactory"/>.</param>
        public WildcardCorsService(IOptions<CorsOptions> options, ILoggerFactory loggerFactory) : base(options, loggerFactory)
        {

        }

        /// <summary>
        /// Evaluates the CORS request for a simple cross-origin request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="policy">The <see cref="CorsPolicy"/> to evaluate.</param>
        /// <param name="result">The <see cref="CorsResult"/> to populate with the evaluation results.</param>
        public override void EvaluateRequest(HttpContext context, CorsPolicy policy, CorsResult result)
        {
            policy = WithWildcardOrigin(policy, context.Request.Headers[CorsConstants.Origin].ToString());
            base.EvaluateRequest(context, policy, result);
        }

        /// <summary>
        /// Evaluates the CORS request for a preflight cross-origin request.
        /// </summary>
        /// <param name="context">The <see cref="HttpContext"/> for the current request.</param>
        /// <param name="policy">The <see cref="CorsPolicy"/> to evaluate.</param>
        /// <param name="result">The <see cref="CorsResult"/> to populate with the evaluation results.</param>
        public override void EvaluatePreflightRequest(HttpContext context, CorsPolicy policy, CorsResult result)
        {
            policy = WithWildcardOrigin(policy, context.Request.Headers[CorsConstants.Origin].ToString());
            base.EvaluatePreflightRequest(context, policy, result);
        }

        /// <summary>
        /// Returns the policy unchanged when the request origin is not covered; otherwise a per-request
        /// copy of the policy that explicitly allows the origin. The shared policy instance is never mutated.
        /// </summary>
        private static CorsPolicy WithWildcardOrigin(CorsPolicy policy, string requestOrigin)
        {
            if (!IsOriginAllowed(policy.Origins, requestOrigin) ||
                policy.Origins.Contains(requestOrigin, StringComparer.OrdinalIgnoreCase))
            {
                return policy;
            }

            return new CorsPolicyBuilder(policy).WithOrigins(requestOrigin).Build();
        }

        /// <summary>
        /// Determines whether the request origin is explicitly allowed or matches a configured wildcard (e.g. "*.example.com").
        /// </summary>
        private static bool IsOriginAllowed(IList<string> allowedOrigins, string requestOrigin)
        {
            if (string.IsNullOrEmpty(requestOrigin))
            {
                return false;
            }

            if (allowedOrigins.Contains(requestOrigin, StringComparer.OrdinalIgnoreCase))
            {
                return true;
            }

            if (requestOrigin == "*")
            {
                return false;
            }

            if (!Uri.TryCreate(requestOrigin, UriKind.Absolute, out var uri))
            {
                return false;
            }

            var requestHost = uri.Host;
            if (string.IsNullOrEmpty(requestHost))
            {
                return false;
            }

            foreach (var wildcardDomain in allowedOrigins.Where(o => o.StartsWith("*")))
            {
                var baseDomain = wildcardDomain.Substring(1);
                if (requestHost.EndsWith(baseDomain, StringComparison.OrdinalIgnoreCase) ||
                    requestHost.Equals(baseDomain.TrimStart('.'), StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
