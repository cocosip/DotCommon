using System;

namespace DotCommon.Http
{
    public class HttpCookie
    {
        /// <summary>Comment
        /// </summary>
        public string Comment { get; set; }

        /// <summary>CommentUri
        /// </summary>
        public Uri CommentUri { get; set; }

        /// <summary>在会话结束后,Cookie是否要被丢弃
        /// </summary>
        public bool Discard { get; set; }

        /// <summary>Cookie域
        /// </summary>
        public string Domain { get; set; }

        /// <summary>是否过期
        /// </summary>
        public bool Expired { get; set; }

        /// <summary>过期时间
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>表明Cookie只能访问服务器
        /// </summary>
        public bool HttpOnly { get; set; }

        /// <summary>Cookie名
        /// </summary>
        public string Name { get; set; }

        /// <summary>Cookie路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>cookie端口
        /// </summary>
        public string Port { get; set; }

        /// <summary>表明Cookie是否只能通过安全通道发送
        /// </summary>
        public bool Secure { get; set; }

        public DateTime TimeStamp { get; set; }

        public string Value { get; set; }

        public int Version { get; set; }
    }
}
