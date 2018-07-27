using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DotCommon.Http
{

    public abstract class HttpResponseBase
    {
        private string content;

        /// <summary>
        ///     Default constructor
        /// </summary>
        protected HttpResponseBase()
        {
            ResponseStatus = ResponseStatus.None;
            Headers = new List<Parameter>();
            Cookies = new List<HttpResponseCookie>();
        }

        /// <summary>请求
        /// </summary>
        public IHttpRequest Request { get; set; }

        /// <summary>MIME ContentType
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>内容长度
        /// </summary>
        public long ContentLength { get; set; }

        /// <summary>内容编码
        /// </summary>
        public string ContentEncoding { get; set; }

        /// <summary>内容
        /// </summary>
        public string Content
        {
            get => content ?? (content = RawBytes.AsString());
            set => content = value;
        }

        /// <summary>Http响应状态
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>是否成功
        /// </summary>
        public bool IsSuccessful => (int)StatusCode >= 200 && (int)StatusCode <= 299 &&
                                    ResponseStatus == ResponseStatus.Completed;

        /// <summary>Http状态描述
        /// </summary>
        public string StatusDescription { get; set; }

        /// <summary>响应内容
        /// </summary>
        public byte[] RawBytes { get; set; }

        /// <summary>响应Uri
        /// </summary>
        public Uri ResponseUri { get; set; }

        /// <summary>Server
        /// </summary>
        public string Server { get; set; }

        /// <summary>响应Cookie
        /// </summary>
        public IList<HttpResponseCookie> Cookies { get; protected internal set; }

        /// <summary>响应头部
        /// </summary>
        public IList<Parameter> Headers { get; protected internal set; }

        /// <summary>响应状态
        /// </summary>
        public ResponseStatus ResponseStatus { get; set; }

        /// <summary>错误信息
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>异常信息
        /// </summary>
        public Exception ErrorException { get; set; }

        /// <summary>The HTTP protocol version (1.0, 1.1, etc)
        /// </summary>
        public Version ProtocolVersion { get; set; }

        /// <summary>
        ///     Assists with debugging responses by displaying in the debugger output
        /// </summary>
        /// <returns></returns>
        protected string DebuggerDisplay()
        {
            return string.Format("StatusCode: {0}, Content-Type: {1}, Content-Length: {2})",
                StatusCode, ContentType, ContentLength);
        }
    }

    public class HttpResponse : HttpResponseBase
    {
    }
}
