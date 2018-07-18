using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace DotCommon.Http
{
    public class HttpResponse
    {
        private string content;

        public string ContentType { get; set; }

        public long ContentLength { get; set; }

        public string ContentEncoding { get; set; }

        public string Content => content ?? (content = RawBytes.AsString());


        public HttpStatusCode StatusCode { get; set; }

        public string StatusDescription { get; set; }

        public byte[] RawBytes { get; set; }

        public Uri ResponseUri { get; set; }

        public string Server { get; set; }

        public IList<HttpHeader> Headers { get; private set; }

        public IList<HttpCookie> Cookies { get; private set; }

        public ResponseStatus ResponseStatus { get; set; }

        public string ErrorMessage { get; set; }

        public Exception ErrorException { get; set; }

        public Version ProtocolVersion { get; set; }
    }
}
