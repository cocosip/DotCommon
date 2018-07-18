using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DotCommon.Http
{
    public interface IHttp
    {
        /// <summary>自动压缩
        /// </summary>
        bool AutomaticDecompression { get; set; }

        /// <summary>总使用
        /// </summary>
        bool AlwaysMultipartFormData { get; set; }

        /// <summary>超时时间
        /// </summary>
        int Timeout { get; set; }

        /// <summary>读写超时时间
        /// </summary>
        int ReadWriteTimeout { get; set; }

        /// <summary>是否允许管道进行重定向
        /// </summary>
        bool Pipelined { get; set; }

        /// <summary>是否允许重定向
        /// </summary>
        bool FollowRedirects { get; set; }

        /// <summary>允许的最大的重定向的数量
        /// </summary>
        int? MaxRedirects { get; set; }

        /// <summary>是否使用默认的身份认证
        /// </summary>
        bool UseDefaultCredentials { get; set; }

        /// <summary>编码
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>RequestBody
        /// </summary>
        string RequestBody { get; set; }

        /// <summary>请求的ContentType
        /// </summary>
        string RequestContentType { get; set; }

        /// <summary>请求Body的二进制数据
        /// </summary>
        byte[] RequestBodyBytes { get; set; }

        /// <summary>Cookie
        /// </summary>
        CookieContainer CookieContainer { get; set; }

        /// <summary>
        /// </summary>
        Action<Stream> ResponseWriter { get; set; }

        /// <summary>请求URL
        /// </summary>
        Uri Url { get; set; }

        /// <summary>HOST
        /// </summary>
        string Host { get; set; }

        /// <summary>PreAuthenticate
        /// </summary>
        bool PreAuthenticate { get; set; }

        /// <summary>UnsafeAuthenticatedConnectionSharing
        /// </summary>
        bool UnsafeAuthenticatedConnectionSharing { get; set; }

        /// <summary>连接分组
        /// </summary>
        string ConnectionGroupName { get; set; }

        /// <summary>凭证
        /// </summary>
        ICredentials Credentials { get; set; }

        /// <summary>代理
        /// </summary>
        IWebProxy Proxy { get; set; }

        /// <summary>请求的x.509证书
        /// </summary>
        X509CertificateCollection ClientCertificates { get; set; }

        /// <summary>验证方法
        /// </summary>
        RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

        /// <summary>请求缓存
        /// </summary>
        RequestCachePolicy CachePolicy { get; set; }

        /// <summary>协议版本
        /// </summary>
        Version ProtocolVersion { get; set; }

        /// <summary>允许的压缩
        /// </summary>
        IList<DecompressionMethods> AllowedDecompressionMethods { get; set; }

        /// <summary>上传的文件
        /// </summary>
        IList<HttpFile> Files { get; }

        /// <summary>请求Header
        /// </summary>
        IList<HttpHeader> Headers { get; }

        /// <summary>请求参数
        /// </summary>
        IList<HttpParameter> Parameters { get; }

        /// <summary>Cookie
        /// </summary>
        IList<HttpCookie> Cookies { get; }

    }
}
