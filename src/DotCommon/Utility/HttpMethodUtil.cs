using DotCommon.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace DotCommon.Utility
{
    /// <summary>HttpMethod������
    /// </summary>
    public static class HttpMethodUtil
    {
        public const string DefaultHttpVerb = "POST";

        /// <summary>
        /// �����ǰ׺
        /// </summary>
        public static Dictionary<string, string[]> ConventionalPrefixes { get; } = new Dictionary<string, string[]>
        {
            { "GET", new [] { "GetList", "GetAll", "Get" } },
            { "PUT", new [] { "Put", "Update" } },
            { "DELETE", new [] { "Delete", "Remove" } },
            { "POST", new [] { "Create", "Add", "Insert", "Post" } },
            { "PATCH", new [] { "Patch" } }
        };

        /// <summary>
        /// ��ȡ�������󷽷���
        /// </summary>
        /// <param name="methodName">���󷽷���</param>
        /// <returns></returns>
        public static string GetConventionalVerbForMethodName(string methodName)
        {
            foreach (var conventionalPrefix in ConventionalPrefixes)
            {
                if (conventionalPrefix.Value.Any(prefix => methodName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase)))
                {
                    return conventionalPrefix.Key;
                }
            }

            return DefaultHttpVerb;
        }

        /// <summary>
        /// �Ƴ�HttpMethodǰ׺
        /// </summary>
        /// <param name="methodName">������</param>
        /// <param name="httpMethod">HttpMethod</param>
        /// <returns></returns>
        public static string RemoveHttpMethodPrefix(string methodName, string httpMethod)
        {

            var prefixes = ConventionalPrefixes.GetOrDefault(httpMethod);
            if (prefixes.IsNullOrEmpty())
            {
                return methodName;
            }

            return methodName.RemovePreFix(prefixes);
        }

        /// <summary>
        /// ��Http������ת��ΪHttpMethod
        /// </summary>
        /// <param name="httpMethod">������</param>
        /// <returns></returns>
        public static HttpMethod ConvertToHttpMethod(string httpMethod)
        {
            return (httpMethod.ToUpperInvariant()) switch
            {
                "GET" => HttpMethod.Get,
                "POST" => HttpMethod.Post,
                "PUT" => HttpMethod.Put,
                "DELETE" => HttpMethod.Delete,
                "OPTIONS" => HttpMethod.Options,
                "TRACE" => HttpMethod.Trace,
                "HEAD" => HttpMethod.Head,
                "PATCH" => new HttpMethod("PATCH"),
                _ => throw new ArgumentException("Unknown HTTP METHOD: " + httpMethod),
            };
        }
    }
}
