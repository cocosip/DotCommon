using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Linq;

namespace DotCommon.Http
{
    public class HttpClient : IHttpClient
    {
        private const string DefaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/67.0.339";
        private IList<string> AcceptTypes { get; }
        public HttpClient()
        {
            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            AcceptTypes = new List<string>
            {
                "application/json",
                "application/xml",
                "text/json",
                "text/x-json",
                "text/javascript",
                "text/xml",
                "*+json",
                "*+xml",
                "*"
            };
        }

        public async Task ExecuteAsync(HttpRequest request)
        {

            //request1.KeepAlive;
            await Task.CompletedTask;
        }

        IHttp ConfigureHttp(IHttpRequest request)
        {
            var http = Http.Create();
            //编码
            http.Encoding = request.Encoding;
            //请求是否都使用MultipartFormData
            http.AlwaysMultipartFormData = request.AlwaysMultipartFormData;
            //是否使用默认的身份认证
            http.UseDefaultCredentials = request.UseDefaultCredentials;
            //http.ResponseWriter = request.ResponseWriter;
            http.CookieContainer = request.CookieContainer;
            http.AutomaticDecompression = request.AutomaticDecompression;
            //http.WebRequestConfigurator = WebRequestConfigurator;

            //添加Accept

            //如果请求中没有Accept
            if (request.Parameters.All(p2 => p2.Name.ToLowerInvariant() != "accept"))
            {
                var accepts = string.Join(", ", AcceptTypes.ToArray());
                request.AddParameter("Accept", accepts, ParameterType.HttpHeader);
            }

            http.Url = BuildUri(request);
            //Host
            http.Host = http.Url.Host;
            http.PreAuthenticate = request.PreAuthenticate;
            http.UnsafeAuthenticatedConnectionSharing = request.UnsafeAuthenticatedConnectionSharing;

            //var userAgent = request.Parameters.FirstOrDefault(p => p.Name == "User-Agent" && p.Type == ParameterType.HttpHeader);
            //if (userAgent != null && userAgent.Value != null)
            var timeout = request.Timeout != 0 ? request.Timeout : 5000;
            if (timeout != 0)
                http.Timeout = timeout;

            var readWriteTimeout = request.ReadWriteTimeout != 0 ? request.ReadWriteTimeout : 5000;
            if (readWriteTimeout != 0)
                http.ReadWriteTimeout = readWriteTimeout;

            //是否允许重定向
            http.FollowRedirects = request.FollowRedirects;
            //最大重定向数量
            http.MaxRedirects = request.MaxRedirects;
            //客户端凭据
            if (request.ClientCertificates != null)
                http.ClientCertificates = request.ClientCertificates;
            //缓存策略
            http.CachePolicy = request.CachePolicy;

            http.Pipelined = request.Pipelined;

            if (request.Credentials != null)
                http.Credentials = request.Credentials;

            if (!string.IsNullOrEmpty(request.ConnectionGroupName))
                http.ConnectionGroupName = request.ConnectionGroupName;

            var headers = from p in request.Parameters
                          where p.Type == ParameterType.HttpHeader
                          select new HttpHeader
                          {
                              Name = p.Name,
                              Value = Convert.ToString(p.Value)
                          };

            foreach (var header in headers)
                http.Headers.Add(header);

            var cookies = from p in request.Parameters
                          where p.Type == ParameterType.Cookie
                          select new HttpCookie
                          {
                              Name = p.Name,
                              Value = Convert.ToString(p.Value)
                          };

            foreach (var cookie in cookies)
                http.Cookies.Add(cookie);

            var @params = from p in request.Parameters
                          where p.Type == ParameterType.GetOrPost && p.Value != null
                          select new HttpParameter
                          {
                              Name = p.Name,
                              Value = Convert.ToString(p.Value)
                          };

            foreach (var parameter in @params)
                http.Parameters.Add(parameter);

            foreach (var file in request.Files)
                http.Files.Add(new HttpFile
                {
                    Name = file.Name,
                    ContentType = file.ContentType,
                    Writer = file.Writer,
                    FileName = file.FileName,
                    ContentLength = file.ContentLength
                });

            var body = request.Parameters.FirstOrDefault(p => p.Type == ParameterType.RequestBody);

            if (body != null)
            {
                http.RequestContentType = body.Name;

                if (!http.AlwaysMultipartFormData && !http.Files.Any())
                {
                    var val = body.Value;

                    if (val is byte[] bytes)
                        http.RequestBodyBytes = bytes;
                    else
                        http.RequestBody = Convert.ToString(body.Value);
                }
                else
                {
                    http.Parameters.Add(new HttpParameter
                    {
                        Name = body.Name,
                        Value = Convert.ToString(body.Value),
                        ContentType = body.ContentType
                    });
                }
            }

            http.AllowedDecompressionMethods = request.AllowedDecompressionMethods;
            http.Proxy = request.Proxy ?? (WebRequest.DefaultWebProxy ?? HttpWebRequest.GetSystemWebProxy());
            http.RemoteCertificateValidationCallback = request.RemoteCertificateValidationCallback;

            return http;
        }

        public Uri BuildUri(IHttpRequest request)
        {
            //DoBuildUriValidations(request);

            //var applied = GetUrlSegmentParamsValues(request);

            //BaseUrl = applied.Uri;
            //string resource = applied.Resource;

            //string mergedUri = MergeBaseUrlAndResource(resource);

            //string finalUri = ApplyQueryStringParamsValuesToUri(mergedUri, request);

            return new Uri(request.Url);
        }

    }
}
