using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DotCommon.Http
{
    public static class HttpRequestExtensions
    {
        /// <summary>添加UserAgent
        /// </summary>
        public static IHttpRequest AddUserAgent(this IHttpRequest request, string userAgent)
        {
            return request.AddParameter("User-Agent", userAgent, ParameterType.HttpHeader);
        }

        /// <summary>添加Referer
        /// </summary>
        public static IHttpRequest AddReferer(this IHttpRequest request, string referer)
        {
            return request.AddParameter("Referer", referer, ParameterType.HttpHeader);
        }

        /// <summary>添加Accept
        /// </summary>
        public static IHttpRequest AddAccept(this IHttpRequest request, string accept)
        {
            if (request.Parameters.Any(p => p.Name.ToLowerInvariant() == "accept"))
            {
                request.RemoveParameter("Accept");
            }
            return request.AddParameter("Accept", accept, ParameterType.HttpHeader);
        }

        /// <summary>添加AcceptType
        /// </summary>
        public static IHttpRequest AddAcceptType(this IHttpRequest request, string acceptType)
        {
            if (!request.AcceptTypes.Any(x => x.Equals(acceptType)))
            {
                request.AcceptTypes.Add(acceptType);
            }
            var accepts = string.Join(", ", request.AcceptTypes.ToArray());
            return AddAccept(request, accepts);
        }


        /// <summary>设置KeepAlive
        /// </summary>
        public static IHttpRequest AddKeepAlive(this IHttpRequest request, bool keepAlive)
        {
            return request.AddParameter("Keep-Alive", keepAlive, ParameterType.HttpHeader);
        }

        /// <summary>设置是否自动压缩
        /// </summary>
        public static IHttpRequest SetAutomaticDecompression(this IHttpRequest request, bool automaticDecompression)
        {
            request.AutomaticDecompression = automaticDecompression;
            return request;
        }

        /// <summary>设置ConnectionGroupName
        /// </summary>
        public static IHttpRequest SetConnectionGroupName(this IHttpRequest request, string connectionGroupName)
        {
            request.ConnectionGroupName = connectionGroupName;
            return request;
        }

        /// <summary>设置是否允许重定向
        /// </summary>
        public static IHttpRequest SetFollowRedirects(this IHttpRequest request, bool followRedirects)
        {
            request.FollowRedirects = followRedirects;
            return request;
        }

        /// <summary>设置最大重定向数量
        /// </summary>
        public static IHttpRequest SetMaxRedirects(this IHttpRequest request, int maxRedirects)
        {
            request.MaxRedirects = maxRedirects;
            return request;
        }

        /// <summary>设置最大重定向数量
        /// </summary>
        public static IHttpRequest SetPipelined(this IHttpRequest request, bool pipelined)
        {
            request.Pipelined = pipelined;
            return request;
        }

        /// <summary>设置缓存策略
        /// </summary>
        public static IHttpRequest SetCachePolicy(this IHttpRequest request, RequestCachePolicy cachePolicy)
        {
            request.CachePolicy = cachePolicy;
            return request;
        }

        /// <summary>设置CookieContainer
        /// </summary>
        public static IHttpRequest SetCookieContainer(this IHttpRequest request, CookieContainer cookieContainer)
        {
            request.CookieContainer = cookieContainer;
            return request;
        }

        /// <summary>设置编码
        /// </summary>
        public static IHttpRequest SetEncoding(this IHttpRequest request, Encoding encoding)
        {
            request.Encoding = encoding;
            return request;
        }

        /// <summary>添加凭证
        /// </summary>
        public static IHttpRequest AddCredentials(this IHttpRequest request, ICredentials credentials)
        {
            request.Credentials = credentials;
            return request;
        }

        /// <summary>添加x.509证书
        /// </summary>
        public static IHttpRequest AddClientCredentials(this IHttpRequest request, X509Certificate x509Certificate)
        {
            request.ClientCertificates.Add(x509Certificate);
            return request;
        }

        /// <summary>清除证书
        /// </summary>
        public static IHttpRequest ClearClientCredentials(this IHttpRequest request)
        {
            request.ClientCertificates.Clear();
            return request;
        }

        /// <summary>缓存策略
        /// </summary>
        public static IHttpRequest AddCachePolicy(this IHttpRequest request, RequestCachePolicy cachePolicy)
        {
            request.CachePolicy = cachePolicy;
            return request;
        }
    }

}
