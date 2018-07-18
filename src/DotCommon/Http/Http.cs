using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace DotCommon.Http
{
    /// <summary>请求的Http对象,用来构建HttpWebRequest
    /// </summary>
    public partial class Http : IHttp
    {
        private const string LINE_BREAK = "\r\n";
        private string FormBoundary = $"-----------------------------{DateTime.Now.Ticks}";

        /// <summary>自动压缩
        /// </summary>
        public bool AutomaticDecompression { get; set; }

        /// <summary>总使用
        /// </summary>
        public bool AlwaysMultipartFormData { get; set; }

        /// <summary>超时时间
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>读写超时时间
        /// </summary>
        public int ReadWriteTimeout { get; set; }

        /// <summary>指示管道连接的首选项, 当 Pipelined 是true应用程序将通过管道传递的连接到支持它们的服务器
        /// </summary>
        public bool Pipelined { get; set; }

        /// <summary>是否允许重定向
        /// </summary>
        public bool FollowRedirects { get; set; }

        /// <summary>允许的最大的重定向的数量
        /// </summary>
        public int? MaxRedirects { get; set; }

        /// <summary>是否使用默认的身份认证
        /// </summary>
        public bool UseDefaultCredentials { get; set; }

        /// <summary>PreAuthenticate
        /// </summary>
        public bool PreAuthenticate { get; set; }

        /// <summary>UnsafeAuthenticatedConnectionSharing
        /// </summary>
        public bool UnsafeAuthenticatedConnectionSharing { get; set; }

        /// <summary>连接分组
        /// </summary>
        public string ConnectionGroupName { get; set; }

        /// <summary>编码
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>RequestBody
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>请求的ContentType
        /// </summary>
        public string RequestContentType { get; set; }

        /// <summary>请求Body的二进制数据
        /// </summary>
        public byte[] RequestBodyBytes { get; set; }

        /// <summary>Cookie
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>
        /// </summary>
        public Action<Stream> ResponseWriter { get; set; }

        /// <summary>请求URL
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>HOST
        /// </summary>
        public string Host { get; set; }

        /// <summary>凭证
        /// </summary>
        public ICredentials Credentials { get; set; }

        /// <summary>代理
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>请求的x.509证书
        /// </summary>
        public X509CertificateCollection ClientCertificates { get; set; }

        /// <summary>验证方法
        /// </summary>
        public RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

        /// <summary>请求缓存
        /// </summary>
        public RequestCachePolicy CachePolicy { get; set; }

        /// <summary>协议版本
        /// </summary>
        public Version ProtocolVersion { get; set; }

        /// <summary>允许的压缩
        /// </summary>
        public IList<DecompressionMethods> AllowedDecompressionMethods { get; set; }

        /// <summary>上传的文件
        /// </summary>
        public IList<HttpFile> Files { get; }

        /// <summary>请求Header
        /// </summary>
        public IList<HttpHeader> Headers { get; }

        /// <summary>请求参数
        /// </summary>
        public IList<HttpParameter> Parameters { get; }

        /// <summary>Cookie
        /// </summary>
        public IList<HttpCookie> Cookies { get; }

        /// <summary>请求配置
        /// </summary>
        public Action<HttpWebRequest> WebRequestConfigurator { get; set; }

        /// <summary>是否有上传文件
        /// </summary>
        protected bool HasFiles => Files.Any();

        private readonly IDictionary<string, Action<HttpWebRequest, string>> restrictedHeaderActions;


        protected virtual HttpWebRequest CreateWebRequest(Uri url) => (HttpWebRequest)WebRequest.Create(url);

        public Http()
        {
            Headers = new List<HttpHeader>();
            Files = new List<HttpFile>();
            Parameters = new List<HttpParameter>();
            Cookies = new List<HttpCookie>();
            restrictedHeaderActions = new Dictionary<string, Action<HttpWebRequest, string>>(
                StringComparer.OrdinalIgnoreCase);

            AddSharedHeaderActions();
            AddSyncHeaderActions();
        }

        public static IHttp Create() => new Http();

        private void AddSharedHeaderActions()
        {
            restrictedHeaderActions.Add("Accept", (r, v) => r.Accept = v);
            restrictedHeaderActions.Add("Content-Type", (r, v) => r.ContentType = v);
            restrictedHeaderActions.Add("Date", (r, v) =>
            {
                if (DateTime.TryParse(v, out var parsed))
                    r.Date = parsed;
            });
            restrictedHeaderActions.Add("Host", (r, v) => r.Host = v);
            restrictedHeaderActions.Add("Range", AddRange);
        }

        private void AddSyncHeaderActions()
        {
            restrictedHeaderActions.Add("Connection", (r, v) => { r.KeepAlive = v.ToLower().Contains("keep-alive"); });
            restrictedHeaderActions.Add("Content-Length", (r, v) => r.ContentLength = Convert.ToInt64(v));
            restrictedHeaderActions.Add("Expect", (r, v) => r.Expect = v);
            restrictedHeaderActions.Add("If-Modified-Since", (r, v) => r.IfModifiedSince = Convert.ToDateTime(v, CultureInfo.InvariantCulture));
            restrictedHeaderActions.Add("Referer", (r, v) => r.Referer = v);
            restrictedHeaderActions.Add("Transfer-Encoding", (r, v) =>
            {
                r.TransferEncoding = v;
                r.SendChunked = true;
            });
            restrictedHeaderActions.Add("User-Agent", (r, v) => r.UserAgent = v);
        }

        /// <summary>添加头部
        /// </summary>
        private void AppendHeaders(HttpWebRequest webRequest)
        {
            foreach (var header in Headers)
            {
                if (restrictedHeaderActions.ContainsKey(header.Name))
                {
                    restrictedHeaderActions[header.Name].Invoke(webRequest, header.Value);
                }
                else
                {
                    webRequest.Headers.Add(header.Name, header.Value);
                }
            }
        }

        /// <summary>添加Cookies
        /// </summary>
        private void AppendCookies(HttpWebRequest webRequest)
        {
            webRequest.CookieContainer = CookieContainer ?? new CookieContainer();

            foreach (var httpCookie in Cookies)
            {
                var cookie = new Cookie
                {
                    Name = httpCookie.Name,
                    Value = httpCookie.Value,
                    Domain = webRequest.RequestUri.Host
                };

                webRequest.CookieContainer.Add(cookie);
            }
        }


        private static readonly Regex AddRangeRegex = new Regex("(\\w+)=(\\d+)-(\\d+)$");
        private static void AddRange(HttpWebRequest r, string range)
        {
            var m = AddRangeRegex.Match(range);

            if (!m.Success)
                return;

            string rangeSpecifier = m.Groups[1].Value;
            long from = Convert.ToInt64(m.Groups[2].Value);
            long to = Convert.ToInt64(m.Groups[3].Value);

            r.AddRange(rangeSpecifier, from, to);
        }


        protected virtual HttpWebRequest ConfigureWebRequest(string method, Uri url)
        {
            var webRequest = CreateWebRequest(url);
            //使用默认的凭据
            webRequest.UseDefaultCredentials = UseDefaultCredentials;
            //使用预认证
            webRequest.PreAuthenticate = PreAuthenticate;
            //指示管道连接的首选项,当Pipelined 是true应用程序将通过管道传递的连接到支持它们的服务器
            webRequest.Pipelined = Pipelined;
            //经过身份验证的连接是否保持打开状态,true表示打开
            webRequest.UnsafeAuthenticatedConnectionSharing = UnsafeAuthenticatedConnectionSharing;
#if NETSTANDARD2_0
            webRequest.Proxy = null;
#endif
            webRequest.ServicePoint.Expect100Continue = false;
            //添加头部
            AppendHeaders(webRequest);
            //添加Cookie
            AppendCookies(webRequest);
            //设置Host
            if (Host != null)
            {
                webRequest.Host = Host;
            }

            //请求方法
            webRequest.Method = method;
            if (!HasFiles && !AlwaysMultipartFormData)
                webRequest.ContentLength = 0;

            if (Credentials != null)
                webRequest.Credentials = Credentials;

            if (ClientCertificates != null)
                webRequest.ClientCertificates.AddRange(ClientCertificates);

            foreach (var allowedDecompressionMethod in AllowedDecompressionMethods)
            {
                webRequest.AutomaticDecompression |= allowedDecompressionMethod;
            }

            if (AutomaticDecompression)
            {
                webRequest.AutomaticDecompression =
                    DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None;
            }

            if (Timeout != 0)
                webRequest.Timeout = Timeout;

            if (ReadWriteTimeout != 0)
                webRequest.ReadWriteTimeout = ReadWriteTimeout;

            webRequest.Proxy = Proxy;

            if (CachePolicy != null)
                webRequest.CachePolicy = CachePolicy;

            webRequest.AllowAutoRedirect = FollowRedirects;
            if (FollowRedirects && MaxRedirects.HasValue)
                webRequest.MaximumAutomaticRedirections = MaxRedirects.Value;

            webRequest.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;

            webRequest.ConnectionGroupName = ConnectionGroupName;

            WebRequestConfigurator?.Invoke(webRequest);

            return webRequest;
        }

    }
}
