using System.Threading.Tasks;

namespace DotCommon.Http
{
    public interface IHttpClient
    {
        /// <summary>执行请求
        /// </summary>
        Task<Response> ExecuteAsync(RequestBuilder builder);
    }
}
