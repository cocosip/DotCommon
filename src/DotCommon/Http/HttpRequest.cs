using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace DotCommon.Http
{
    public class HttpRequest : IHttpRequest
    {

        private static readonly Regex PortSplitRegex = new Regex(@":\d+");

        /// <summary>请求参数
        /// </summary>
        public List<Parameter> Parameters { get; }

        /// <summary>请求文件
        /// </summary>
        public List<FileParameter> Files { get; }

        /// <summary>压缩方法列表
        /// </summary>
        private readonly IList<DecompressionMethods> alloweDecompressionMethods;

        /// <summary>压缩方法列表
        /// </summary>
        public IList<DecompressionMethods> AllowedDecompressionMethods =>
            alloweDecompressionMethods.Any()
                ? alloweDecompressionMethods
                : new[] { DecompressionMethods.None, DecompressionMethods.Deflate, DecompressionMethods.GZip };

        /// <summary>Accepts
        /// </summary>
        public IList<string> AcceptTypes { get; set; }

        /// <summary>基础Url
        /// </summary>
        public Uri BaseUrl { get; set; }

        /// <summary>Http请求的方法
        /// </summary>
        public Method Method { get; set; }

        /// <summary>Resource
        /// </summary>
        public string Resource { get; set; }

        /// <summary>格式
        /// </summary>
        public DataFormat RequestFormat { get; set; }

        /// <summary>时间格式化
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>凭据
        /// </summary>
        public ICredentials Credentials { get; set; }

        /// <summary> 超时时间,以毫秒为单位
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>读写超时时间,以毫秒为单位
        /// </summary>
        public int ReadWriteTimeout { get; set; }

        /// <summary>用户状态
        /// </summary>
        public object UserState { get; set; }

        /// <summary>总是使用MultipartFormData发送请求
        /// </summary>
        public bool AlwaysMultipartFormData { get; set; }

        /// </summary>
        public Action<Stream> ResponseWriter { get; set; }

        /// <summary>使用默认的凭据
        /// </summary>
        public bool UseDefaultCredentials { get; set; }

        /// <summary>编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>Keep-Alive
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>客户端证书
        /// </summary>
        public X509CertificateCollection ClientCertificates { get; set; }

        /// <summary>缓存
        /// </summary>
        public RequestCachePolicy CachePolicy { get; set; }

        /// <summary>是否允许重定向
        /// </summary>
        public bool FollowRedirects { get; set; }

        /// <summary>最大的重定向数量
        /// </summary>
        public int? MaxRedirects { get; set; }

        /// <summary>Pipelined
        /// </summary>
        public bool? Pipelined { get; set; }

        /// <summary>连接分组
        /// </summary>
        public string ConnectionGroupName { get; set; }

        /// <summary>CookieContainer
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>自动压缩
        /// </summary>
        public bool? AutomaticDecompression { get; set; }

        /// <summary>是否发送认证头部
        /// </summary>
        public bool? PreAuthenticate { get; set; }

        /// <summary>HttpWebRequest操作
        /// </summary>
        public Action<HttpWebRequest> WebRequestConfigurator { get; set; }

        /// <summary>默认构造函数
        /// </summary>
        public HttpRequest()
        {
            RequestFormat = DataFormat.Xml;
            Method = Method.GET;
            Parameters = new List<Parameter>();
            Files = new List<FileParameter>();
            alloweDecompressionMethods = new List<DecompressionMethods>();
        }

        /// <summary>Sets Method property to value of method
        /// </summary>
        public HttpRequest(Method method) : this()
        {
            Method = method;
        }

        /// <summary>
        /// </summary>
        public HttpRequest(string resource) : this(resource, Method.GET)
        {
        }

        /// <summary> Sets Resource and Method properties
        /// </summary>
        public HttpRequest(string resource, Method method) : this()
        {
            Resource = resource;
            Method = method;
        }

        /// <summary>
        /// <param name="resource">Resource to use for this request</param>
        public HttpRequest(Uri resource) : this(resource, Method.GET)
        {
        }

        /// <summary>
        ///     Sets Resource and Method properties
        /// </summary>
        /// <param name="resource">Resource to use for this request</param>
        /// <param name="method">Method to use for this request</param>
        public HttpRequest(Uri resource, Method method)
            : this(resource.IsAbsoluteUri
                ? resource.AbsolutePath + resource.Query
                : resource.OriginalString, method)
        {
            //resource.PathAndQuery not supported by Silverlight :(
        }



        /// <summary>添加文件
        /// </summary>
        public IHttpRequest AddFile(string name, string path, string contentType = null)
        {
            var f = new FileInfo(path);
            var fileLength = f.Length;

            return AddFile(new FileParameter
            {
                Name = name,
                FileName = Path.GetFileName(path),
                ContentLength = fileLength,
                Writer = s =>
                {
                    using (var file = new StreamReader(new FileStream(path, FileMode.Open)))
                    {
                        file.BaseStream.CopyTo(s);
                    }
                },
                ContentType = contentType
            });
        }

        /// <summary>添加文件
        /// </summary>
        public IHttpRequest AddFile(string name, byte[] bytes, string fileName, string contentType = null)
        {
            return AddFile(FileParameter.Create(name, bytes, fileName, contentType));
        }

        /// <summary>添加文件
        /// </summary>
        public IHttpRequest AddFile(string name, Action<Stream> writer, string fileName, long contentLength, string contentType = null)
        {
            return AddFile(new FileParameter
            {
                Name = name,
                Writer = writer,
                FileName = fileName,
                ContentLength = contentLength,
                ContentType = contentType
            });
        }

        /// <summary>添加byte[]文件
        /// </summary>>
        public IHttpRequest AddFileBytes(string name, byte[] bytes, string filename, string contentType = "application/x-gzip")
        {
            long length = bytes.Length;

            return AddFile(new FileParameter
            {
                Name = name,
                FileName = filename,
                ContentLength = length,
                ContentType = contentType,
                Writer = s =>
                {
                    using (var file = new StreamReader(new MemoryStream(bytes)))
                    {
                        file.BaseStream.CopyTo(s);
                    }
                }
            });
        }

        /// <summary>添加文件
        /// </summary>
        private IHttpRequest AddFile(FileParameter file)
        {
            Files.Add(file);
            return this;
        }

        /// <summary>添加Body
        /// </summary>
        public IHttpRequest AddBody(string contentType, string body)
        {
            RequestFormat = contentType.ToLowerInvariant() == "application/json" ? DataFormat.Json : DataFormat.Xml;
            return AddParameter(contentType.ToLowerInvariant(), body, ParameterType.RequestBody);
        }

        /// <summary>添加JSON数据
        /// </summary>
        public IHttpRequest AddJsonBody(string json)
        {
            RequestFormat = DataFormat.Json;
            return AddBody("application/json", json);
        }

        /// <summary>添加XML数据
        /// </summary>
        public IHttpRequest AddXmlBody(string xml)
        {
            RequestFormat = DataFormat.Xml;
            return AddBody("application/xml", xml);
        }

        /// <summary>添加参数
        /// </summary>
        public IHttpRequest AddParameter(Parameter p)
        {
            Parameters.Add(p);

            return this;
        }

        /// <summary>添加参数
        /// </summary>
        public IHttpRequest AddParameter(string name, object value)
        {
            return AddParameter(new Parameter
            {
                Name = name,
                Value = value,
                Type = ParameterType.GetOrPost
            });
        }

        /// <summary>添加参数
        /// </summary>
        public IHttpRequest AddParameter(string name, object value, ParameterType type)
        {
            return AddParameter(new Parameter
            {
                Name = name,
                Value = value,
                Type = type
            });
        }

        /// <summary>添加参数
        /// </summary>
        public IHttpRequest AddParameter(string name, object value, string contentType, ParameterType type)
        {
            return AddParameter(new Parameter
            {
                Name = name,
                Value = value,
                ContentType = contentType,
                Type = type
            });
        }

        /// <summary>添加或者修改参数
        /// </summary>
        public IHttpRequest AddOrUpdateParameter(Parameter p)
        {
            if (Parameters.Any(param => param.Name == p.Name))
            {
                var parameter = Parameters.First(param => param.Name == p.Name);
                parameter.Value = p.Value;
                return this;
            }
            Parameters.Add(p);
            return this;
        }

        /// <summary>添加或者修改参数
        /// </summary>
        public IHttpRequest AddOrUpdateParameter(string name, object value)
        {
            return AddOrUpdateParameter(new Parameter
            {
                Name = name,
                Value = value,
                Type = ParameterType.GetOrPost
            });
        }

        /// <summary>添加或者修改参数
        /// </summary>
        public IHttpRequest AddOrUpdateParameter(string name, object value, ParameterType type)
        {
            return AddOrUpdateParameter(
                new Parameter
                {
                    Name = name,
                    Value = value,
                    Type = type
                });
        }

        /// <summary>添加或者修改参数
        /// </summary>
        public IHttpRequest AddOrUpdateParameter(string name, object value, string contentType, ParameterType type)
        {
            return AddOrUpdateParameter(new Parameter
            {
                Name = name,
                Value = value,
                ContentType = contentType,
                Type = type
            });
        }

        /// <summary>添加头部
        /// </summary>
        public IHttpRequest AddHeader(string name, string value)
        {
            bool InvalidHost(string host)
            {
                return Uri.CheckHostName(PortSplitRegex.Split(host)[0]) == UriHostNameType.Unknown;
            }

            if (name == "Host" && InvalidHost(value))
                throw new ArgumentException("The specified value is not a valid Host header string.", "value");
            return AddParameter(name, value, ParameterType.HttpHeader);
        }

        /// <summary>添加Cookie
        /// </summary>
        public IHttpRequest AddCookie(string name, string value)
        {
            return AddParameter(name, value, ParameterType.Cookie);
        }

        /// <summary>添加URL片段
        /// </summary>
        public IHttpRequest AddUrlSegment(string name, string value)
        {
            return AddParameter(name, value, ParameterType.UrlSegment);
        }


        /// <summary>添加QueryString
        /// </summary>
        public IHttpRequest AddQueryParameter(string name, string value)
        {
            return AddParameter(name, value, ParameterType.QueryString);
        }

        /// <summary>添加Decompression
        /// </summary>
        public IHttpRequest AddDecompressionMethod(DecompressionMethods decompressionMethod)
        {
            if (!alloweDecompressionMethods.Contains(decompressionMethod))
                alloweDecompressionMethods.Add(decompressionMethod);

            return this;
        }

    }
}
