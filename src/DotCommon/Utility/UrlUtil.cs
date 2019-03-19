﻿using DotCommon.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace DotCommon.Utility
{
    /// <summary>Url操作类
    /// </summary>
    public class UrlUtil
    {

        /// <summary>是否为Https
        /// </summary>
        public static bool IsHttps(string url)
        {
            var uri = new Uri(url);
            return uri.Scheme.Equals("https", StringComparison.OrdinalIgnoreCase);
        }


        /// <summary>判断是否为主域名或者www开头的域名
        /// </summary>
        public static bool IsMainDomain(string url)
        {
            if (!url.IsNullOrEmpty())
            {
                url = url.ToLower();
                var reg = new Regex(
                    @"^http(s)?\://((www.)?[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|cn|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
                return reg.IsMatch(url);
            }
            return false;
        }

        /// <summary>将url转换成对应的协议模式下的url
        /// </summary>
        public static string PadLeftUrl(string url, string schema = "http://")
        {
            url = url.ToLower();
            if (!url.StartsWith("http://") && !url.StartsWith("https://"))
            {
                return string.Concat(schema, url);
            }
            return url;
        }

        /// <summary>获取服务器的域名系统 (DNS) 主机名或 IP 地址和端口号
        /// </summary>
        public static string GetAuthority(string url)
        {
            //baseUrl，?问号之前的url地址
            return new Uri(url).Authority;
        }

        /// <summary>合并Url,比如:http://www.baidu.com 与 /img/xxxx合并
        /// </summary>
        public static string CombineUrl(params string[] url)
        {
            var urlBuilder = new StringBuilder();
            for (int i = 0; i < url.Length; i++)
            {
                url[i] = url[i].Replace("../", "").Replace("~/", "");
                if (!url[i].EndsWith("/"))
                {
                    url[i] = $"{url[i]}/";
                }
                if (url[i].StartsWith("/"))
                {
                    url[i] = url[i].Remove(0, 1);
                }
                urlBuilder.Append(url[i]);
            }
            urlBuilder.Remove(urlBuilder.Length - 1, 1);
            return urlBuilder.ToString();
        }

        /// <summary>判断两个url地址是否为同域名
        /// </summary>
        public static bool SameDomain(string url1, string url2)
        {
            if (!string.IsNullOrWhiteSpace(url1) && !string.IsNullOrWhiteSpace(url2))
            {
                url1 = PadLeftUrl(url1);
                url2 = PadLeftUrl(url2);
                var uri1 = new Uri(url1);
                var uri2 = new Uri(url2);
                if (uri1.Authority.ToLower().Equals(uri2.Authority.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>获取url地址中的全部参数
        /// </summary>
        public static Dictionary<string, string> GetUrlParameters(string url)
        {
            var uri = new Uri(url);
            var paramArray = uri.Query.Replace("?", "").Split('&');
            return paramArray.Select(param => param.Split('='))
                .Where(itemArray =>
                    itemArray.Length == 2 && !string.IsNullOrWhiteSpace(itemArray[0]) &&
                    !string.IsNullOrWhiteSpace(itemArray[1]))
                .ToDictionary(itemArray => itemArray[0], itemArray => itemArray[1]);
        }

        /// <summary>获取除某些参数以外的参数集合
        /// </summary>
        public static Dictionary<string, string> GetExpectUrlParameters(string url, params string[] excepts)
        {
            var parameters = GetUrlParameters(url);
            var lowerExpects = excepts.Select(x => x.ToLower()).ToList();
            return parameters.Where(x => !lowerExpects.Contains(x.Key)).ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>将参数附加到url上
        /// </summary>
        public static string UrlAttachParameter(string url, string key, string value, bool replaceSame = false)
        {
            return UrlAttachParameters(url, new Dictionary<string, string>() { { key, value } }, replaceSame);
        }

        /// <summary>将参数附加到url上
        /// </summary>
        public static string UrlAttachParameters(string url, Dictionary<string, string> paramDict,
            bool replaceSame = false)
        {
            var parameters = GetUrlParameters(url);
            var sortParameters = new SortedDictionary<string, string>(parameters);
            foreach (var kv in paramDict)
            {
                if (!string.IsNullOrWhiteSpace(kv.Key) && !string.IsNullOrWhiteSpace(kv.Value))
                {
                    if (sortParameters.ContainsKey(kv.Key.ToLower()))
                    {
                        if (replaceSame)
                        {
                            sortParameters[kv.Key.ToLower()] = kv.Value;
                        }
                    }
                    else
                    {
                        sortParameters.Add(kv.Key, kv.Value);
                    }
                }
            }
            var uri = new Uri(url);
            var separator = sortParameters.Any() ? "?" : "";
            var parameterUri = string.Join("&", sortParameters.Select(x => string.Concat(x.Key, "=", x.Value)));
            UriBuilder uriBuilder = new UriBuilder()
            {
                Host = uri.Host,
                Scheme = uri.Scheme,
                Fragment = uri.Fragment,
                Path = uri.LocalPath,
                Query = string.Concat(separator, parameterUri),
            };

            return uriBuilder.Uri.ToString();
        }
    }
}
