using DotCommon.Extensions;
using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DotCommon.Http
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

        /// <summary>创建请求的HttpWebRequst
        /// </summary>
        public static HttpWebRequest BuildWebRequest(RequestOptions options)
        {
            //获取请求Url,非GET请求为原地址
            var url = BuildRequestUrl(options);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            //请求方法
            request.Method = options.HttpMethod;
            //KeepAlive
            request.KeepAlive = options.KeepAlive;
            //自动重定向
            request.AllowAutoRedirect = options.AllowAutoRedirect;

            //非GET请求才需要设置ContentType
            if (options.HttpMethod.ToUpper() != RequestConsts.Methods.Get)
            {
                request.ContentType = BuildContentType(options);
            }
            //UserAgent
            if (!options.UserAgent.IsNullOrWhiteSpace())
            {
                request.UserAgent = options.UserAgent;
            }
            //Referer
            if (!options.Referer.IsNullOrWhiteSpace())
            {
                request.Referer = options.Referer;
            }
            //Accept
            if (!options.Accept.IsNullOrWhiteSpace())
            {
                request.Accept = options.Accept;
            }

            //Cookie
            if (options.Cookie != null && options.Cookie.Count > 0)
            {
                request.CookieContainer = options.Cookie;
            }
            //证书
            if (options.ClientCer != null)
            {
                request.ClientCertificates.Add(options.ClientCer);
            }
            //Https验证
            foreach (var item in options.RequestHeaders)
            {
                request.Headers.Add(item.Key, item.Value);
            }


            return request;
        }

        /// <summary>创建请求的Url地址
        /// </summary>
        public static string BuildRequestUrl(RequestOptions options)
        {
            var url = options.Url;
            if (options.HttpMethod.ToUpper() == RequestConsts.Methods.Get)
            {
                BuildGetUrl(options.Url, options.RequestParameters, options.IsUrlEncode, options.UrlHandler);
            }
            return url;
        }

        /// <summary>创建ContentType
        /// </summary>
        public static string BuildContentType(RequestOptions options)
        {
            //GET请求默认text/html;
            if (options.HttpMethod.ToUpper() == RequestConsts.Methods.Get)
            {
                return $"text/html;charset={Encoding.GetEncoding(options.Encode).WebName}";
            }
            else
            {
                switch (options.PostType)
                {
                    case PostType.FormUrlEncoded:
                    default:
                        return @"application/x-www-form-urlencoded";
                    case PostType.Multipart:
                        var boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                        return "multipart/form-data; boundary=" + boundary;
                    case PostType.Json:
                        return @"application/json";
                    case PostType.Xml:
                        return @"text/xml";
                }
            }
        }

        /// <summary>准备请求的数据
        /// </summary>
        public static void PrepareRequestData(HttpWebRequest request, RequestOptions options)
        {
            //非GET请求才会添加数据
            if (options.HttpMethod.ToUpper() != RequestConsts.Methods.Get)
            {
                switch (options.PostType)
                {
                    case PostType.FormUrlEncoded:
                    default:

                        break;
                }
            }
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
                    StatusCode = (int)message.StatusCode,
                    //Cookies = GetResponseCookie(message),
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
 
        /// <summary>获取Get请求的Url地址
        /// </summary>
        private static string BuildGetUrl(string url, Dictionary<string, string> paramTemp, bool isUrlEncode, Func<KeyValuePair<string, string>, string> urlHandler)
        {
            //过滤与排序之后的集合
            var sParam = FilterParams(paramTemp);
            //参数组合
            var linkString = CreateLinkString(sParam, isUrlEncode, urlHandler);
            var conbine = linkString.IsNullOrWhiteSpace() ? "" : "?";
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
        private static string CreateLinkString(SortedDictionary<string, string> paramTemp, bool isUrlEncode, Func<KeyValuePair<string, string>, string> urlHandler)
        {
            var sb = new StringBuilder();
            foreach (var kv in paramTemp)
            {
                if (urlHandler == null)
                {
                    //使用默认的UrlEncode
                    if (isUrlEncode)
                    {
                        sb.Append(kv.Key + "=" + WebUtility.UrlEncode(kv.Value) + "&");
                    }
                    else
                    {
                        sb.Append(kv.Key + "=" + kv.Value + "&");
                    }
                }
                else
                {
                    sb.Append(urlHandler.Invoke(kv));
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
