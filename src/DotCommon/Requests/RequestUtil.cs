using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DotCommon.Requests
{
    public class RequestUtil
    {
        /// <summary>根据方法名称获取方法枚举
        /// </summary>
        public static HttpMethod GetHttpMethod(string @method)
        {
            switch (@method.ToUpper())
            {
                case "POST":
                    return HttpMethod.Post;
                case "DELETE":
                    return HttpMethod.Delete;
                case "PUT":
                    return HttpMethod.Put;
                case "TRACE":
                    return HttpMethod.Trace;
                case "GET":
                default:
                    return HttpMethod.Get;
            }
        }

        /// <summary>创建消息
        /// </summary>
        public static HttpRequestMessage BuildRequestMessage(RequestOptions options)
        {
            var httpMethod = GetHttpMethod(options.HttpMethod);
            HttpRequestMessage message;
            //判断是否为Get请求
            if (httpMethod == HttpMethod.Get)
            {
                var url = BuildGetUrl(options.Url, options.RequestParameters.Value, options.IsUrlEncode);
                message = new HttpRequestMessage(httpMethod, url);
            }
            else
            {
                message = new HttpRequestMessage(httpMethod, options.Url);
            }

            //设置认证
            if (!string.IsNullOrWhiteSpace(options.AuthorizationParameter))
            {
                message.Headers.Authorization = new AuthenticationHeaderValue(options.AuthorizationSchema,
                    options.AuthorizationParameter);
            }

            //设置Referer
            if (!string.IsNullOrWhiteSpace(options.Referer))
            {
                message.Headers.Referrer = new Uri(options.Referer);
            }
            //设置UserAgent
            if (!string.IsNullOrWhiteSpace(options.UserAgent))
            {
                message.Headers.Add(RequestConsts.UserAgent, options.UserAgent);
            }
            //添加header
            foreach (var header in options.RequestHeaders.Value)
            {
                message.Headers.Add(header.Key, header.Value);
            }
            //KeepAlive
            if (options.KeepAlive)
            {
                message.Headers.Connection.Add(RequestConsts.KeepAlive);
            }
            //设置Range
            if (options.RangeFrom > 0)
            {
                message.Headers.Range = new RangeHeaderValue(options.RangeFrom, options.RangeTo);
            }
            //CacheContorl NoCache
            message.Headers.CacheControl = new CacheControlHeaderValue();
            //CacheControl NoStore
            if (options.CacheControlNostore)
            {
                message.Headers.CacheControl.NoStore = options.CacheControlNostore;
            }
            //请求报头域用于指定客户端接受哪些类型的信息
            if (!string.IsNullOrWhiteSpace(options.Accept))
            {
                message.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(options.Accept));
            }

            //设置content
            if (httpMethod != HttpMethod.Get)
            {
                message.Content = BuildContent(options);
            }
            return message;
        }


        /// <summary>创建请求的Content
        /// </summary>
        public static HttpContent BuildContent(RequestOptions options)
        {
            switch (options.PostType)
            {
                case PostType.Multipart:
                    var boundary = "--------" + DateTime.Now.Ticks.ToString("x");
                    var content = new MultipartFormDataContent(boundary);
                    foreach (var file in options.RequestFiles.Value)
                    {
                        content.Add(CreateStreamContent(file), file.ParamName, file.FileName);
                    }
                    return content;
                case PostType.Json:
                case PostType.Xml:
                    return new StringContent(options.PostString, Encoding.GetEncoding(options.Encode));
                case PostType.FormUrlEncoded:
                default:
                    return new FormUrlEncodedContent(options.RequestParameters.Value);
            }
        }

        /// <summary>生成文件的StremContent
        /// </summary>
        private static StreamContent CreateStreamContent(RequestFile file)
        {
            var fileContent = new StreamContent(file.Data);
            fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue(file.ParamName)
            {
                FileName = file.FileName
            };
            //扩展名
            var extension = file.FileName.Substring(file.FileName.LastIndexOf(".", StringComparison.Ordinal));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue(MimeTypeNameUtil.GetMimeName(extension));
            return fileContent;
        }

        /// <summary>Response返回结果
        /// </summary>
        public static async Task<Response> ParseResponse(HttpResponseMessage message)
        {
            try
            {
                var response = new Response
                {
                    Success = message.IsSuccessStatusCode,
                    ContentType = message.Content.Headers.ContentType.ToString(),
                    StatusCode = (int) message.StatusCode,
                    Cookies = GetResponseCookie(message),
                    Server = message.Headers.Server.ToString(),
                    ResponseData = await message.Content.ReadAsByteArrayAsync()
                };
                return response;
            }
            catch (AggregateException ex)
            {
                return BuildErrorResponse(ex);
            }
            catch (Exception ex)
            {
                return BuildErrorResponse(ex);
            }
        }

        /// <summary>创建错误的Response
        /// </summary>
        internal static Response BuildErrorResponse(Exception e)
        {
            var response = new Response
            {
                Success = false,
                ExceptionMessage = e.Message
            };
            return response;
        }

        /// <summary>获取响应Cookie
        /// </summary>
        private static IEnumerable<string> GetResponseCookie(HttpResponseMessage message)
        {
            IEnumerable<string> cookies;
            message.Headers.TryGetValues(RequestConsts.ResponseCookie, out cookies);
            return cookies ?? new List<string>();
        }

        /// <summary>获取Get请求的Url地址
        /// </summary>
        private static string BuildGetUrl(string url, Dictionary<string, string> paramTemp, bool isUrlEncode = false)
        {
            //过滤与排序之后的集合
            var sParam = FilterParams(paramTemp);
            //参数组合
            var linkString = CreateLinkString(sParam, isUrlEncode);
            var conbine = string.IsNullOrWhiteSpace(linkString) ? "" : "?";
            return $"{url}{conbine}{linkString}";
        }

        /// <summary>过滤参数,去除无效的
        /// </summary>
        private static SortedDictionary<string, string> FilterParams(Dictionary<string, string> paramTemp)
        {
            //对请求的参数进行过滤,去除掉空字符和无效的
            var fParam = paramTemp.Where(temp => !string.IsNullOrWhiteSpace(temp.Value))
                .ToDictionary(temp => temp.Key, temp => temp.Value);
            return new SortedDictionary<string, string>(fParam);
        }

        /// <summary> 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        private static string CreateLinkString(SortedDictionary<string, string> paramTemp, bool isUrlEncode = false)
        {
            var sb = new StringBuilder();
            foreach (var temp in paramTemp)
            {
                if (isUrlEncode)
                {
                    sb.Append(temp.Key + "=" + temp.Value + "&");
                }
                else
                {
                    sb.Append(temp.Key + "=" + WebUtility.UrlEncode(temp.Value) + "&");
                }
            }
            //去掉最後一個&字符
            if (sb.Length > 0)
            {
                sb.Remove(sb.Length - 1, 1);
            }
            return sb.ToString();
        }

    }
}



