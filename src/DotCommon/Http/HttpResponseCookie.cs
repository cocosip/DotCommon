using System;
using System.Collections.Generic;
using System.Text;

namespace DotCommon.Http
{
    public class HttpResponseCookie
    {
        /// <summary>Comment of the cookie
        /// </summary>
        public string Comment { get; set; }

        /// <summary>Comment of the cookie
        /// </summary>
        public Uri CommentUri { get; set; }

        /// <summary> Indicates whether the cookie should be discarded at the end of the session
        /// </summary>
        public bool Discard { get; set; }

        /// <summary>Cookie所在域名
        /// </summary>
        public string Domain { get; set; }

        /// <summary>是否过期
        /// </summary>
        public bool Expired { get; set; }

        /// <summary>过期时间
        /// </summary>
        public DateTime Expires { get; set; }

        /// <summary>是否只允许服务器端生成
        /// </summary>
        public bool HttpOnly { get; set; }

        /// <summary>Cookie名
        /// </summary>
        public string Name { get; set; }

        /// <summary>Cookie Path
        /// </summary>
        public string Path { get; set; }

        /// <summary>Cookie端口号
        /// </summary>
        public string Port { get; set; }

        /// <summary>表明cookie只能通过安全通道发送
        /// </summary>
        public bool Secure { get; set; }

        /// <summary>Cookie创建时间
        /// </summary>
        public DateTime TimeStamp { get; set; }

        /// <summary>Cookie值
        /// </summary>
        public string Value { get; set; }

        /// <summary>Cookie 版本
        /// </summary>
        public int Version { get; set; }
    }
}
