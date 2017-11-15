using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace DotCommon.Http
{
    public class RequestBuilder
    {
        private readonly RequestOptions _options = new RequestOptions();

        public RequestBuilder(string url, string httpMethod)
        {
            _options.Url = url;
            _options.HttpMethod = httpMethod;
        }

        public static RequestBuilder Instance(string url, string httpMethod)
        {
            return new RequestBuilder(url, httpMethod);
        }

        public RequestOptions GetOptions()
        {
            return _options;
        }

        public RequestBuilder SetEncode(string encode = "utf-8")
        {
            _options.Encode = encode;
            return this;
        }

        public RequestBuilder AttachHeader(string key, string value)
        {
            _options.RequestHeaders.Add(key, value);
            return this;
        }

        public RequestBuilder SetHeaders(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (var kv in headers)
                {
                    AttachHeader(kv.Key, kv.Value);
                }
            }
            return this;
        }

        public RequestBuilder AttachParam(string key, string value)
        {
            _options.RequestParameters.Add(key, value);
            return this;
        }

        public RequestBuilder SetParams(Dictionary<string, string> parameters)
        {
            if (parameters != null)
            {
                foreach (var kv in parameters)
                {
                    AttachParam(kv.Key, kv.Value);
                }
            }
            return this;
        }

        public RequestBuilder AttachFile(RequestFile file)
        {
            _options.RequestFiles.Add(file);
            return this;
        }

        public RequestBuilder SetFiles(List<RequestFile> files)
        {
            if (files != null && files.Any())
            {
                foreach (var file in files)
                {
                    AttachFile(file);
                }
            }
            return this;
        }


        public RequestBuilder SetUserAgent(string userAgent)
        {
            _options.UserAgent = userAgent;
            return this;
        }

        public RequestBuilder SetReferer(string referer)
        {
            _options.Referer = referer;
            return this;
        }

        public RequestBuilder SetCookie(CookieContainer cookie)
        {
            if (_options.Cookie == null)
            {
                _options.Cookie = cookie;
            }
            return this;
        }

        public RequestBuilder SetPost(PostType postType, string postString)
        {
            if (_options.HttpMethod.ToUpper() != RequestConsts.Methods.Get)
            {
                _options.PostType = postType;
                _options.PostString = postString;
            }
            return this;
        }

        public RequestBuilder SetPost(PostType postType)
        {
            if (_options.HttpMethod.ToUpper() != RequestConsts.Methods.Get)
            {
                _options.PostType = postType;
            }
            return this;
        }

        /// <summary>设置Url请求参数进行编码
        /// </summary>
        public RequestBuilder SetUrlEncode(string encode = "utf-8")
        {
            _options.IsUrlEncode = true;
            _options.UrlEncode = encode;
            return this;
        }

        /// <summary>设置Url操作
        /// </summary>
        public RequestBuilder SetUrlHandler(Func<KeyValuePair<string, string>, string> urlHandler)
        {
            _options.UrlHandler = urlHandler;
            return this;
        }

        /// <summary>设置KeepAlive
        /// </summary>
        public RequestBuilder SetKeepAlive(bool keepAlive = true)
        {
            _options.KeepAlive = keepAlive;
            return this;
        }

        /// <summary>设置允许自动重定向
        /// </summary>
        public RequestBuilder SetAllowAutoRedirect(bool allowAutoRedirect = true)
        {
            _options.AllowAutoRedirect = allowAutoRedirect;
            return this;
        }

        /// <summary>设置Range
        /// </summary>
        public RequestBuilder SetRange(long from, long to)
        {
            _options.RangeFrom = from;
            _options.RangeTo = to;
            return this;
        }

        /// <summary>设置CacheControl
        /// </summary>
        public RequestBuilder SetCacheControlNocache(bool cacheControlNocache)
        {
            _options.CacheControlNocache = cacheControlNocache;
            return this;
        }
        /// <summary>设置CacheControl
        /// </summary>
        public RequestBuilder SetCacheControlNostore(bool cacheControlNostore)
        {
            _options.CacheControlNostore = cacheControlNostore;
            return this;
        }
        /// <summary>设置Accept
        /// </summary>
        public RequestBuilder SetAccept(string accept)
        {
            _options.Accept = accept;
            return this;
        }

        /// <summary>设置证书,添加证书后会默认启用SSL
        /// </summary>
        public RequestBuilder SetCer(X509Certificate cer)
        {
            _options.ClientCer = cer;
            _options.IsSsl = true;
            return this;
        }



        /// <summary>设置认证参数
        /// </summary>
        public RequestBuilder SetAuthorization(string schema, string parameter)
        {
            _options.AuthorizationSchema = schema;
            _options.AuthorizationParameter = parameter;
            return this;
        }

        /// <summary>设置认证参数
        /// </summary>
        public RequestBuilder SetAuthorization(string parameter)
        {
            return SetAuthorization(RequestConsts.AuthenticationSchema.Basic, parameter);
        }

    }
}
