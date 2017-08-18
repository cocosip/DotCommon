#if !NETSTANDARD2_0
using System;
using System.Web;

namespace DotCommon.Utility
{
    /// <summary>Cookie操作
    /// </summary>
    public class CookieUtil
    {

        #region 写入cookie

        /// <summary>写入cookie
        /// </summary>
        public static void WriteCookie(string key, string value, DateTime? exp = null)
        {
            var cookie = new HttpCookie(key, value)
            {
                Expires = exp ?? DateTime.MaxValue
            };

            HttpContext.Current.Response.Cookies.Set(cookie);

        }

        // 在关闭浏览器的时候,就自动清除
        public static void WriteAutoCookie(string key, string value)
        {
            var cookie = new HttpCookie(key, value);
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        /// <summary>
        /// 写入cookie
        /// </summary>
        /// <param name="key">cookie的key</param>
        /// <param name="value">cookie的值</param>
        /// <param name="exp">过期时间</param>
        /// <param name="domain">cookie的域</param>
        /// <param name="path">虚拟路径</param>
        /// <param name="urlEncode">是否以UrlEncode进行转码</param>
        // ReSharper disable once MethodOverloadWithOptionalParameter
        public static void WriteCookie(string key, string value, DateTime? exp = null, string domain = null, string path = "/", bool urlEncode = true)
        {
            if (urlEncode)
            {
                value = HttpUtility.UrlEncode(value);
            }
            var cookie = new HttpCookie(key, value)
            {
                Path = path,
                Expires = exp ?? DateTime.MaxValue

            };
            if (!string.IsNullOrEmpty(domain))
            {
                cookie.Domain = domain;
            }
            HttpContext.Current.Response.Cookies.Set(cookie);
        }

        #endregion

        #region 读取cookie
        /// <summary>读取cookie
        /// </summary>
        public static string Get(string key)
        {
            var cookieValue = string.Empty;
            if (HttpContext.Current.Request.Cookies[key] != null && HttpContext.Current.Request.Cookies[key].ToString() != "")
            {
                HttpCookie cookie = HttpContext.Current.Request.Cookies[key];
                cookieValue = cookie.Value;
            }
            return cookieValue;
        }
        #endregion

        #region 根据key删除某个Cookie的值
        /// <summary> 根据key删除某个Cookie的值
        /// </summary>
        public static void Remove(string key)
        {
            var cookie = HttpContext.Current.Request.Cookies[key];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
            }
        }
        #endregion

        #region 清除Cookie,将cookie的时间都变成过期

        /// <summary> 清除Cookie
        /// </summary>
        public static void ClearCookie()
        {
            int cookieCount = HttpContext.Current.Request.Cookies.Count;
            for (int i = 0; i < cookieCount; i++)
            {
                var httpCookie = HttpContext.Current.Request.Cookies[i];
                if (httpCookie != null)
                {
                    var cookieName = httpCookie.Name;
                    var cookie = new HttpCookie(cookieName)
                    {
                        Expires = DateTime.Now.AddDays(-1)
                    };
                    HttpContext.Current.Response.SetCookie(cookie);
                }
            }
        }

        #endregion
    }
}
#endif