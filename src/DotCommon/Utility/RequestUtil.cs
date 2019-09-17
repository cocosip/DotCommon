using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotCommon.Utility
{
    /// <summary>请求工具类
    /// </summary>
    public static class RequestUtil
    {
        /// <summary>User-Agents列表
        /// </summary>
        public static Dictionary<string, string> UserAgents
        {
            get
            {
                return GetUserAgentDictionary();
            }
        }


        #region 与HttpContext请求相关的其他数据

        /// <summary> 获取UserAgent集合
        /// </summary>
        private static Dictionary<string, string> GetUserAgentDictionary()
        {
            var dict = new Dictionary<string, string>
            {
                {
                    @"safari_mac",@"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_8; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50"
                },
                {
                    @"safari_windows",@"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_8; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50"
                },
                {
                    @"ie9",@"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)" },
                {
                    @"ie8", @"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)"},
                {
                    @"ie7", @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)"},
                {
                    @"ie6", @"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)"},
                {
                    @"firefox_mac", @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:2.0.1) Gecko/20100101 Firefox/4.0.1"},
                {
                    @"firefox_windows", @"Mozilla/5.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1"},
                {
                    @"opera_mac", @"Opera/9.80 (Macintosh; Intel Mac OS X 10.6.8; U; en) Presto/2.8.131 Version/11.11"},
                {
                    @"opera_windows", @"Opera/9.80 (Windows NT 6.1; U; en) Presto/2.8.131 Version/11.11"},
                {
                    @"chrome_mac",@"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11"
                },
                {
                    @"chrome_windows", @"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36"
                },
                {
                    @"maxthon", @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Maxthon 2.0)"},
                {
                    @"tencenttt", @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; TencentTraveler 4.0)"},
                {
                    @"theworld", @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1;The World)"},
                {
                    @"sougou",@"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; SE 2.X MetaSr 1.0; SE 2.X MetaSr 1.0; .NET CLR 2.0.50727; SE 2.X MetaSr 1.0)"
                },
                {
                    @"360", @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; 360SE)"
                },
                {
                    @"safari_iphone",@"Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5"
                },
                {
                    @"safari_ipod",@"Mozilla/5.0 (iPod; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5"
                },
                {
                    @"safari_ipad",
                    @"Mozilla/5.0 (iPad; U; CPU OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5"
                },
                {
                    @"android",@"Mozilla/5.0 (Linux; U; Android 2.3.7; en-us; Nexus One Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1"
                },
                {
                    @"androidqq",@"MQQBrowser/26 Mozilla/5.0 (Linux; U; Android 2.3.7; zh-cn; MB200 Build/GRJ22; CyanogenMod-7) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1"
                },
                {
                    @"opera_android",@"Opera/9.80 (Android 2.3.4; Linux; Opera Mobi/build-1107180945; U; en-GB) Presto/2.8.149 Version/11.10"
                },
                {
                    @"android_pad",@"Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/534.13 (KHTML, like Gecko) Version/4.0 Safari/534.13"
                },
                {
                    @"uc_iphone", @"IUC(U;iOS 5.1.1;Zh-cn;320*480;)/UCWEB8.9.1.271/42/800"
                },
                {
                    @"uc_android",@"Mozilla/5.0 (Linux; U; Android 4.0.3; zh-cn; M032 Build/IML74K) UC AppleWebKit/534.31 (KHTML, like Gecko) Mobile Safari/534.31"
                },
                {
                    @"navigator",@"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.12) Gecko/20080219 Firefox/2.0.0.12 Navigator/9.0.0.6"
                }
            };

            #region

            //PC 
            //Safari
            //IE
            //Firefox
            //Opera
            //Chrome
            //遨游Maxthon
            //腾讯TT
            //世界之窗
            //Sougou
            //360浏览器
            //移动端
            //iPod
            //iPad
            //Android
            //Android QQ浏览器
            //opera android版本
            //android pad

            //UC
            //UC安卓版

            #endregion

            return dict;
        }

        /// <summary>获取UserAgent的真实值
        /// </summary>
        public static string GetUserAgent(string agentKey)
        {
            var dict = GetUserAgentDictionary();
            string agent;
            dict.TryGetValue(agentKey.ToLower(), out agent);
            return agent;
        }

        #endregion


        /*****************请求相关的数据****************/

        #region 判断是否为手机用户

        /// <summary>判断是否为手机用户
        /// </summary>
        public static bool IsMobile(string userAgent)
        {
            var reg = new Regex(
                @"(iemobile|iphone|ipod|android|nokia|sonyericsson|blackberry|samsung|sec\-|windows ce|motorola|mot\-|up.b|midp\-)",
                RegexOptions.IgnoreCase | RegexOptions.Compiled);
            return reg.IsMatch(userAgent);
        }

        #endregion

        #region 获取手机的类型

        /// <summary>根据UserAgent获取平台类型
        /// </summary>
        public static string GetPlatform(string userAgent)
        {
            userAgent = userAgent.ToUpper();
            var agentFlag = "";
            string[] windowsKeys = { "Windows NT", "compatible", "MSIE", ".NET CLR" };
            string[] androidKeys = { "Android" };
            string[] iphoneKeys = { "iPhone", "iPad", "iPod" };
            string[] macKeys = { "Macintosh" };
            if (windowsKeys.Any(item => userAgent != null && userAgent.Contains(item.ToUpper())))
            {
                return MobilePlatform.Windows;
            }
            if (androidKeys.Any(item => userAgent != null && userAgent.Contains(item.ToUpper())))
            {
                return MobilePlatform.Android;
            }
            if (iphoneKeys.Any(item => userAgent != null && userAgent.Contains(item.ToUpper())))
            {
                return MobilePlatform.IPhone;
            }
            if (macKeys.Any(item => userAgent != null && userAgent.Contains(item.ToUpper())))
            {
                return MobilePlatform.MacBook;
            }
            return agentFlag;
        }

        /// <summary>是否在微信中
        /// </summary>
        public static bool IsWechatPlatform(string userAgent)
        {
            string[] weixinKeys = { "MicroMessenger" };
            if (weixinKeys.Any(item => userAgent != null && userAgent.ToUpper().Contains(item.ToUpper())))
            {
                return true;
            }
            return false;
        }
        #endregion

    }

    /// <summary>手机平台
    /// </summary>
    public class MobilePlatform
    {
        /// <summary>安卓
        /// </summary>
        public const string Android = "Android";

        /// <summary>IPhone
        /// </summary>
        public const string IPhone = "IPhone";

        /// <summary>MacBook
        /// </summary>
        public const string MacBook = "MacBook";

        /// <summary>Windows
        /// </summary>
        public const string Windows = "Windows";
    }

}
