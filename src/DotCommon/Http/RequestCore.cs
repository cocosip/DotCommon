using DotCommon.Extensions;
using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Text;

namespace DotCommon.Http
{
    /// <summary>核心的一些操作
    /// </summary>
    public class RequestCore
    {
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
            if (options.IsUseCert)
            {
                request.ClientCertificates.Add(options.ClientCert);
            }

            //Header
            foreach (var item in options.RequestHeaders)
            {
                request.Headers.Add(item.Key, item.Value);
            }
            //超时时间
            request.Timeout = options.TimeoutSecond * 1000;

            //是否为Https
            if (UrlUtil.IsHttps(options.Url))
            {
                if (options.SslValidationCallback != null)
                {

                    ServicePointManager.ServerCertificateValidationCallback = options.SslValidationCallback;
                }
                else
                {
                    ServicePointManager.ServerCertificateValidationCallback =
                    new RemoteCertificateValidationCallback((o, c, ch, e) => true);
                }
            }
            //认证
            if (!options.AuthorizationSchema.IsNullOrWhiteSpace() && !options.AuthorizationParameter.IsNullOrWhiteSpace())
            {
                request.Headers.Add(RequestConsts.Authorization, $"{options.AuthorizationSchema} {options.AuthorizationParameter}");
            }
            //代理
            if (options.Proxy != null)
            {
                request.Proxy = options.Proxy;
            }


            PrepareRequestData(request, options);
            return request;
        }


        /// <summary>创建请求的Url地址
        /// </summary>
        private static string BuildRequestUrl(RequestOptions options)
        {
            var url = options.Url;
            if (options.HttpMethod.ToUpper() == RequestConsts.Methods.Get)
            {
                //过滤与排序之后的集合
                var sParam = RequestUtil.FilterParams(options.RequestParameters);
                //参数组合
                var linkString = RequestUtil.CreateLinkString(sParam, options.IsUrlEncode, options.UrlHandler);
                var conbine = linkString.IsNullOrWhiteSpace() ? "" : "?";
                url = $"{url}{conbine}{linkString}";
            }
            return url;
        }

        /// <summary>创建ContentType
        /// </summary>
        private static string BuildContentType(RequestOptions options)
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
                        return "multipart/form-data; boundary=" + options.Boundary;
                    case PostType.Json:
                        return @"application/json";
                    case PostType.Xml:
                        return @"text/xml";
                }
            }
        }

        /// <summary>准备请求的数据
        /// </summary>
        private static void PrepareRequestData(HttpWebRequest request, RequestOptions options)
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
                            WriteRequestData(request, data);
                        }
                        break;
                    case PostType.Json:
                    case PostType.Xml:
                        {
                            var data = BuildStringPostData(options);
                            WriteRequestData(request, data);
                        }
                        break;
                    case PostType.Multipart:
                        {

                            var headerData = BuildMultipartHeaderData(options);
                            var footerData = BuildMultipartFooterData(options);
                            //长度,三部分之和
                            request.ContentLength = headerData.Length + footerData.Length + options.RequestFiles.Sum(s => s.Data.Length);
                            var requestStream = request.GetRequestStream();
                            //表单数据
                            requestStream.Write(headerData, 0, headerData.Length);
                            //文件内容 
                            foreach (var item in options.RequestFiles)
                            {
                                byte[] buffer = new byte[checked((uint)Math.Min(4096, (int)item.Data.Length))];
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
        private static byte[] BuildFormUrlEncodedData(RequestOptions options)
        {
            //过滤与排序之后的集合
            var sParam = RequestUtil.FilterParams(options.RequestParameters);
            //参数组合
            var linkString = RequestUtil.CreateLinkString(sParam, options.IsUrlEncode, options.UrlHandler);
            var data = Encoding.GetEncoding(options.Encode).GetBytes(linkString);
            return data;
        }

        /// <summary>创建application/json  application/xml 请求的数据
        /// </summary>
        private static byte[] BuildStringPostData(RequestOptions options)
        {
            return Encoding.GetEncoding(options.Encode).GetBytes(options.PostString);
        }

        /// <summary>创建 multipart/form-data Header部分数据
        /// </summary>
        private static byte[] BuildMultipartHeaderData(RequestOptions options)
        {
            StringBuilder sb = new StringBuilder();
            //上传的文件
            foreach (var requestFile in options.RequestFiles)
            {
                sb.Append("--" + options.Boundary);
                sb.Append("\r\n");
                sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"", requestFile.ParamName, requestFile.FileName);
                sb.Append("\r\n");
                sb.AppendFormat("Content-Type: {0}", "");
                sb.Append("\r\n\r\n");
            }
            foreach (KeyValuePair<string, string> temp in options.RequestParameters)
            {
                sb.Append("--" + options.Boundary);
                sb.Append("\r\n");
                sb.AppendFormat("Content-Disposition: form-data;name=\"{0}\"", temp.Key);
                sb.Append("\r\n\r\n");
                sb.Append(temp.Value);
                sb.Append("\r\n");
            }

            byte[] headerData = Encoding.GetEncoding(options.Encode).GetBytes(sb.ToString());
            return headerData;
        }

        /// <summary>创建 multipart/form-data Footer部分数据
        /// </summary>
        private static byte[] BuildMultipartFooterData(RequestOptions options)
        {
            return Encoding.GetEncoding(options.Encode).GetBytes("\r\n--" + options.Boundary + "--\r\n");
        }

        /// <summary>写入请求的数据
        /// </summary>
        private static void WriteRequestData(HttpWebRequest request, byte[] data)
        {
            request.ContentLength = data.Length;
            var requestStream = request.GetRequestStream();
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
        }


        /// <summary>Response返回结果
        /// </summary>
        public static Response ParseResponse(HttpWebRequest httpRequest, HttpWebResponse httpResponse)
        {
            var response = new Response
            {
                Success = true,
                ContentType = httpResponse.ContentType,
                StatusCode = (int)httpResponse.StatusCode,
                Cookies = httpRequest.CookieContainer,
                CookieString = httpRequest.CookieContainer?.GetCookieHeader(httpRequest.RequestUri),
                Server = httpResponse.Server,
                ResponseData = RequestUtil.ReadFromStream(httpResponse.GetResponseStream())
            };
            if (!httpResponse.ContentEncoding.IsNullOrWhiteSpace())
            {
                response.Encode = httpResponse.ContentEncoding;
            }
            return response;
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



    }
}
