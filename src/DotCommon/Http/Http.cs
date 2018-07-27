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
    /// <summary>
    ///     HttpWebRequest wrapper
    /// </summary>
    public partial class Http : IHttp
    {
        private const string LINE_BREAK = "\r\n";

        private const string FORM_BOUNDARY = "-----------------------------28947758029299";

        private readonly IDictionary<string, Action<HttpWebRequest, string>> restrictedHeaderActions;

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

        /// <summary>请求是否拥有参数
        /// </summary>
        protected bool HasParameters => Parameters.Any();

        /// <summary>请求是否拥有Cookie
        /// </summary>
        protected bool HasCookies => Cookies.Any();

        /// <summary>请求是否拥有Body
        /// </summary>
        protected bool HasBody => RequestBodyBytes != null || !string.IsNullOrEmpty(RequestBody);

        /// <summary>请求是否上传文件
        /// </summary>
        protected bool HasFiles => Files.Any();

        /// <summary>是否启用自动gzip/deflate  压缩
        /// </summary>
        public bool AutomaticDecompression { get; set; }

        /// <summary>总是用 multipart/form-data发送请求
        /// </summary>
        public bool AlwaysMultipartFormData { get; set; }

        /// <summary>UserAgent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>超时时间
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>读写超时时间
        /// </summary>
        public int ReadWriteTimeout { get; set; }

        /// <summary>默认凭据
        /// </summary>
        public ICredentials Credentials { get; set; }

        /// <summary>CookieContainer
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>写响应的方法
        /// </summary>
        public Action<Stream> ResponseWriter { get; set; }

        /// <summary>是否自动重定向
        /// </summary>
        public bool FollowRedirects { get; set; }

        /// <summary>管道
        /// </summary>
        public bool Pipelined { get; set; }

        /// <summary>X509证书
        /// </summary>
        public X509CertificateCollection ClientCertificates { get; set; }

        /// <summary>最大的重定向数量
        /// </summary>
        public int? MaxRedirects { get; set; }

        /// <summary>是否使用默认凭证
        /// </summary>
        public bool UseDefaultCredentials { get; set; }

        /// <summary>连接分组
        /// </summary>
        public string ConnectionGroupName { get; set; }

        /// <summary>编码
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>KeepAlive
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>POST请求Body
        /// </summary>
        public string RequestBody { get; set; }

        /// <summary>ContentType
        /// </summary>
        public string RequestContentType { get; set; }

        /// <summary>请求Body数据
        /// </summary>
        public byte[] RequestBodyBytes { get; set; }

        /// <summary>请求Url
        /// </summary>
        public Uri Url { get; set; }

        /// <summary>请求Host
        /// </summary>
        public string Host { get; set; }

        /// <summary>是否发送认证头部
        /// </summary>
        public bool PreAuthenticate { get; set; }

        /// <summary>是否使用安全连接
        /// </summary>
        public bool UnsafeAuthenticatedConnectionSharing { get; set; }

        /// <summary>代理
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>缓存
        /// </summary>
        public RequestCachePolicy CachePolicy { get; set; }

        /// <summary>Https验证
        /// </summary>
        public RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

        /// <summary>孕育压缩方法
        /// </summary>
        public IList<DecompressionMethods> AllowedDecompressionMethods { get; set; }

        /// <summary>上传文件
        /// </summary>
        public IList<HttpFile> Files { get; }

        /// <summary>请求头部
        /// </summary>
        public IList<HttpHeader> Headers { get; }

        /// <summary>请求参数
        /// </summary>
        public IList<HttpParameter> Parameters { get; }

        /// <summary>Cookie
        /// </summary>
        public IList<HttpCookie> Cookies { get; }

        /// <summary>
        /// </summary>
        public static IHttp Create() => new Http();

        protected virtual HttpWebRequest CreateWebRequest(Uri url) => (HttpWebRequest)WebRequest.Create(url);

        public Action<HttpWebRequest> WebRequestConfigurator { get; set; }

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

        private static string GetMultipartFormContentType()
        {
            return string.Format("multipart/form-data; boundary={0}", FORM_BOUNDARY);
        }

        private static string GetMultipartFileHeader(HttpFile file)
        {
            return string.Format(
                "--{0}{4}Content-Disposition: form-data; name=\"{1}\"; filename=\"{2}\"{4}Content-Type: {3}{4}{4}",
                FORM_BOUNDARY, file.Name, file.FileName, file.ContentType ?? "application/octet-stream", LINE_BREAK);
        }

        private string GetMultipartFormData(HttpParameter param)
        {
            var format = param.Name == RequestContentType
                ? "--{0}{3}Content-Type: {4}{3}Content-Disposition: form-data; name=\"{1}\"{3}{3}{2}{3}"
                : "--{0}{3}Content-Disposition: form-data; name=\"{1}\"{3}{3}{2}{3}";

            return string.Format(format, FORM_BOUNDARY, param.Name, param.Value, LINE_BREAK, param.ContentType);
        }

        private static string GetMultipartFooter()
        {
            return string.Format("--{0}--{1}", FORM_BOUNDARY, LINE_BREAK);
        }

        // handle restricted headers the .NET way - thanks @dimebrain!
        // http://msdn.microsoft.com/en-us/library/system.net.httpwebrequest.headers.aspx
        private void AppendHeaders(HttpWebRequest webRequest)
        {
            foreach (var header in Headers)
                if (restrictedHeaderActions.ContainsKey(header.Name))
                    restrictedHeaderActions[header.Name].Invoke(webRequest, header.Value);
                else
                    webRequest.Headers.Add(header.Name, header.Value);
        }

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

        private string EncodeParameters()
        {
            var querystring = new StringBuilder();

            foreach (var p in Parameters)
            {
                if (querystring.Length > 1)
                    querystring.Append("&");

                querystring.AppendFormat("{0}={1}", p.Name.UrlEncode(), p.Value.UrlEncode());
            }

            return querystring.ToString();
        }

        private void PreparePostBody(HttpWebRequest webRequest)
        {

            bool needsContentType = String.IsNullOrEmpty(webRequest.ContentType);

            if (HasFiles || AlwaysMultipartFormData)
            {
                if (needsContentType)
                    webRequest.ContentType = GetMultipartFormContentType();
            }
            else if (HasParameters)
            {
                if (needsContentType)
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                RequestBody = EncodeParameters();
            }
            else if (HasBody)
            {
                if (needsContentType)
                    webRequest.ContentType = RequestContentType;
            }
        }

        private void WriteStringTo(Stream stream, string toWrite)
        {
            var bytes = Encoding.GetBytes(toWrite);

            stream.Write(bytes, 0, bytes.Length);
        }

        private void WriteMultipartFormData(Stream requestStream)
        {
            foreach (var param in Parameters)
                WriteStringTo(requestStream, GetMultipartFormData(param));

            foreach (var file in Files)
            {
                // Add just the first part of this param, since we will write the file data directly to the Stream
                WriteStringTo(requestStream, GetMultipartFileHeader(file));

                // Write the file data directly to the Stream, rather than serializing it to a string.
                file.Writer(requestStream);
                WriteStringTo(requestStream, LINE_BREAK);
            }

            WriteStringTo(requestStream, GetMultipartFooter());
        }

        private void ExtractResponseData(Response response, HttpWebResponse webResponse)
        {
            using (webResponse)
            {
                response.ContentEncoding = webResponse.ContentEncoding;
                response.Server = webResponse.Server;
                response.ProtocolVersion = webResponse.ProtocolVersion;
                response.ContentType = webResponse.ContentType;
                response.ContentLength = webResponse.ContentLength;

                var webResponseStream = webResponse.GetResponseStream();

                ProcessResponseStream(webResponseStream, response);

                response.StatusCode = webResponse.StatusCode;
                response.StatusDescription = webResponse.StatusDescription;
                response.ResponseUri = webResponse.ResponseUri;
                response.ResponseStatus = ResponseStatus.Completed;

                if (webResponse.Cookies != null)
                    foreach (Cookie cookie in webResponse.Cookies)
                        response.Cookies.Add(new HttpCookie
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

                foreach (var headerName in webResponse.Headers.AllKeys)
                {
                    var headerValue = webResponse.Headers[headerName];

                    response.Headers.Add(new HttpHeader
                    {
                        Name = headerName,
                        Value = headerValue
                    });
                }

                webResponse.Close();
            }
        }

        private void ProcessResponseStream(Stream webResponseStream, Response response)
        {
            if (ResponseWriter == null)
                response.RawBytes = webResponseStream.ReadAsBytes();
            else
                ResponseWriter(webResponseStream);
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
    }
}
