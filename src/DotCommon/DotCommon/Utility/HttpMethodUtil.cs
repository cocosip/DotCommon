using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace DotCommon.Utility
{
    /// <summary>
    /// A utility class for handling HTTP methods.
    /// </summary>
    public static class HttpMethodUtil
    {
        /// <summary>
        /// Defines the default HTTP verb used when a conventional verb cannot be determined.
        /// </summary>
        public const string DefaultHttpVerb = HttpVerbs.Post;

        /// <summary>
        /// A dictionary that maps HTTP verbs to their conventional method name prefixes.
        /// </summary>
        public static IReadOnlyDictionary<string, string[]> ConventionalPrefixes { get; } = new Dictionary<string, string[]>
        {
            { HttpVerbs.Get, new[] { "GetList", "GetAll", "Get" } },
            { HttpVerbs.Put, new[] { "Put", "Update" } },
            { HttpVerbs.Delete, new[] { "Delete", "Remove" } },
            { HttpVerbs.Post, new[] { "Create", "Add", "Insert", "Post" } },
            { HttpVerbs.Patch, new[] { "Patch" } }
        };

        /// <summary>
        /// Determines the conventional HTTP verb for a given method name based on its prefix.
        /// </summary>
        /// <param name="methodName">The name of the method to analyze.</param>
        /// <returns>The conventional HTTP verb (e.g., "GET", "POST"). Returns the <see cref="DefaultHttpVerb"/> if no conventional prefix is found.</returns>
        public static string GetConventionalVerbForMethodName(string methodName)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                return DefaultHttpVerb;
            }

            foreach (var kvp in ConventionalPrefixes)
            {
                if (kvp.Value.Any(prefix => methodName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                {
                    return kvp.Key;
                }
            }

            return DefaultHttpVerb;
        }

        /// <summary>
        /// Removes the conventional HTTP method prefix from a method name.
        /// </summary>
        /// <param name="methodName">The method name (e.g., "GetUser").</param>
        /// <param name="httpMethod">The HTTP method (e.g., "GET").</param>
        /// <returns>The method name without the conventional prefix.</returns>
        public static string RemoveHttpMethodPrefix(string methodName, string httpMethod)
        {
            if (string.IsNullOrEmpty(methodName) || string.IsNullOrEmpty(httpMethod))
            {
                return methodName;
            }

            if (!ConventionalPrefixes.TryGetValue(httpMethod, out var prefixes) || prefixes.Length == 0)
            {
                return methodName;
            }

            var prefix = prefixes.FirstOrDefault(p => methodName.StartsWith(p, StringComparison.OrdinalIgnoreCase));

            return prefix != null ? methodName.Substring(prefix.Length) : methodName;
        }

        /// <summary>
        /// Converts an HTTP verb string to its corresponding <see cref="HttpMethod"/> object.
        /// </summary>
        /// <param name="httpMethod">The HTTP verb string (e.g., "GET", "POST").</param>
        /// <returns>The corresponding <see cref="HttpMethod"/> object.</returns>
        /// <exception cref="ArgumentException">Thrown if the HTTP method is unknown.</exception>
        public static HttpMethod ConvertToHttpMethod(string httpMethod)
        {
            return (httpMethod.ToUpperInvariant()) switch
            {
                HttpVerbs.Get => HttpMethod.Get,
                HttpVerbs.Post => HttpMethod.Post,
                HttpVerbs.Put => HttpMethod.Put,
                HttpVerbs.Delete => HttpMethod.Delete,
                HttpVerbs.Options => HttpMethod.Options,
                HttpVerbs.Trace => HttpMethod.Trace,
                HttpVerbs.Head => HttpMethod.Head,
#if NETSTANDARD2_0
                HttpVerbs.Patch => new HttpMethod("PATCH"),
#else
                HttpVerbs.Patch => HttpMethod.Patch,
#endif
                _ => throw new ArgumentException("Unknown HTTP METHOD: " + httpMethod),
            };
        }
    }

    /// <summary>
    /// Defines constants for standard HTTP verbs.
    /// </summary>
    public static class HttpVerbs
    {
        public const string Get = "GET";
        public const string Post = "POST";
        public const string Put = "PUT";
        public const string Delete = "DELETE";
        public const string Options = "OPTIONS";
        public const string Trace = "TRACE";
        public const string Head = "HEAD";
        public const string Patch = "PATCH";
    }
}