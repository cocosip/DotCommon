using DotCommon.Extensions;
using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.IO;
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
            PrepareRequestData(request, options);
            return request;
        }

        /// <summary>创建请求的Url地址
        /// </summary>
        public static string BuildRequestUrl(RequestOptions options)
        {
            var url = options.Url;
            if (options.HttpMethod.ToUpper() == RequestConsts.Methods.Get)
            {
                //过滤与排序之后的集合
                var sParam = FilterParams(options.RequestParameters);
                //参数组合
                var linkString = CreateLinkString(sParam, options.IsUrlEncode, options.UrlHandler);
                var conbine = linkString.IsNullOrWhiteSpace() ? "" : "?";
                url = $"{url}{conbine}{linkString}";
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
                        {
                            var data = BuildFormUrlEncodedData(options);
                            request.ContentLength = data.Length;
                            var requestStream = request.GetRequestStream();
                            requestStream.Write(data, 0, data.Length);
                            requestStream.Close();
                        }
                        break;
                    case PostType.Json:
                    case PostType.Xml:
                        {
                            var data = BuildStringPostData(options);
                            request.ContentLength = data.Length;
                            var requestStream = request.GetRequestStream();
                            requestStream.Write(data, 0, data.Length);
                            requestStream.Close();
                        }
                        break;
                    case PostType.Multipart:
                        {
                            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
                            var headerData = BuildMultipartHeaderData(options, boundary);
                            var footerData = BuildMultipartFooterData(options, boundary);
                            //长度,三部分之和
                            request.ContentLength = headerData.Length + footerData.Length + options.RequestFiles.Sum(s => s.Data.Length);
                            var requestStream = request.GetRequestStream();
                            //表单数据
                            requestStream.Write(headerData, 0, headerData.Length);
                            //文件内容 
                            foreach (var item in options.RequestFiles)
                            {
                                byte[] buffer = new Byte[checked((uint)Math.Min(4096, (int)item.Data.Length))];
                                int bytesRead;
                                while ((bytesRead = item.Data.Read(buffer, 0, buffer.Length)) != 0)
                                {
                                    requestStream.Write(buffer, 0, bytesRead);
                                }
                            }
                            //结尾 
                            requestStream.Write(footerData, 0, footerData.Length);
                            requestStream.Close();
                        }
                        break;
                }
            }
        }

        /// <summary>创建application/x-www-form-urlencoded请求的数据
        /// </summary>
        public static byte[] BuildFormUrlEncodedData(RequestOptions options)
        {
            //过滤与排序之后的集合
            var sParam = FilterParams(options.RequestParameters);
            //参数组合
            var linkString = CreateLinkString(sParam, options.IsUrlEncode, options.UrlHandler);
            var data = Encoding.GetEncoding(options.Encode).GetBytes(linkString);
            return data;
        }

        /// <summary>创建application/json  application/xml 请求的数据
        /// </summary>
        public static byte[] BuildStringPostData(RequestOptions options)
        {
            return Encoding.GetEncoding(options.Encode).GetBytes(options.PostString);
        }

        /// <summary>创建 multipart/form-data Header部分数据
        /// </summary>
        public static byte[] BuildMultipartHeaderData(RequestOptions options, string boundary)
        {
            //表单数据
            var head = "";
            StringBuilder sb = new StringBuilder();
            //上传的文件
            foreach (var requestFile in options.RequestFiles)
            {
                sb.Append("--" + boundary);
                sb.Append("\r\n");
                sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", requestFile.ParamName, requestFile.FileName);
                sb.Append("\r\n");
                sb.AppendFormat("Content-Type: {0}", "");
                sb.Append("\r\n\r\n");
            }
            foreach (KeyValuePair<string, string> temp in options.RequestParameters)
            {
                sb.Append("--" + boundary);
                sb.Append("\r\n");
                sb.AppendFormat("Content-Disposition: form-data;name=\"{0}\"", temp.Key);
                sb.Append("\r\n\r\n");
                sb.Append(temp.Value);
                sb.Append("\r\n");
            }

            byte[] headerData = Encoding.GetEncoding(options.Encode).GetBytes(head);
            return headerData;
            //结尾
            // byte[] footData = Encoding.GetEncoding(options.Encode).GetBytes("\r\n--" + boundary + "--\r\n");
        }

        /// <summary>创建 multipart/form-data Footer部分数据
        /// </summary>
        public static byte[] BuildMultipartFooterData(RequestOptions options, string boundary)
        {
            return Encoding.GetEncoding(options.Encode).GetBytes("\r\n--" + boundary + "--\r\n");
        }




        /// <summary>Response返回结果
        /// </summary>
        public static Response ParseResponse(HttpWebRequest httpRequest, HttpWebResponse httpResponse)
        {
            try
            {
                var response = new Response
                {
                    Success = true,
                    ContentType = httpResponse.ContentType,
                    StatusCode = (int)httpResponse.StatusCode,
                    Cookies = httpRequest.CookieContainer,
                    CookieString = httpRequest.CookieContainer.GetCookieHeader(httpRequest.RequestUri),
                    Server = httpResponse.Headers.ToString(),
                    ResponseData = ReadFromStream(httpResponse.GetResponseStream())
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

        #region Utilities
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

        /// <summary> 从流中读取byte[]
        /// </summary>
        private static byte[] ReadFromStream(Stream stream)
        {
            // 初始化一个缓存区
            byte[] buffer = new byte[1024 * 4];
            int read = 0;
            int block;
            // 每次从流中读取缓存大小的数据，直到读取完所有的流为止
            while ((block = stream.Read(buffer, read, buffer.Length - read)) > 0)
            {
                // 重新设定读取位置
                read += block;
                // 检查是否到达了缓存的边界，检查是否还有可以读取的信息
                if (read == buffer.Length)
                {
                    // 尝试读取一个字节
                    int nextByte = stream.ReadByte();
                    // 读取失败则说明读取完成可以返回结果
                    if (nextByte == -1)
                    {
                        return buffer;
                    }
                    // 调整数组大小准备继续读取
                    byte[] newBuf = new byte[buffer.Length * 2];
                    Array.Copy(buffer, newBuf, buffer.Length);

                    newBuf[read] = (byte)nextByte;
                    // buffer是一个引用（指针），这里意在重新设定buffer指针指向一个更大的内存
                    buffer = newBuf;
                    read++;
                }
            }
            // 如果缓存太大则使用ret来收缩前面while读取的buffer，然后直接返回
            byte[] ret = new byte[read];
            Array.Copy(buffer, ret, read);
            return ret;
        }

        #endregion

    }
}
