using System.Threading.Tasks;

namespace DotCommon.Requests
{
    public interface IRequestClient
    {
        /// <summary>异步发送请求
        /// </summary>
        Task<Response> SendAsync(RequestBuilder builder);
    }
}
