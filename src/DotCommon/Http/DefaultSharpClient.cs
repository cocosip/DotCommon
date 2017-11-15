using System;
using System.Net;
using System.Threading.Tasks;

namespace DotCommon.Http
{
    public class DefaultSharpClient : ISharpClient
    {
        public DefaultSharpClient()
        {
        }


        /// <summary>执行请求
        /// </summary>
        public async Task<Response> ExecuteAsync(RequestBuilder builder)
        {
            try
            {
                var options = builder.GetOptions();
                var request = RequestUtil.BuildWebRequest(options);
                var httpResponse = (HttpWebResponse)(await request.GetResponseAsync());
                var response = RequestUtil.ParseResponse(request, httpResponse);
                httpResponse.Close();
                return response;
            }
            catch (AggregateException ex)
            {
                return RequestUtil.BuildErrorResponse(ex);
            }
            catch (Exception ex)
            {
                return RequestUtil.BuildErrorResponse(ex);
            }
            finally
            {
            }
        }

    }
}
