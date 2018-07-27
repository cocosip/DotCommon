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
        Action<Stream> ResponseWriter { get; set; }

        CookieContainer CookieContainer { get; set; }

        ICredentials Credentials { get; set; }

        /// <summary>
        /// Enable or disable automatic gzip/deflate decompression
        /// </summary>
        bool AutomaticDecompression { get; set; }

        /// <summary>
        /// Always send a multipart/form-data request - even when no Files are present.
        /// </summary>
        bool AlwaysMultipartFormData { get; set; }

        string UserAgent { get; set; }

        int Timeout { get; set; }

        int ReadWriteTimeout { get; set; }

        bool FollowRedirects { get; set; }

        bool Pipelined { get; set; }

        X509CertificateCollection ClientCertificates { get; set; }

        int? MaxRedirects { get; set; }

        bool UseDefaultCredentials { get; set; }

        Encoding Encoding { get; set; }

        /// <summary>KeepAlive
        /// </summary>
        bool KeepAlive { get; set; }

        string RequestBody { get; set; }

        string RequestContentType { get; set; }

        bool PreAuthenticate { get; set; }

        bool UnsafeAuthenticatedConnectionSharing { get; set; }

        RequestCachePolicy CachePolicy { get; set; }

        string ConnectionGroupName { get; set; }

        /// <summary>
        /// An alternative to RequestBody, for when the caller already has the byte array.
        /// </summary>
        byte[] RequestBodyBytes { get; set; }

        Uri Url { get; set; }

        string Host { get; set; }

        IWebProxy Proxy { get; set; }

        RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

        Action<HttpWebRequest> WebRequestConfigurator { get; set; }

        IList<DecompressionMethods> AllowedDecompressionMethods { get; set; }

        IList<HttpHeader> Headers { get; }

        IList<HttpParameter> Parameters { get; }

        IList<HttpFile> Files { get; }

        IList<HttpCookie> Cookies { get; }
    }
}
