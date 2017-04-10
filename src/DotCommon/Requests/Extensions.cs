using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace DotCommon.Requests
{
    public static class Extensions
    {
        /// <summary>获取Header一个值
        /// </summary>
        public static string GetHeader(this HttpResponseHeaders headers, string key)
        {
            IEnumerable<string> values;
            headers.TryGetValues(RequestConsts.ContentType, out values);
            return values?.FirstOrDefault() ?? "";
        }

        /// <summary>获取Header一个值
        /// </summary>
        public static string GetHeader(this HttpRequestHeaders headers, string key)
        {
            IEnumerable<string> values;
            headers.TryGetValues(RequestConsts.ContentType, out values);
            return values?.FirstOrDefault() ?? "";
        }
    }
}
