using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace DotCommon.Requests
{
    public class RequestBuilder
    {
        private readonly Lazy<RequestOptions> _options = new Lazy<RequestOptions>();

        public RequestBuilder(string url, string httpMethod)
        {
            _options.Value.Url = url;
            _options.Value.HttpMethod = httpMethod;
        }

        public static RequestBuilder Instance(string url, string httpMethod)
        {
            return new RequestBuilder(url, httpMethod);
        }

        public RequestOptions GetOptions()
        {
            return _options.Value;
        }

        public RequestBuilder SetEncode(string encode = "utf-8")
        {
            _options.Value.Encode = encode;
            return this;
        }

        public RequestBuilder AttachHeader(string key, string value)
        {
            _options.Value.RequestHeaders.Value.Add(key, value);
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
            _options.Value.RequestParameters.Value.Add(key, value);
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
            _options.Value.RequestFiles.Value.Add(file);
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
            _options.Value.UserAgent = userAgent;
            return this;
        }

        public RequestBuilder SetReferer(string referer)
        {
            _options.Value.Referer = referer;
            return this;
        }

        public RequestBuilder SetCookie(Cookie cookie)
        {
            if (_options.Value.Cookie == null)
            {
                _options.Value.Cookie = cookie;
            }
            return this;
        }

        public RequestBuilder SetPost(PostType postType, string postString)
        {
            if (_options.Value.HttpMethod.ToUpper() == "POST")
            {
                _options.Value.PostType = postType;
                _options.Value.PostString = postString;
            }
            return this;
        }

        public RequestBuilder SetPost(PostType postType)
        {
            if (_options.Value.HttpMethod.ToUpper() == "POST")
            {
                _options.Value.PostType = postType;
            }
            return this;
        }

        public RequestBuilder SetUrlEncode(string encode = "utf-8")
        {
            _options.Value.IsUrlEncode = true;
            _options.Value.UrlEncode = encode;
            return this;
        }


        /// <summary>设置KeepAlive
        /// </summary>
        public RequestBuilder SetKeepAlive()
        {
            _options.Value.KeepAlive = true;
            return this;
        }

        /// <summary>设置Range
        /// </summary>
        public RequestBuilder SetRange(long from, long to)
        {
            _options.Value.RangeFrom = from;
            _options.Value.RangeTo = to;
            return this;
        }

        /// <summary>设置CacheControl
        /// </summary>
        public RequestBuilder SetCacheControlNocache(bool cacheControlNocache)
        {
            _options.Value.CacheControlNocache = cacheControlNocache;
            return this;
        }
        /// <summary>设置CacheControl
        /// </summary>
        public RequestBuilder SetCacheControlNostore(bool cacheControlNostore)
        {
            _options.Value.CacheControlNostore = cacheControlNostore;
            return this;
        }
        /// <summary>设置Accept
        /// </summary>
        public RequestBuilder SetAccept(string accept)
        {
            _options.Value.Accept = accept;
            return this;
        }

        /// <summary>设置证书,添加证书后会默认启用SSL
        /// </summary>
        public RequestBuilder SetCer(X509Certificate cer)
        {
            _options.Value.Cer = cer;
            _options.Value.IsSsl = true;
            return this;
        }



        /// <summary>设置认证参数
        /// </summary>
        public RequestBuilder SetAuthorization(string schema, string parameter)
        {
            _options.Value.AuthorizationSchema = schema;
            _options.Value.AuthorizationParameter = parameter;
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
