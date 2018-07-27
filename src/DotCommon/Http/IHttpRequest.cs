using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace DotCommon.Http
{
    public interface IHttpRequest
    {
        /// <summary>请求参数
        /// </summary>
        List<Parameter> Parameters { get; }

        /// <summary>请求文件
        /// </summary>
        List<FileParameter> Files { get; }

        /// <summary>压缩方法列表
        /// </summary>
        IList<DecompressionMethods> AllowedDecompressionMethods { get; }

        /// <summary>Accepts
        /// </summary>
        IList<string> AcceptTypes { get; set; }

        /// <summary>BaseUrl
        /// </summary>
        Uri BaseUrl { get; set; }

        /// <summary>Http请求的方法
        /// </summary>
        Method Method { get; set; }

        /// <summary>Resource
        /// </summary>
        string Resource { get; set; }

        /// <summary>格式
        /// </summary>
        DataFormat RequestFormat { get; set; }

        /// <summary>时间格式化
        /// </summary>
        string DateFormat { get; set; }

        /// <summary>凭据
        /// </summary>
        ICredentials Credentials { get; set; }

        /// <summary> 超时时间,以毫秒为单位
        /// </summary>
        int Timeout { get; set; }

        /// <summary>读写超时时间,以毫秒为单位
        /// </summary>
        int ReadWriteTimeout { get; set; }

        /// <summary>用户状态
        /// </summary>
        object UserState { get; set; }

        /// <summary>总是使用MultipartFormData发送请求
        /// </summary>
        bool AlwaysMultipartFormData { get; set; }

        Action<Stream> ResponseWriter { get; set; }

        /// <summary>编码
        /// </summary>
        Encoding Encoding { get; set; }

        /// <summary>使用默认的凭据
        /// </summary>
        bool UseDefaultCredentials { get; set; }

        /// <summary>Keep-Alive
        /// </summary>
        bool KeepAlive { get; set; }

        /// <summary>客户端证书
        /// </summary>
        X509CertificateCollection ClientCertificates { get; set; }

        /// <summary>缓存
        /// </summary>
        RequestCachePolicy CachePolicy { get; set; }

        /// <summary>是否允许重定向
        /// </summary>
        bool FollowRedirects { get; set; }

        /// <summary>最大的重定向数量
        /// </summary>
        int? MaxRedirects { get; set; }

        /// <summary>Pipelined
        /// </summary>
        bool? Pipelined { get; set; }

        /// <summary>连接分组
        /// </summary>
        string ConnectionGroupName { get; set; }

        /// <summary>CookieContainer
        /// </summary>
        CookieContainer CookieContainer { get; set; }

        /// <summary>自动压缩
        /// </summary>
        bool? AutomaticDecompression { get; set; }

        /// <summary>是否发送认证头部
        /// </summary>
        bool? PreAuthenticate { get; set; }

        /// <summary>HttpWebRequest操作
        /// </summary>
        Action<HttpWebRequest> WebRequestConfigurator { get; set; }

        /// <summary>添加文件
        /// </summary>
        IHttpRequest AddFile(string name, string path, string contentType = null);

        /// <summary>添加文件
        /// </summary>
        IHttpRequest AddFile(string name, byte[] bytes, string fileName, string contentType = null);

        /// <summary>添加文件
        /// </summary>
        IHttpRequest AddFile(string name, Action<Stream> writer, string fileName, long contentLength, string contentType = null);

        /// <summary>添加byte[]文件
        /// </summary>>
        IHttpRequest AddFileBytes(string name, byte[] bytes, string filename, string contentType = "application/x-gzip");

        /// <summary>添加Body
        /// </summary>
        IHttpRequest AddBody(string contentType, string body);

        /// <summary>添加JSON数据
        /// </summary>
        IHttpRequest AddJsonBody(string json);

        /// <summary>添加XML数据
        /// </summary>
        IHttpRequest AddXmlBody(string xml);

        /// <summary>添加参数
        /// </summary>
        IHttpRequest AddParameter(Parameter p);

        /// <summary>添加参数
        /// </summary>
        IHttpRequest AddParameter(string name, object value);

        /// <summary>添加参数
        /// </summary>
        IHttpRequest AddParameter(string name, object value, ParameterType type);

        /// <summary>添加参数
        /// </summary>
        IHttpRequest AddParameter(string name, object value, string contentType, ParameterType type);

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

        /// <summary>添加头部
        /// </summary>
        IHttpRequest AddHeader(string name, string value);

        /// <summary>添加Cookie
        /// </summary>
        IHttpRequest AddCookie(string name, string value);

        /// <summary>添加URL片段
        /// </summary>
        IHttpRequest AddUrlSegment(string name, string value);

        /// <summary>添加QueryString
        /// </summary>
        IHttpRequest AddQueryParameter(string name, string value);

        /// <summary>添加Decompression
        /// </summary>
        IHttpRequest AddDecompressionMethod(DecompressionMethods decompressionMethod);


    }
}
