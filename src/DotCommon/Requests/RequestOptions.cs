using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace DotCommon.Requests
{
    public class RequestOptions
    {
        /// <summary>请求地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>请求方式
        /// </summary>
        public string HttpMethod { get; set; }

        /// <summary>编码
        /// </summary>
        public string Encode { get; set; } = "utf-8";

        /// <summary>请求参数
        /// </summary>
        public Dictionary<string, string> RequestParameters = new Dictionary<string, string>();

        /// <summary>请求头部
        /// </summary>
        public Dictionary<string, string> RequestHeaders = new Dictionary<string, string>();

        /// <summary>请求的文件
        /// </summary>
        public List<RequestFile> RequestFiles = new List<RequestFile>();

        /// <summary>UserAgent
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>Referer
        /// </summary>
        public string Referer { get; set; }

        /// <summary>Cookie
        /// </summary>
        public Cookie Cookie { get; set; }

        /// <summary>是否进行Url编码
        /// </summary>
        public bool IsUrlEncode { get; set; }

        /// <summary>GET请求参数设置参数操作
        /// </summary>
        public Func<KeyValuePair<string, string>, string> UrlHandler { get; set; }

        /// <summary>Url编码格式
        /// </summary>
        public string UrlEncode { get; set; } = "utf-8";

        /// <summary>KeepAlive
        /// </summary>
        public bool KeepAlive { get; set; } = true;

        /// <summary>Range from
        /// </summary>
        public long RangeFrom { get; set; } = 0;

        /// <summary>Range to
        /// </summary>
        public long RangeTo { get; set; } = 0;

        /// <summary>设置CacheControl
        /// </summary>
        public bool CacheControlNocache { get; set; } = true;
        /// <summary>设置CacheControl
        /// </summary>
        public bool CacheControlNostore { get; set; } = true;
        /// <summary>请求报头域用于指定客户端接受哪些类型的信息
        /// </summary>
        public string Accept { get; set; }

        /// <summary>Post请求时的类型
        /// </summary>
        public PostType PostType { get; set; }

        /// <summary>设置Json或者Xml请求的数据
        /// </summary>
        public string PostString { get; set; }

        /// <summary>是否SSL
        /// </summary>
        public bool IsSsl { get; set; } = false;

        /// <summary>x.509证书
        /// </summary>
        public X509Certificate Cer { get; set; }

        /// <summary>认证架构
        /// </summary>
        public string AuthorizationSchema { get; set; }

        /// <summary>认证参数
        /// </summary>
        public string AuthorizationParameter { get; set; }

        public override string ToString()
        {
            return $"[Url]:{Url},[HttpMethod]:{HttpMethod}";
        }
    }
}
