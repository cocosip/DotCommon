using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DotCommon.Http
{
    public partial class HttpClient : IHttpClient
    {
        private static readonly Version version = new AssemblyName(Assembly.GetExecutingAssembly().FullName).Version;

        private IList<string> AcceptTypes { get; }
        private Action<HttpWebRequest> WebRequestConfigurator { get; set; }

        /// <summary>启用或者禁用自动压缩
        /// </summary>
        public bool AutomaticDecompression { get; set; }

        /// <summary>最大允许跳转数量
        /// </summary>
        public int? MaxRedirects { get; set; }

        /// <summary>X509CertificateCollection 客户端证书
        /// </summary>
        public X509CertificateCollection ClientCertificates { get; set; }

        /// <summary>代理
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>客户端请求缓存
        /// </summary>
        public RequestCachePolicy CachePolicy { get; set; }

        /// <summary>
        /// </summary>
        public bool Pipelined { get; set; }

        /// <summary>是否自动重定向
        /// </summary>
        public bool FollowRedirects { get; set; }

        /// <summary>CookieContainer
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>UserAgent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>客户端请求超时时间,以毫秒为单位
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>读写的超时时间,以毫秒为单位
        /// </summary>
        public int ReadWriteTimeout { get; set; }

        /// <summary>编码
        /// </summary>
        public Encoding Encoding { get; set; }

        public bool PreAuthenticate { get; set; }

        /// <summary>允许高速NTLM身份验证的连接共享
        /// </summary>
        public bool UnsafeAuthenticatedConnectionSharing { get; set; }

        /// <summary>RemoteCertificateValidationCallback
        /// </summary>
        public RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

        /// <summary>默认参数
        /// </summary>
        public IList<Parameter> DefaultParameters { get; }

        /// <summary>BaseHost
        /// </summary>
        public string BaseHost { get; set; }

        /// <summary>如果您需要添加具有相同名称的多个默认参数，设置为true。只支持 query 和 form parameters
        /// </summary>
        public bool AllowMultipleDefaultParametersWithSameName { get; set; } = false;

        /// <summary>HttpWebRequest操作
        /// </summary>
        public void ConfigureWebRequest(Action<HttpWebRequest> configurator) =>
            WebRequestConfigurator = configurator;

        /// <summary>初始化
        /// </summary>
        public HttpClient()
        {
            Encoding = Encoding.UTF8;
            AcceptTypes = new List<string>();
            DefaultParameters = new List<Parameter>();
            AutomaticDecompression = true;

            AcceptTypes.Add("application/json");
            AcceptTypes.Add("application/xml");
            AcceptTypes.Add("text/json");
            AcceptTypes.Add("text/x-json");
            AcceptTypes.Add("text/javascript");
            AcceptTypes.Add("text/xml");
            AcceptTypes.Add("*+json");
            AcceptTypes.Add("*+xml");
            AcceptTypes.Add("*");
            FollowRedirects = true;
            MaxRedirects = 1;
        }

        /// <summary>执行的方法
        /// </summary>
        private async Task<IHttpResponse> ExecuteAsync(IHttpRequest request, string httpMethod,
        Func<IHttp, string, Task<Response>> getResponse)
        {

            IHttpResponse response = new HttpResponse();
            try
            {
                var http = ConfigureHttp(request);
                var resp = await getResponse(http, httpMethod);
                response = ConvertToHttpResponse(request, resp);
                response.Request = request;
            }
            catch (Exception ex)
            {
                response.ResponseStatus = ResponseStatus.Error;
                response.ErrorMessage = ex.Message;
                response.ErrorException = ex;
            }

            return response;
        }

        /// <summary>组装URL参数
        /// </summary>
        public Uri BuildUri(IHttpRequest request)
        {
            DoBuildUriValidations(request);

            var applied = GetUrlSegmentParamsValues(request);
            //
            request.BaseUrl = applied.Uri;
            string resource = applied.Resource;

            string mergedUri = MergeBaseUrlAndResource(request, resource);

            string finalUri = ApplyQueryStringParamsValuesToUri(mergedUri, request);

            return new Uri(finalUri);
        }

        private void DoBuildUriValidations(IHttpRequest request)
        {
            if (request.BaseUrl == null)
            {
                throw new NullReferenceException("RestClient must contain a value for BaseUrl");
            }

            var nullValuedParams = request.Parameters
                .Where(p => p.Type == ParameterType.UrlSegment && p.Value == null)
                .Select(p => p.Name);

            if (nullValuedParams.Any())
            {
                var names = string.Join(", ", nullValuedParams.Select(name => $"'{name}'").ToArray());
                throw new ArgumentException($"Url 片段参数中 {names} 的值为null",
                    nameof(request));
            }
        }

        /// <summary>获取Url中Segment片段值
        /// </summary>
        private UrlSegmentParamsValues GetUrlSegmentParamsValues(IHttpRequest request)
        {
            var assembled = request.Resource;
            var hasResource = !string.IsNullOrEmpty(assembled);
            var urlParms = request.Parameters.Where(p => p.Type == ParameterType.UrlSegment);
            var builder = new UriBuilder(request.BaseUrl);

            foreach (var parameter in urlParms)
            {
                var paramPlaceHolder = $"{{{parameter.Name}}}";
                var paramValue = parameter.Value.ToString().UrlEncode();

                if (hasResource)
                {
                    assembled = assembled.Replace(paramPlaceHolder, paramValue);
                }

                builder.Path = builder.Path.UrlDecode().Replace(paramPlaceHolder, paramValue);
            }

            return new UrlSegmentParamsValues(builder.Uri, assembled);
        }

        private string MergeBaseUrlAndResource(IHttpRequest request, string resource)
        {
            var assembled = resource;

            if (!string.IsNullOrEmpty(assembled) && assembled.StartsWith("/"))
            {
                assembled = assembled.Substring(1);
            }

            if (request.BaseUrl == null || string.IsNullOrEmpty(request.BaseUrl.AbsoluteUri))
            {
                return assembled;
            }

            Uri usingBaseUri = request.BaseUrl;
            if (!request.BaseUrl.AbsoluteUri.EndsWith("/") && !string.IsNullOrEmpty(assembled))
            {
                usingBaseUri = new Uri(request.BaseUrl.AbsoluteUri + "/");
            }
            assembled = new Uri(usingBaseUri, assembled).AbsoluteUri;
            return assembled;
        }

        /// <summary>添加QueryString参数到请求
        /// </summary>
        private string ApplyQueryStringParamsValuesToUri(string mergedUri, IHttpRequest request)
        {
            var parameters = GetQueryStringParameters(request);

            if (!parameters.Any())
            {
                return mergedUri;
            }

            var separator = mergedUri != null && mergedUri.Contains("?") ? "&" : "?";
            //Url上的参数,是需要进行编码转换的
            return string.Concat(mergedUri, separator, EncodeParameters(parameters, Encoding));
        }

        /// <summary>获取QueryString参数
        /// </summary>
        private static IEnumerable<Parameter> GetQueryStringParameters(IHttpRequest request)
        {
            //如果是GET请求,那么就是GetOrPost参数与QueryString参数,这两个类型的参数是一样的,都是添加到请求Url上,
            //如果是其他请求,那么都是在QueryString参数上
            return request.Method != Method.POST && request.Method != Method.PUT && request.Method != Method.PATCH
                ? request.Parameters
                    .Where(p => p.Type == ParameterType.GetOrPost ||
                                p.Type == ParameterType.QueryString)
                : request.Parameters
                    .Where(p => p.Type == ParameterType.QueryString);
        }

        private static string EncodeParameters(IEnumerable<Parameter> parameters, Encoding encoding) =>
            string.Join("&", parameters.Select(parameter => EncodeParameter(parameter, encoding)).ToArray());

        private static string EncodeParameter(Parameter parameter, Encoding encoding) =>
            parameter.Value == null
                ? string.Concat(parameter.Name.UrlEncode(encoding), "=")
                : string.Concat(parameter.Name.UrlEncode(encoding), "=",
                    parameter.Value.ToString().UrlEncode(encoding));

        private static readonly ParameterType[] MultiParameterTypes =
            {ParameterType.QueryString, ParameterType.GetOrPost};

        internal IHttp ConfigureHttp(IHttpRequest request)
        {
            var http = Http.Create();
            //设置编码
            http.Encoding = request.Encoding ?? Encoding;
            //KeepAlive
            http.KeepAlive = request.KeepAlive;

            //是否总是使用Mulit FormData
            http.AlwaysMultipartFormData = request.AlwaysMultipartFormData;
            //是否使用默认凭据
            http.UseDefaultCredentials = request.UseDefaultCredentials;
            http.ResponseWriter = request.ResponseWriter;
            //CookieContainer
            http.CookieContainer = request.CookieContainer ?? CookieContainer;
            //自动压缩
            http.AutomaticDecompression = request.AutomaticDecompression ?? AutomaticDecompression;
            //WebRequest操作
            http.WebRequestConfigurator = request.WebRequestConfigurator ?? WebRequestConfigurator;

            // 将Client的默认参数添加到Request中
            foreach (var p in DefaultParameters)
            {
                var parameterExists = request.Parameters.Any(p2 => p2.Name == p.Name && p2.Type == p.Type);

                if (AllowMultipleDefaultParametersWithSameName)
                {
                    var isMultiParameter = MultiParameterTypes.Any(pt => pt == p.Type);
                    parameterExists = !isMultiParameter && parameterExists;
                }

                if (parameterExists)
                    continue;

                request.AddParameter(p);
            }

            //如果请求头部中没有添加Accept,那么就使用Client中的默认的
            if (request.Parameters.All(p2 => p2.Name.ToLowerInvariant() != "accept"))
            {
                var accepts = string.Join(", ", AcceptTypes.ToArray());

                request.AddParameter("Accept", accepts, ParameterType.HttpHeader);
            }

            http.Url = BuildUri(request);
            http.Host = BaseHost;
            http.PreAuthenticate = request.PreAuthenticate ?? PreAuthenticate;
            http.UnsafeAuthenticatedConnectionSharing = UnsafeAuthenticatedConnectionSharing;

            var userAgent = UserAgent ?? http.UserAgent;

            http.UserAgent = userAgent.HasValue()
                ? userAgent
                : "CSharp/" + version;

            var timeout = request.Timeout != 0
                ? request.Timeout
                : Timeout;

            if (timeout != 0)
                http.Timeout = timeout;

            var readWriteTimeout = request.ReadWriteTimeout != 0
                ? request.ReadWriteTimeout
                : ReadWriteTimeout;

            if (readWriteTimeout != 0)
                http.ReadWriteTimeout = readWriteTimeout;

            //x509证书
            http.ClientCertificates = request.ClientCertificates ?? ClientCertificates ?? null;
            //是否允许重定向
            http.FollowRedirects = FollowRedirects;
            //最大重定向数量
            http.MaxRedirects = request.MaxRedirects ?? MaxRedirects;
            //缓存
            http.CachePolicy = request.CachePolicy ?? CachePolicy;
            //Pipelined
            http.Pipelined = request.Pipelined ?? Pipelined;

            if (request.Credentials != null)
                http.Credentials = request.Credentials;
            //连接分组
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

            //如果没有任何文件，使它成为一个multi-formdata单请求，只会增加Body
            // 如果有文件或AlwaysMultipartFormData=true，然后将body运到HTTP添加参数
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
            http.Proxy = Proxy ?? (WebRequest.DefaultWebProxy ?? HttpWebRequest.GetSystemWebProxy());
            http.RemoteCertificateValidationCallback = RemoteCertificateValidationCallback;

            return http;
        }

        /// <summary>将Response转换成HttpResponse
        /// </summary>
        private static HttpResponse ConvertToHttpResponse(IHttpRequest request, Response response)
        {
            var httpResponse = new HttpResponse
            {
                Content = response.Content,
                ContentEncoding = response.ContentEncoding,
                ContentLength = response.ContentLength,
                ContentType = response.ContentType,
                ErrorException = response.ErrorException,
                ErrorMessage = response.ErrorMessage,
                RawBytes = response.RawBytes,
                ResponseStatus = response.ResponseStatus,
                ResponseUri = response.ResponseUri,
                ProtocolVersion = response.ProtocolVersion,
                Server = response.Server,
                StatusCode = response.StatusCode,
                StatusDescription = response.StatusDescription,
                Request = request
            };

            foreach (var header in response.Headers)
                httpResponse.Headers.Add(new Parameter
                {
                    Name = header.Name,
                    Value = header.Value,
                    Type = ParameterType.HttpHeader
                });

            foreach (var cookie in response.Cookies)
                httpResponse.Cookies.Add(new HttpResponseCookie
                {
                    Comment = cookie.Comment,
                    CommentUri = cookie.CommentUri,
                    Discard = cookie.Discard,
                    Domain = cookie.Domain,
                    Expired = cookie.Expired,
                    Expires = cookie.Expires,
                    HttpOnly = cookie.HttpOnly,
                    Name = cookie.Name,
                    Path = cookie.Path,
                    Port = cookie.Port,
                    Secure = cookie.Secure,
                    TimeStamp = cookie.TimeStamp,
                    Value = cookie.Value,
                    Version = cookie.Version
                });

            return httpResponse;
        }

        private class UrlSegmentParamsValues
        {
            public UrlSegmentParamsValues(Uri builderUri, string assembled)
            {
                Uri = builderUri;
                Resource = assembled;
            }

            public Uri Uri { get; }
            public string Resource { get; }
        }
    }
}
