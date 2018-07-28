using System.Threading.Tasks;

namespace DotCommon.Http
{
    public static class HttpClientExtensions
    {
        /// <summary>发送GET请求
        /// </summary>
        public static async Task<IHttpResponse> GetAsync(this IHttpClient client, IHttpRequest request)
        {
            request.Method = Method.GET;
            return await client.ExecuteAsync(request);
        }

        /// <summary>发送POST请求
        /// </summary>
        public static async Task<IHttpResponse> PostAsync(this IHttpClient client, IHttpRequest request)
        {
            request.Method = Method.POST;
            return await client.ExecuteAsync(request);
        }
    }
}
