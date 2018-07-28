using System;
using System.Collections.Generic;
using System.Net;

namespace DotCommon.Http
{
    public interface IHttpResponse
    {
        /// <summary>请求
        /// </summary>
        IHttpRequest Request { get; set; }

        /// <summary>MIME ContentType
        /// </summary>
        string ContentType { get; set; }

        /// <summary>内容长度
        /// </summary>
        long ContentLength { get; set; }

        /// <summary>内容编码
        /// </summary>
        string ContentEncoding { get; set; }

        /// <summary>内容
        /// </summary>
        string Content { get; set; }

        /// <summary>Http响应状态
        /// </summary>
        HttpStatusCode StatusCode { get; set; }

        /// <summary>是否成功
        /// </summary>
        bool IsSuccessful { get; }

        /// <summary>Http状态描述
        /// </summary>
        string StatusDescription { get; set; }

        /// <summary>响应内容
        /// </summary>
        byte[] RawBytes { get; set; }

        /// <summary>响应Uri
        /// </summary>
        Uri ResponseUri { get; set; }

        /// <summary>Server
        /// </summary>
        string Server { get; set; }

        /// <summary>响应Cookie
        /// </summary>
        IList<HttpResponseCookie> Cookies { get; }

        /// <summary>响应头部
        /// </summary>
        IList<Parameter> Headers { get; }

        /// <summary>响应状态
        /// </summary>
        ResponseStatus ResponseStatus { get; set; }

        /// <summary>错误信息
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>异常信息
        /// </summary>
        Exception ErrorException { get; set; }

        /// <summary>The HTTP protocol version (1.0, 1.1, etc)
        /// </summary>
        Version ProtocolVersion { get; set; }

    }


    public interface IHttpResponse<T> : IHttpResponse
    {
        T Data { get; set; }
    }
}
