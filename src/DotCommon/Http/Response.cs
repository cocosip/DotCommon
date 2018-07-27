using System;
using System.Collections.Generic;
using System.Net;

namespace DotCommon.Http
{
    /// <summary>
    /// HTTP response data
    /// </summary>
    public class Response : IResponse
    {
        private string content;

        public Response()
        {
            ResponseStatus = ResponseStatus.None;
            Headers = new List<HttpHeader>();
            Cookies = new List<HttpCookie>();
        }

        /// <summary>响应的ContentType(MIME)
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>响应内容的长度
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>响应的编码
        /// </summary>
        public string ContentEncoding { get; set; }

        /// <summary>响应的内容
        /// </summary>
        public string Content
        {
            get { return content ?? (content = RawBytes.AsString()); }
        }

        /// <summary>
        /// HTTP response status code
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>Description of HTTP status returned
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>响应内容
        /// </summary>
        public byte[] RawBytes { get; set; }

        /// <summary>The URL that actually responded to the content (different from request if redirected)
        /// </summary>
        public Uri ResponseUri { get; set; }

        /// <summary>Server
        /// </summary>
        public string Server { get; set; }

        /// <summary>服务器端响应的Header
        /// </summary>
        public IList<HttpHeader> Headers { get; private set; }

        /// <summary>服务器端返回的Cookie
        /// </summary>
        public IList<HttpCookie> Cookies { get; private set; }

        /// <summary>响应状态,Http状态为错误的时候,仍然会响应
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }

        /// <summary>错误信息
        /// </summary>
        public string ErrorMessage { get; set; }


        /// <summary> Exception thrown when error is encountered.
        /// </summary>
        public Exception ErrorException { get; set; }

        /// <summary>
        /// The HTTP protocol version (1.0, 1.1, etc)
        /// </summary>
        /// <remarks>Only set when underlying framework supports it.</remarks>
        public Version ProtocolVersion { get; set; }
    }
}
