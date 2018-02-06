using System;
using System.Net;
using System.Threading.Tasks;

namespace DotCommon.Http
{
    public class DefaultHttpClient : IHttpClient
    {
        /// <summary>执行请求
        /// </summary>
        public async Task<Response> ExecuteAsync(RequestBuilder builder)
        {
            try
            {
                var options = builder.GetOptions();
                var request = RequestCore.BuildWebRequest(options);
                var httpResponse = (HttpWebResponse)(await request.GetResponseAsync());
                var response = RequestCore.ParseResponse(request, httpResponse);
                httpResponse.Close();
                return response;
            }
            catch (WebException ex)
            {
                return RequestCore.BuildWebErrorResponse(ex);
            }
            catch (Exception ex)
            {
                return RequestCore.BuildErrorResponse(ex);
            }
        }
    }
}
