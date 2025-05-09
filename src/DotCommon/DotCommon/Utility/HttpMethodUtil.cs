using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

namespace DotCommon.Utility
{
    /// <summary>
    /// HttpMethod工具类
    /// </summary>
    public static class HttpMethodUtil
    {
        /// <summary>
        /// 默认Http请求类型
        /// </summary>
        public const string DefaultHttpVerb = "POST";

        /// <summary>
        /// 常规的前缀
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
        /// 获取常规请求方法名
        /// </summary>
        /// <param name="methodName">请求方法名</param>
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
        /// 移除HttpMethod前缀
        /// </summary>
        /// <param name="methodName">方法名</param>
        /// <param name="httpMethod">HttpMethod</param>
        /// <returns></returns>
        public static string RemoveHttpMethodPrefix(string methodName, string httpMethod)
        {

            var prefixes = ConventionalPrefixes.GetOrDefault(httpMethod);
            if (prefixes.IsNullOrEmpty())
            {
                return methodName;
            }

            return methodName.RemovePreFix(prefixes!);
        }

        /// <summary>
        /// 将Http方法名转换为HttpMethod
        /// </summary>
        /// <param name="httpMethod">方法名</param>
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
