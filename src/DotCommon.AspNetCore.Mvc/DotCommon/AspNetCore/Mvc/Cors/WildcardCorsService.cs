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
            var origin = context.Request.Headers[CorsConstants.Origin].ToString();
            EvaluateOriginForWildcard(policy.Origins, origin);
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
            var origin = context.Request.Headers[CorsConstants.Origin].ToString();
            EvaluateOriginForWildcard(policy.Origins, origin);
            base.EvaluatePreflightRequest(context, policy, result);
        }

        /// <summary>
        /// Evaluates if the incoming origin matches any wildcard origins defined in the policy.
        /// If a match is found, the actual origin is added to the policy's allowed origins for the current request.
        /// </summary>
        /// <param name="allowedOrigins">The list of allowed origins from the CORS policy.</param>
        /// <param name="requestOrigin">The origin from the current request's 'Origin' header.</param>
        private void EvaluateOriginForWildcard(IList<string> allowedOrigins, string requestOrigin)
        {
            // Only proceed if the exact request origin is not already explicitly allowed.
            if (!allowedOrigins.Contains(requestOrigin, StringComparer.OrdinalIgnoreCase))
            {
                // Handle the special case of "*" origin
                if (requestOrigin == "*")
                {
                    if (allowedOrigins.Contains("*"))
                    {
                        allowedOrigins.Add(requestOrigin);
                    }
                    return;
                }

                // Try to parse the request origin as a URI to get the host
                string? requestHost = null;
                if (Uri.TryCreate(requestOrigin, UriKind.Absolute, out var uri))
                {
                    requestHost = uri.Host;
                }

                if (string.IsNullOrEmpty(requestHost))
                {
                    return; // Cannot determine host, skip wildcard evaluation
                }

                // Find all configured wildcard domains (e.g., "*.example.com").
                var wildcardDomains = allowedOrigins.Where(o => o.StartsWith("*"));
                if (wildcardDomains.Any())
                {
                    foreach (var wildcardDomain in wildcardDomains)
                    {
                        // Extract the base domain part from the wildcard (e.g., ".example.com" from "*.example.com")
                        var baseDomain = wildcardDomain.Substring(1);

                        // Check if the incoming request host is the base domain itself (e.g., "example.com")
                        // or a subdomain of the base domain (e.g., "sub.example.com" ends with ".example.com")
                        // The StringComparison.OrdinalIgnoreCase is crucial for case-insensitive domain matching.
                        if (requestHost.EndsWith(baseDomain, StringComparison.OrdinalIgnoreCase) ||
                            requestHost.Equals(baseDomain.TrimStart('.'), StringComparison.OrdinalIgnoreCase))
                        {
                            // If a match is found, add the actual request origin to the allowed origins
                            // so that the base CorsService can then successfully validate it.
                            allowedOrigins.Add(requestOrigin);
                            break; // Found a match, no need to check other wildcards
                        }
                    }
                }
            }
        }
    }
}