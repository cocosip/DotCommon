using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DotCommon.Http
{
    public partial class Http
    {
        /// <summary>GET请求
        /// </summary>
        public Task<Response> AsGet(string httpMethod) => GetStyleMethodInternal(httpMethod.ToUpperInvariant());

        /// <summary>POST请求
        /// </summary>
        public Task<Response> AsPost(string httpMethod) => PostPutInternal(httpMethod.ToUpperInvariant());

        private async Task<Response> GetStyleMethodInternal(string method)
        {
            var webRequest = ConfigureWebRequest(method, Url);

            if (HasBody && (method == "DELETE" || method == "OPTIONS"))
            {
                webRequest.ContentType = RequestContentType;
                WriteRequestBody(webRequest);
            }

            return await GetResponse(webRequest);
        }

        private async Task<Response> PostPutInternal(string method)
        {
            var webRequest = ConfigureWebRequest(method, Url);

            PreparePostBody(webRequest);

            WriteRequestBody(webRequest);
            return await GetResponse(webRequest);
        }

        /// <summary>写入请求的Body
        /// </summary>
        private void WriteRequestBody(HttpWebRequest webRequest)
        {
            if (HasBody || HasFiles || AlwaysMultipartFormData)
                webRequest.ContentLength = CalculateContentLength();

            using (var requestStream = webRequest.GetRequestStream())
            {
                if (HasFiles || AlwaysMultipartFormData)
                    WriteMultipartFormData(requestStream);
                else if (RequestBodyBytes != null)
                    requestStream.Write(RequestBodyBytes, 0, RequestBodyBytes.Length);
                else if (RequestBody != null)
                    WriteStringTo(requestStream, RequestBody);
            }
        }

        /// <summary>计算Content长度
        /// </summary>
        private long CalculateContentLength()
        {
            if (RequestBodyBytes != null)
                return RequestBodyBytes.Length;

            if (!HasFiles && !AlwaysMultipartFormData)
                return Encoding.GetByteCount(RequestBody);

            // calculate length for multipart form
            long length = 0;

            foreach (var file in Files)
            {
                length += Encoding.GetByteCount(GetMultipartFileHeader(file));
                length += file.ContentLength;
                length += Encoding.GetByteCount(LINE_BREAK);
            }

            length = Parameters.Aggregate(length,
                (current, param) => current + Encoding.GetByteCount(GetMultipartFormData(param)));

            length += Encoding.GetByteCount(GetMultipartFooter());

            return length;
        }

        private async Task<Response> GetResponse(HttpWebRequest request)
        {
            var response = new Response { ResponseStatus = ResponseStatus.None };

            try
            {
                var webResponse = await GetRawResponse(request);

                ExtractResponseData(response, webResponse);
            }
            catch (Exception ex)
            {
                ExtractErrorResponse(response, ex);
            }

            return response;
        }

        private static void ExtractErrorResponse(IResponse httpResponse, Exception ex)
        {
            if (ex is WebException webException && webException.Status == WebExceptionStatus.Timeout)
            {
                httpResponse.ResponseStatus = ResponseStatus.TimedOut;
                httpResponse.ErrorMessage = ex.Message;
                httpResponse.ErrorException = webException;
            }
            else
            {
                httpResponse.ErrorMessage = ex.Message;
                httpResponse.ErrorException = ex;
                httpResponse.ResponseStatus = ResponseStatus.Error;
            }
        }

        private async Task<HttpWebResponse> GetRawResponse(HttpWebRequest request)
        {
            try
            {
                var webResponse = await request.GetResponseAsync();

                return (HttpWebResponse)webResponse;
            }
            catch (WebException ex)
            {
                // Check to see if this is an HTTP error or a transport error.
                // In cases where an HTTP error occurs ( status code >= 400 )
                // return the underlying HTTP response, otherwise assume a
                // transport exception (ex: connection timeout) and
                // rethrow the exception

                if (ex.Response is HttpWebResponse response)
                    return response;

                throw;
            }
        }

        protected virtual HttpWebRequest ConfigureWebRequest(string method, Uri url)
        {
            var webRequest = CreateWebRequest(url);

            webRequest.UseDefaultCredentials = UseDefaultCredentials;

            webRequest.PreAuthenticate = PreAuthenticate;
            webRequest.Pipelined = Pipelined;
            webRequest.UnsafeAuthenticatedConnectionSharing = UnsafeAuthenticatedConnectionSharing;
#if NETSTANDARD2_0
            webRequest.Proxy = null;
#endif
            webRequest.ServicePoint.Expect100Continue = false;

            AppendHeaders(webRequest);
            AppendCookies(webRequest);

            if (Host != null) webRequest.Host = Host;

            webRequest.Method = method;

            // make sure Content-Length header is always sent since default is -1
            if (!HasFiles && !AlwaysMultipartFormData)
                webRequest.ContentLength = 0;

            if (Credentials != null)
                webRequest.Credentials = Credentials;

            if (UserAgent.HasValue())
                webRequest.UserAgent = UserAgent;

            if (ClientCertificates != null)
                webRequest.ClientCertificates.AddRange(ClientCertificates);


            foreach (var allowedDecompressionMethod in AllowedDecompressionMethods)
            {
                webRequest.AutomaticDecompression |= allowedDecompressionMethod;
            }

            if (AutomaticDecompression)
            {
                webRequest.AutomaticDecompression =
                    DecompressionMethods.Deflate | DecompressionMethods.GZip | DecompressionMethods.None;
            }

            if (Timeout != 0)
                webRequest.Timeout = Timeout;

            if (ReadWriteTimeout != 0)
                webRequest.ReadWriteTimeout = ReadWriteTimeout;

            webRequest.Proxy = Proxy;

            if (CachePolicy != null)
                webRequest.CachePolicy = CachePolicy;

            webRequest.AllowAutoRedirect = FollowRedirects;

            if (FollowRedirects && MaxRedirects.HasValue)
                webRequest.MaximumAutomaticRedirections = MaxRedirects.Value;

            webRequest.ServerCertificateValidationCallback = RemoteCertificateValidationCallback;

            webRequest.ConnectionGroupName = ConnectionGroupName;

            WebRequestConfigurator?.Invoke(webRequest);

            return webRequest;
        }
    }
}
