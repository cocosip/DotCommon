using System;
using System.Threading.Tasks;

namespace DotCommon.Http
{
    public partial class HttpClient
    {

        /// <summary>执行请求
        /// </summary>
        public async Task<IHttpResponse> ExecuteAsync(IHttpRequest request)
        {
            var method = Enum.GetName(typeof(Method), request.Method);

            switch (request.Method)
            {
                case Method.COPY:
                case Method.POST:
                case Method.PUT:
                case Method.PATCH:
                case Method.MERGE:
                    return await ExecuteAsync(request, method, DoExecuteAsPost);

                default:
                    return await ExecuteAsync(request, method, DoExecuteAsGet);
            }
        }

        /// <summary>下载数据
        /// </summary>
        public Task<byte[]> DownloadData(IHttpRequest request) => DownloadData(request, false);

        /// <summary>下载数据
        /// </summary>
        public async Task<byte[]> DownloadData(IHttpRequest request, bool throwOnError)
        {
            var response = await ExecuteAsync(request);
            if (response.ResponseStatus == ResponseStatus.Error && throwOnError)
            {
                throw response.ErrorException;
            }

            return response.RawBytes;
        }


        private async Task<Response> DoExecuteAsGet(IHttp http, string method)
        {
            return await http.AsGet(method);
        }

        private async Task<Response> DoExecuteAsPost(IHttp http, string method)
        {
            return await http.AsPost(method);
        }
    }
}
