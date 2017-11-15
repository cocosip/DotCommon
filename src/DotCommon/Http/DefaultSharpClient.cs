using System;
using System.Collections.Generic;
using System.Text;
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
                //var options = builder.GetOptions();
                //var message = RequestUtil.BuildRequestMessage(options);
                //var response = await _client.SendAsync(message);
                //return await RequestUtil.ParseResponse(response);
                throw new Exception("");
            }
            catch (AggregateException ex)
            {
                return RequestUtil.BuildErrorResponse(ex);
            }
            catch (Exception ex)
            {
                return RequestUtil.BuildErrorResponse(ex);
            }
        }

    }
}
