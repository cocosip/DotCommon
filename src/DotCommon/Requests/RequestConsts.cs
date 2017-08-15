namespace DotCommon.Requests
{
    public class RequestConsts
    {
        /// <summary>长连接
        /// </summary>
        public const string KeepAlive = "Keep-alive";
        /// <summary>UserAgent
        /// </summary>
        public const string UserAgent = "User-Agent";
        /// <summary>请求时的Cookie
        /// </summary>
        public const string RequestCookie = "Cookie";
        /// <summary>返回的Cookie
        /// </summary>
        public const string ResponseCookie = "Set-Cookie";
        /// <summary>ContentType
        /// </summary>
        public const string ContentType = "Content-Type";
        /// <summary>Server
        /// </summary>
        public const string Server = "Server";

        /// <summary>
        /// </summary>
        public class AuthenticationSchema
        {
            /// <summary>基础认证
            /// </summary>
            public const string Basic = "Basic";

            /// <summary>摘要
            /// </summary>
            public const string Digest = "Digest";

            /// <summary>NTLM认证
            /// </summary>
            public const string Ntlm = "Ntlm";
            /// <summary>Bearer
            /// </summary>
            public const string Bearer = "Bearer";
        }

        public class ContentTypes
        {
            public const string ImageJpeg = "image/jpeg";
        }

        public class Methods
        {
            public const string Post = "POST";
            public const string Get = "GET";
        }
    }
}
