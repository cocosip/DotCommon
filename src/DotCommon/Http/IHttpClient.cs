using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace DotCommon.Http
{
    public interface IHttpClient
    {

        /// <summary>启用或者禁用自动压缩
        /// </summary>
        bool AutomaticDecompression { get; set; }

        /// <summary>最大允许跳转数量
        /// </summary>
        int? MaxRedirects { get; set; }

        /// <summary>X509CertificateCollection 客户端证书
        /// </summary>
        X509CertificateCollection ClientCertificates { get; set; }

        /// <summary>代理
        /// </summary>
        IWebProxy Proxy { get; set; }

        /// <summary>客户端请求缓存
        /// </summary>
        RequestCachePolicy CachePolicy { get; set; }

        /// <summary>Pipelined
        /// </summary>
        bool Pipelined { get; set; }

        /// <summary>是否自动重定向
        /// </summary>
        bool FollowRedirects { get; set; }

        /// <summary>CookieContainer
        /// </summary>
        CookieContainer CookieContainer { get; set; }

        /// <summary>UserAgent
        /// </summary>
        string UserAgent { get; set; }

        /// <summary>客户端请求超时时间,以毫秒为单位
        /// </summary>
        int Timeout { get; set; }

        /// <summary>读写的超时时间,以毫秒为单位
        /// </summary>
        int ReadWriteTimeout { get; set; }

        /// <summary>认证
        /// </summary>
        //public IAuthenticator Authenticator { get; set; }

        /// <summary>编码
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>PreAuthenticate
        /// </summary>
        bool PreAuthenticate { get; set; }

        /// <summary>允许高速NTLM身份验证的连接共享
        /// </summary>
        bool UnsafeAuthenticatedConnectionSharing { get; set; }

        /// <summary>RemoteCertificateValidationCallback
        /// </summary>
        RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

        /// <summary>默认参数
        /// </summary>
        IList<Parameter> DefaultParameters { get; }

        /// <summary>BaseHost
        /// </summary>
        string BaseHost { get; set; }

        /// <summary>如果您需要添加具有相同名称的多个默认参数，设置为true。只支持 query 和 form parameters
        /// </summary>
        bool AllowMultipleDefaultParametersWithSameName { get; set; }

        /// <summary>HttpWebRequest操作
        /// </summary>
        void ConfigureWebRequest(Action<HttpWebRequest> configurator);

        /// <summary>执行请求
        /// </summary>
        Task<IHttpResponse> ExecuteAsync(IHttpRequest request);

        /// <summary>下载数据
        /// </summary>
        Task<byte[]> DownloadData(IHttpRequest request);

        /// <summary>下载数据
        /// </summary>
        Task<byte[]> DownloadData(IHttpRequest request, bool throwOnError);
    }
}
