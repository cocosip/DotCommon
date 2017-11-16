using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

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

        /// <summary>过滤参数,去除无效的
        /// </summary>
        public static SortedDictionary<string, string> FilterParams(Dictionary<string, string> paramTemp)
        {
            //对请求的参数进行过滤,去除掉空字符和无效的
            var fParam = paramTemp.Where(temp => !string.IsNullOrWhiteSpace(temp.Value))
                .ToDictionary(temp => temp.Key, temp => temp.Value);
            return new SortedDictionary<string, string>(fParam);
        }

        /// <summary> 把数组所有元素，按照“参数=参数值”的模式用“&”字符拼接成字符串，并对参数值做urlencode
        /// </summary>
        public static string CreateLinkString(SortedDictionary<string, string> paramTemp, bool isUrlEncode, Func<KeyValuePair<string, string>, string> urlHandler)
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
                    sb.Append(urlHandler(kv));
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
        public static byte[] ReadFromStream(Stream stream)
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



    }
}
