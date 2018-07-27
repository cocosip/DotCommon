using System;
using System.Collections.Generic;
using System.Net;

namespace DotCommon.Http
{
    public interface IResponse
    {
        /// <summary>响应的ContentType(MIME)
        /// </summary>
        string ContentType { get; set; }

        /// <summary>响应内容的长度
        /// </summary>
        long ContentLength { get; set; }

        /// <summary>响应的编码
        /// </summary>
        string ContentEncoding { get; set; }

        /// <summary>响应的内容
        /// </summary>
        string Content { get; }

        /// <summary>Http状态码
        /// </summary>
        HttpStatusCode StatusCode { get; set; }

        /// <summary>Description of HTTP status returned
        /// </summary>
        string StatusDescription { get; set; }

        /// <summary>响应内容
        /// </summary>
        byte[] RawBytes { get; set; }

        /// <summary>The URL that actually responded to the content (different from request if redirected)
        /// </summary>
        Uri ResponseUri { get; set; }

        /// <summary>Server
        /// </summary>
        string Server { get; set; }

        /// <summary>服务器端响应的Header
        /// </summary>
        IList<HttpHeader> Headers { get; }

        /// <summary>服务器端返回的Cookie
        /// </summary>
        IList<HttpCookie> Cookies { get; }

        /// <summary>响应状态,Http状态为错误的时候,仍然会响应
        /// </summary>
        ResponseStatus ResponseStatus { get; set; }

        /// <summary>错误信息
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary> Exception thrown when error is encountered.
        /// </summary>
        Exception ErrorException { get; set; }

        /// <summary>The HTTP protocol version (1.0, 1.1, etc)
        /// </summary>
        Version ProtocolVersion { get; set; }
    }
}
