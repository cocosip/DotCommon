using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

namespace DotCommon.Requests
{
    /// <summary>Request客户端配置
    /// </summary>
    public class ClientConfig
    {
        /// <summary>是否允许自动跳转
        /// </summary>
        public bool AllowAutoRedirect { get; set; } = false;

        /// <summary>获取或设置处理程序的使用的请求内容的最大缓冲区大小
        /// </summary>
        public long MaxRequestContentBufferSize { get; set; } = 2048 * 1000;

        /// <summary>获取或设置将跟随的处理程序的重定向的最大数目
        /// </summary>
        public int MaxAutomaticRedirections { get; set; } = 3;

        /// <summary>是否默认保持长连接
        /// </summary>
        public bool IsDefaultKeepAlive { get; set; } = false;

        /// <summary>默认是否开启SSL
        /// </summary>
        public bool IsSsl { get; set; } = false;

        /// <summary>默认证书
        /// </summary>
        public List<X509Certificate> Cers { get; set; } = new List<X509Certificate>();
    }
}
