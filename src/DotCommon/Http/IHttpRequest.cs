using System.Collections.Generic;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DotCommon.Http
{
    public interface IHttpRequest
    {
        /// <summary>请求的URL
        /// </summary>
        string Url { get; set; }

        /// <summary>请求方法,GET或者POST或者其他
        /// </summary>
        Method Method { get; set; }

        /// <summary>编码
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>超时时间
        /// </summary>
        int Timeout { get; set; }

        /// <summary>读取或者写入超时时间
        /// </summary>
        int ReadWriteTimeout { get; set; }

        /// <summary> 总是发送 multipart/form-data 请求,即使没有文件的时候
        /// </summary>
        bool AlwaysMultipartFormData { get; set; }

        /// <summary>使用默认凭证
        /// </summary>
        bool UseDefaultCredentials { get; set; }

        /// <summary>PreAuthenticate
        /// </summary>
        bool PreAuthenticate { get; set; }

        /// <summary> Allow high-speed NTLM-authenticated connection sharing
        /// </summary>
        bool UnsafeAuthenticatedConnectionSharing { get; set; }

        /// <summary>是否自动压缩
        /// </summary>
        bool AutomaticDecompression { get; set; }

        /// <summary>是否允许管道进行重定向
        /// </summary>
        bool Pipelined { get; set; }

        /// <summary>是否允许重定向
        /// </summary>
        bool FollowRedirects { get; set; }

        /// <summary>最大的重定向数量
        /// </summary>
        int MaxRedirects { get; set; }

        /// <summary>连接分组
        /// </summary>
        string ConnectionGroupName { get; set; }

        /// <summary>缓存策略
        /// </summary>
        RequestCachePolicy CachePolicy { get; set; }

        /// <summary>设置CookieContainer
        /// </summary>
        CookieContainer CookieContainer { get; set; }

        /// <summary>凭据
        /// </summary>
        ICredentials Credentials { get; set; }

        /// <summary>代理
        /// </summary>
        IWebProxy Proxy { get; set; }

        /// <summary>客户端x.509证书
        /// </summary>
        X509CertificateCollection ClientCertificates { get; set; }

        /// <summary>验证方法
        /// </summary>
        RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

        /// <summary>Accept类型
        /// </summary>
        IList<string> AcceptTypes { get; set; }

        /// <summary>请求参数
        /// </summary>
        List<Parameter> Parameters { get; }

        /// <summary>上传文件
        /// </summary>
        List<FileParameter> Files { get; }

        /// <summary>允许的压缩方式
        /// </summary>
        IList<DecompressionMethods> AllowedDecompressionMethods { get; }


        /// <summary>添加参数
        /// </summary>
        IHttpRequest AddParameter(Parameter p);

        /// <summary>添加Http请求参数 (QueryString for GET, DELETE, OPTIONS and HEAD; Encoded form for POST and PUT)
        /// </summary>
        IHttpRequest AddParameter(string name, object value);

        /// <summary>添加Http请求参数 (QueryString for GET, DELETE, OPTIONS and HEAD; Encoded form for POST and PUT)
        /// </summary>
        IHttpRequest AddParameter(string name, object value, ParameterType type);

        /// <summary>添加Http请求参数 (QueryString for GET, DELETE, OPTIONS and HEAD; Encoded form for POST and PUT)
        /// </summary>
        IHttpRequest AddParameter(string name, object value, string contentType, ParameterType type);

        /// <summary>移除参数
        /// </summary>
        IHttpRequest RemoveParameter(string name);

        /// <summary>添加或者修改参数
        /// </summary>
        IHttpRequest AddOrUpdateParameter(Parameter p);

        /// <summary>添加或者修改参数
        /// </summary>
        IHttpRequest AddOrUpdateParameter(string name, object value);

        /// <summary>添加或者修改参数
        /// </summary>
        IHttpRequest AddOrUpdateParameter(string name, object value, ParameterType type);

        /// <summary>添加或者修改参数
        /// </summary>
        IHttpRequest AddOrUpdateParameter(string name, object value, string contentType, ParameterType type);

        /// <summary>添加请求头部
        /// </summary>
        IHttpRequest AddHeader(string name, string value);

        /// <summary>添加Cookie
        /// </summary>
        IHttpRequest AddCookie(string name, string value);

        /// <summary>添加Url链接片段
        /// </summary>
        IHttpRequest AddUrlSegment(string name, string value);

        /// <summary>添加查询参数
        /// </summary>
        IHttpRequest AddQueryParameter(string name, string value);

        /// <summary>添加压缩方法
        /// </summary>
        IHttpRequest AddDecompressionMethod(DecompressionMethods decompressionMethod);

        /// <summary>添加文件
        /// </summary>
        IHttpRequest AddFile(FileParameter file);

        /// <summary>添加文件
        /// </summary>
        IHttpRequest AddFile(string name, string path, string contentType = null);

        /// <summary>添加文件
        /// </summary>
        IHttpRequest AddFile(string name, byte[] bytes, string fileName, string contentType = null);

        /// <summary>添加二进制文件
        /// </summary>
        IHttpRequest AddFileBytes(string name, byte[] bytes, string filename, string contentType = "application/x-gzip");

        /// <summary>添加Body请求数据
        /// </summary>
        IHttpRequest AddBody(string body, DataFormat format);

        /// <summary>添加Json请求数据
        /// </summary>
        IHttpRequest AddJsonBody(string body);

        /// <summary>添加XML请求数据
        /// </summary>
        IHttpRequest AddXmlBody(string body);

    }
}
