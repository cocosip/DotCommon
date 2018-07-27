using System;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;

namespace DotCommon.Http
{
    public static class HttpRequestExtensions
    {
        /// <summary>添加AddReferer
        /// </summary>
        public static IHttpRequest AddReferer(this IHttpRequest request, string value)
        {
            return request.AddParameter("Referer", value, ParameterType.HttpHeader);
        }

        /// <summary>添加User-Agent
        /// </summary>
        public static IHttpRequest AddUserAgent(this IHttpRequest request, string value)
        {
            return request.AddParameter("User-Agent", value, ParameterType.HttpHeader);
        }

        /// <summary>添加Accept
        /// </summary>
        public static IHttpRequest AddAccept(this IHttpRequest request, string value)
        {
            //如果添加了Accepts
            request.AcceptTypes.Add(value);
            var accepts = string.Join(", ", request.AcceptTypes.ToArray());
            return request.AddOrUpdateParameter("Accept", accepts, ParameterType.HttpHeader);
        }

        /// <summary>直接使用固定完成的Accepts
        /// </summary>
        public static IHttpRequest UseAccept(this IHttpRequest request, string value)
        {
            request.AcceptTypes.Clear();
            var accepts = value.Split(',');
            foreach (var accept in accepts)
                request.AcceptTypes.Add(accept.TrimEnd());
            var newAccepts = string.Join(", ", request.AcceptTypes.ToArray());
            return request.AddOrUpdateParameter("Accept", accepts, ParameterType.HttpHeader);
        }

        /// <summary>设置KeepAlive
        /// </summary>
        public static IHttpRequest SetKeepAlive(this IHttpRequest request, bool keepAlive)
        {
            request.KeepAlive = keepAlive;
            return request;
        }

        /// <summary>使用证书
        /// </summary>
        public static IHttpRequest SetClientCertificates(this IHttpRequest request, X509CertificateCollection certificates)
        {
            request.ClientCertificates = certificates;
            return request;
        }

        /// <summary>使用缓存
        /// </summary>
        public static IHttpRequest SetCachePolicy(this IHttpRequest request, RequestCachePolicy cachePolicy)
        {
            request.CachePolicy = cachePolicy;
            return request;
        }

        /// <summary>使用是否重定向与最大重定向数量
        /// </summary>
        public static IHttpRequest SetRedirects(this IHttpRequest request, bool followRedirects, int? maxRedirects)
        {
            request.FollowRedirects = followRedirects;
            request.MaxRedirects = maxRedirects;
            return request;
        }

        /// <summary>设置Pipelined
        /// </summary>
        public static IHttpRequest SetPipelined(this IHttpRequest request, bool pipelined)
        {
            request.Pipelined = pipelined;
            return request;
        }

        /// <summary>设置ConnectionGroupName
        /// </summary>
        public static IHttpRequest SetConnectionGroupName(this IHttpRequest request, string connectionGroupName)
        {
            request.ConnectionGroupName = connectionGroupName;
            return request;
        }

        /// <summary>设置CookieContainer
        /// </summary>
        public static IHttpRequest SetCookieContainer(this IHttpRequest request, CookieContainer cookieContainer)
        {
            request.CookieContainer = cookieContainer;
            return request;
        }

        /// <summary>设置自动压缩
        /// </summary>
        public static IHttpRequest SetAutomaticDecompression(this IHttpRequest request, bool automaticDecompression)
        {
            request.AutomaticDecompression = automaticDecompression;
            return request;
        }

        /// <summary>设置请求配置
        /// </summary>
        public static IHttpRequest SetWebRequestConfigurator(this IHttpRequest request, Action<HttpWebRequest> webRequestConfigurator)
        {
            request.WebRequestConfigurator = webRequestConfigurator;
            return request;
        }

        /// <summary>设置是否发送头部认证
        /// </summary>
        public static IHttpRequest SetPreAuthenticate(this IHttpRequest request, bool preAuthenticate)
        {
            request.PreAuthenticate = preAuthenticate;
            return request;
        }


    }
}
