using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;

namespace DotCommon.Http
{
    public class HttpRequest : IHttpRequest
    {
        /// <summary>端口检测表达式
        /// </summary>
        private static readonly Regex PortSplitRegex = new Regex(@":\d+");

        /// <summary>请求的URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>编码
        /// </summary>
        public Encoding Encoding { get; set; }

        /// <summary>请求方法,GET或者POST或者其他
        /// </summary>
        public Method Method { get; set; }

        /// <summary>超时时间
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>读取或者写入超时时间
        /// </summary>
        public int ReadWriteTimeout { get; set; }

        /// <summary> 总是发送 multipart/form-data 请求,即使没有文件的时候
        /// </summary>
        public bool AlwaysMultipartFormData { get; set; }

        /// <summary>使用默认凭证
        /// </summary>
        public bool UseDefaultCredentials { get; set; }

        /// <summary>PreAuthenticate
        /// </summary>
        public bool PreAuthenticate { get; set; }

        /// <summary> Allow high-speed NTLM-authenticated connection sharing
        /// </summary>
        public bool UnsafeAuthenticatedConnectionSharing { get; set; }

        /// <summary>是否自动压缩
        /// </summary>
        public bool AutomaticDecompression { get; set; }

        /// <summary>是否允许管道进行重定向
        /// </summary>
        public bool Pipelined { get; set; }

        /// <summary>是否允许重定向
        /// </summary>
        public bool FollowRedirects { get; set; }

        /// <summary>最大的重定向数量
        /// </summary>
        public int MaxRedirects { get; set; }

        /// <summary>连接分组
        /// </summary>
        public string ConnectionGroupName { get; set; }

        /// <summary>缓存策略
        /// </summary>
        public RequestCachePolicy CachePolicy { get; set; }

        /// <summary>设置CookieContainer
        /// </summary>
        public CookieContainer CookieContainer { get; set; }

        /// <summary>凭据
        /// </summary>
        public ICredentials Credentials { get; set; }

        /// <summary>代理
        /// </summary>
        public IWebProxy Proxy { get; set; }

        /// <summary>客户端x.509证书
        /// </summary>
        public X509CertificateCollection ClientCertificates { get; set; }

        /// <summary>验证方法
        /// </summary>
        public RemoteCertificateValidationCallback RemoteCertificateValidationCallback { get; set; }

        /// <summary>Accept类型
        /// </summary>
        public IList<string> AcceptTypes { get; set; }

        /// <summary>请求参数
        /// </summary>
        public List<Parameter> Parameters { get; } = new List<Parameter>();

        /// <summary>上传文件
        /// </summary>
        public List<FileParameter> Files { get; } = new List<FileParameter>();

        /// <summary>允许的解压方法
        /// </summary>
        private readonly IList<DecompressionMethods> alloweDecompressionMethods;


        public HttpRequest(string url)
        {
            Url = url;
            Method = Method.GET;
            alloweDecompressionMethods = new List<DecompressionMethods>();
        }

        public HttpRequest(string url, Method method) : this(url)
        {
            Method = Method;
        }

        public IList<DecompressionMethods> AllowedDecompressionMethods =>
          alloweDecompressionMethods.Any()
              ? alloweDecompressionMethods
              : new[] { DecompressionMethods.None, DecompressionMethods.Deflate, DecompressionMethods.GZip };

        /// <summary>添加Http请求参数 (QueryString for GET, DELETE, OPTIONS and HEAD; Encoded form for POST and PUT)
        /// </summary>
        public IHttpRequest AddParameter(Parameter p)
        {
            Parameters.Add(p);
            return this;
        }

        /// <summary>添加Http请求参数 (QueryString for GET, DELETE, OPTIONS and HEAD; Encoded form for POST and PUT)
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

        /// <summary>添加Http请求参数 (QueryString for GET, DELETE, OPTIONS and HEAD; Encoded form for POST and PUT)
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

        /// <summary>添加Http请求参数 (QueryString for GET, DELETE, OPTIONS and HEAD; Encoded form for POST and PUT)
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

        /// <summary>移除参数
        /// </summary>
        public IHttpRequest RemoveParameter(string name)
        {
            Parameter parameter = Parameters.SingleOrDefault(
                p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

            if (parameter != null)
            {
                Parameters.Remove(parameter);
            }
            return this;
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

        /// <summary>添加请求头部
        /// </summary>
        public IHttpRequest AddHeader(string name, string value)
        {
            bool InvalidHost(string host)
            {
                return Uri.CheckHostName(PortSplitRegex.Split(host)[0]) == UriHostNameType.Unknown;
            }

            if (name == "Host" && InvalidHost(value))
                throw new ArgumentException("指定的值不是有效的主机标题的字符串.", "value");
            return AddParameter(name, value, ParameterType.HttpHeader);
        }

        /// <summary>添加Cookie
        /// </summary>
        public IHttpRequest AddCookie(string name, string value)
        {
            return AddParameter(name, value, ParameterType.Cookie);
        }

        /// <summary>添加Url链接片段
        /// </summary>
        public IHttpRequest AddUrlSegment(string name, string value)
        {
            return AddParameter(name, value, ParameterType.UrlSegment);
        }

        /// <summary>添加查询参数
        /// </summary>
        public IHttpRequest AddQueryParameter(string name, string value)
        {
            return AddParameter(name, value, ParameterType.QueryString);
        }

        /// <summary>添加压缩方法
        /// </summary>
        public IHttpRequest AddDecompressionMethod(DecompressionMethods decompressionMethod)
        {
            if (!alloweDecompressionMethods.Contains(decompressionMethod))
                alloweDecompressionMethods.Add(decompressionMethod);

            return this;
        }

        /// <summary>添加文件
        /// </summary>
        public IHttpRequest AddFile(FileParameter file)
        {
            Files.Add(file);
            return this;
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

        /// <summary>添加二进制文件
        /// </summary>
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

        /// <summary>添加Body请求数据
        /// </summary>
        public IHttpRequest AddBody(string body, DataFormat format)
        {
            var contentType = format == DataFormat.Json ? "application/json" : "application/xml";
            return AddParameter(contentType, body, ParameterType.RequestBody);
        }

        /// <summary>添加Json请求数据
        /// </summary>
        public IHttpRequest AddJsonBody(string body)
        {
            return AddParameter("application/json", body, ParameterType.RequestBody);
        }

        /// <summary>添加XML请求数据
        /// </summary>
        public IHttpRequest AddXmlBody(string body)
        {
            return AddParameter("application/xml", body, ParameterType.RequestBody);
        }


    }
}
