using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DotCommon.AspNetCore.Mvc.Extensions
{
    /// <summary>HttpRequestExtensions
    /// </summary>
    public static class HttpRequestExtensions
    {

        /// <summary>
        /// Retrieve the raw body as a string from the Request.Body stream
        /// </summary>
        /// <param name="request">Request instance to apply to</param>
        /// <param name="encoding">Optional - Encoding, defaults to UTF8</param>
        /// <returns></returns>
        public static async Task<string> GetRawBodyStringAsync(this HttpRequest request, Encoding? encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;

            using (StreamReader reader = new StreamReader(request.Body, encoding))
                return await reader.ReadToEndAsync();
        }

        /// <summary>
        /// Retrieves the raw body as a byte array from the Request.Body stream
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static async Task<byte[]> GetRawBodyBytesAsync(this HttpRequest request)
        {
            using (var ms = new MemoryStream(2048))
            {
                await request.Body.CopyToAsync(ms);
                return ms.ToArray();
            }
        }

        /// <summary>获取客户端的IP地址
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="isIPv6">是否为IPv6</param>
        /// <returns></returns>
        public static string GetRemoteIpAddress(this HttpRequest request, bool isIPv6 = false)
        {
            var remoteIpAddress = request.HttpContext.Connection.RemoteIpAddress;
            return isIPv6 ? remoteIpAddress?.MapToIPv6().ToString() ?? "" : remoteIpAddress?.MapToIPv4().ToString() ?? "";
        }

    }
}
