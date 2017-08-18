using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
#if !NETSTANDARD2_0
using System.Reflection;
using System.Web;
#endif


namespace DotCommon.Utility
{
    /// <summary>请求工具类
    /// </summary>
    public class RequestUtil
    {
        private static readonly Lazy<Dictionary<string, string>> UserAgentsLazy =
            new Lazy<Dictionary<string, string>>(GetUserAgentDictionary);

        public static Dictionary<string, string> UserAgents => UserAgentsLazy.Value;


#if !NETSTANDARD2_0
        #region Get请求获取参数

        /// <summary> 获取某个请求参数的值,返回int类型
        /// </summary>
        public static int GetInt(string queryName, int defaultValue = 0)
        {
            string query = HttpContext.Current.Request.QueryString[queryName];
            if (string.IsNullOrEmpty(query))
            {
                return defaultValue;
            }
            return RegexUtil.IsInt32(query) ? int.Parse(query) : defaultValue;
        }

        /// <summary>获取某个Get请求参数的值,返回decimal类型
        /// </summary>
        public static decimal GetDecimal(string queryName, decimal defaultValue = 0M)
        {
            string query = HttpContext.Current.Request.QueryString[queryName];
            if (string.IsNullOrEmpty(query))
            {
                return defaultValue;
            }
            return RegexUtil.IsDecimal(query) ? decimal.Parse(query) : defaultValue;
        }

        /// <summary>获取某个Get请求参数的值,返回double类型
        /// </summary>
        public static double GetDouble(string queryName, double defaultValue = 0)
        {
            string query = HttpContext.Current.Request.QueryString[queryName];
            if (string.IsNullOrEmpty(query))
            {
                return defaultValue;
            }
            return RegexUtil.IsDouble(query) ? double.Parse(query) : defaultValue;
        }

        /// <summary> 根据请求的参数名获取参数值
        /// </summary>
        public static string GetString(string queryName)
        {
            string query = HttpContext.Current.Request.QueryString[queryName];
            if (query == null)
            {
                return string.Empty;
            }
            return StringUtil.SqlFilter(query.Trim());
        }

	    /// <summary>获取所有Get参数的列表
	    /// </summary>
	    public static Dictionary<string, string> GetAll()
	    {
		    var paraDict = new Dictionary<string, string>();
		    var collection = HttpContext.Current.Request.QueryString;
		    var keys = collection.AllKeys;
		    for (var i = 0; i < keys.Length; i++)
		    {
			    paraDict.Add(keys[i], collection[i]);
		    }
		    return paraDict;
	    }

	    /// <summary>根据Get请求传递的参数值,将请求的参数转换成对应的实体
        /// </summary>
        public static T GetToObject<T>() where T : new()
        {
            T model = new T();
            foreach (PropertyInfo property in model.GetType().GetProperties())
            {
                string temp = GetString(property.Name);
                if (!string.IsNullOrEmpty(temp))
                {
                    if (property.PropertyType.IsInterface)
                    {
                        continue;
                    }
                    property.SetValue(model,
                        property.PropertyType.IsEnum
                            ? Enum.ToObject(property.PropertyType, Convert.ToByte(temp))
                            : Convert.ChangeType(temp.Trim(), property.PropertyType), null);
                }
            }

            return model;
        }
        #endregion

        #region Post请求获取参数

        /// <summary>当以表单的形式提交的时候,按照参数名获取int类型的参数
        /// </summary>
        public static int PostInt(string queryName, int defaultValue = 0)
        {
            string query = HttpContext.Current.Request.Form[queryName];
            if (string.IsNullOrEmpty(query))
            {
                return defaultValue;
            }
            return RegexUtil.IsInt32(query) ? int.Parse(query) : defaultValue;
        }

        /// <summary> 当以表单的形式提交的时候,按照参数名获取decimal类型的参数
        /// </summary>
        public static decimal PostDecimal(string queryName, decimal defaultValue = 0M)
        {
            string query = HttpContext.Current.Request.Form[queryName];
            if (string.IsNullOrEmpty(query))
            {
                return defaultValue;
            }
            return RegexUtil.IsDecimal(query, 10) ? decimal.Parse(query) : defaultValue;
        }

        /// <summary>当以表单的形式提交的时候,按照参数名获取double类型的参数
        /// </summary>
        public static double PostDouble(string queryName, double defaultValue = 0)
        {
            string query = HttpContext.Current.Request.Form[queryName];
            if (string.IsNullOrEmpty(query))
            {
                return defaultValue;
            }
            return RegexUtil.IsDouble(query, 10) ? double.Parse(query) : defaultValue;
        }

        /// <summary>当以Post表单的形式提交的时候,按照参数名获取参数值
        /// </summary>
        public static string PostString(string queryName)
        {
            string query = HttpContext.Current.Request.Form[queryName];
            if (query == null)
            {
                return string.Empty;
            }
            return StringUtil.SqlFilter(query.Trim());
        }

        /// <summary>获取所有的Post请求的参数
        /// </summary>
        public static Dictionary<string, string> PostAll()
        {
			var paraDict = new Dictionary<string, string>();
			var collection = HttpContext.Current.Request.Form;
            var keys = collection.AllKeys;
	        for (var i = 0; i < keys.Length; i++)
	        {
		        paraDict.Add(keys[i], collection[i]);
	        }

	        return paraDict;
        }

        /// <summary>根据Post请求传递的参数值,将请求的参数转换成对应的实体
        /// </summary>
        public static T PostToObject<T>() where T : new()
        {
            T model = new T();
            foreach (var property in model.GetType().GetProperties())
            {
                var temp = HttpContext.Current.Request.Form[property.Name];
                if (!string.IsNullOrEmpty(temp))
                {
                    if (property.PropertyType.IsInterface)
                    {
                        continue;
                    }
                    property.SetValue(model,
                        property.PropertyType.IsEnum
                            ? Enum.ToObject(property.PropertyType, Convert.ToByte(temp))
                            : Convert.ChangeType(temp.Trim(), property.PropertyType), null);
                }
            }
            return model;
        }

        #endregion

        #region 判断当前的请求是否为Get或者Post
        /// <summary> 判断当前页面是否接收到了Post请求
        /// </summary>
        public static bool IsPost()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("POST");
        }

        /// <summary>判断当前页面是否接收到了Get请求
        /// </summary>
        public static bool IsGet()
        {
            return HttpContext.Current.Request.HttpMethod.Equals("GET");
        }
        #endregion

        #region 获取当前请求的原始 URL
        /// <summary> 获取当前请求的原始 URL(URL 中域信息之后的部分,包括查询字符串(如果存在))
        /// </summary>
        public static string GetRawUrl()
        {
            return HttpContext.Current.Request.RawUrl;
        }
        #endregion

        #region 获取当前请求地址的Referer
        /// <summary> 获取当前请求地址的Referer
        /// </summary>
        public static Uri GetReferer()
        {
            var uri = HttpContext.Current.Request.UrlReferrer;
            return uri;
        }
        #endregion

        #region 获得当前完整Url地址
        /// <summary>获得当前完整Url地址
        /// </summary>
        public static string GetUrl()
        {
            return HttpContext.Current.Request.Url.ToString();
        }
        #endregion

        #region 获取当前请求的虚拟路径
        /// <summary>获取当前请求的虚拟路径
        /// </summary>
        public static string GetParthUrl()
        {
            return HttpContext.Current.Request.Path;
        }
        #endregion

        #region 获取获取DNS主机名或者IP地址和端口号

        /// <summary> 获取获取DNS主机名或者IP地址和端口号
        /// </summary>
        public static string GetAuthority()
        {
            return HttpContext.Current.Request.Url.Authority;
        }
        #endregion

        #region 获得当前页面客户端的IP
        /// <summary>获得当前页面客户端的IP
        /// </summary>
        public static string GetIp()
        {
            var result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (string.IsNullOrEmpty(result))
            {
                result = HttpContext.Current.Request.UserHostAddress;
            }
            if (string.IsNullOrEmpty(result) || !NetUtil.IsIp(result))
            {
                return "127.0.0.1";
            }

            return result;
        }
        #endregion

        #region 获取手机的类型
        /// <summary>根据UserAgent获取手机的类型
        /// </summary>
        public static string GetAgent()
        {
            var agentFlag = "";
            var context = HttpContext.Current;
            string[] weixinKeys = { "MicroMessenger" };
            string[] windowsKeys = { "Windows NT", "compatible;", "MSIE", ".NET CLR" };
            string[] androidKeys = { "Android" };
            string[] iphoneKeys = { "iPhone", "iPad", "iPod" };
            string[] macKeys = { "Macintosh" };
            if (context != null)
            {
                var agent = context.Request.UserAgent;
                if (weixinKeys.Any(item => agent != null && agent.Contains(item)))
                {
                    return "weixin";
                }
                if (windowsKeys.Any(item => agent != null && agent.Contains(item)))
                {
                    return "windows";
                }
                if (androidKeys.Any(item => agent != null && agent.Contains(item)))
                {
                    return "android";
                }
                if (iphoneKeys.Any(item => agent != null && agent.Contains(item)))
                {
                    return "iphone";
                }
                if (macKeys.Any(item => agent != null && agent.Contains(item)))
                {
                    return "macbook";
                }
            }

            return agentFlag;
        }
        #endregion

#endif


        #region 与HttpContext请求相关的其他数据

        /// <summary> 获取UserAgent集合
        /// </summary>
        private static Dictionary<string, string> GetUserAgentDictionary()
        {
            var dict = new Dictionary<string, string>
            {
                {
                    @"safari_mac",
                    @"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_8; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50"
                },
                {
                    @"safari_windows",
                    @"Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_8; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50"
                },
                {@"ie9", @"Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)"},
                {@"ie8", @"Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)"},
                {@"ie7", @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)"},
                {@"ie6", @"Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)"},
                {@"firefox_mac", @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:2.0.1) Gecko/20100101 Firefox/4.0.1"},
                {@"firefox_windows", @"Mozilla/5.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1"},
                {@"opera_mac", @"Opera/9.80 (Macintosh; Intel Mac OS X 10.6.8; U; en) Presto/2.8.131 Version/11.11"},
                {@"opera_windows", @"Opera/9.80 (Windows NT 6.1; U; en) Presto/2.8.131 Version/11.11"},
                {
                    @"chrome_mac",
                    @"Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11"
                },
                {
                    @"chrome_windows",
                    @"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36"
                },
                {@"maxthon", @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Maxthon 2.0)"},
                {@"tencenttt", @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; TencentTraveler 4.0)"},
                {@"theworld", @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1;The World)"},
                {
                    @"sougou",
                    @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; SE 2.X MetaSr 1.0; SE 2.X MetaSr 1.0; .NET CLR 2.0.50727; SE 2.X MetaSr 1.0)"
                },
                {@"360", @"Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; 360SE)"},
                {
                    @"safari_iphone",
                    @"Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5"
                },
                {
                    @"safari_ipod",
                    @"Mozilla/5.0 (iPod; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5"
                },
                {
                    @"safari_ipad",
                    @"Mozilla/5.0 (iPad; U; CPU OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5"
                },
                {
                    @"android",
                    @"Mozilla/5.0 (Linux; U; Android 2.3.7; en-us; Nexus One Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1"
                },
                {
                    @"androidqq",
                    @"MQQBrowser/26 Mozilla/5.0 (Linux; U; Android 2.3.7; zh-cn; MB200 Build/GRJ22; CyanogenMod-7) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1"
                },
                {
                    @"opera_android",
                    @"Opera/9.80 (Android 2.3.4; Linux; Opera Mobi/build-1107180945; U; en-GB) Presto/2.8.149 Version/11.10"
                },
                {
                    @"android_pad",
                    @"Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/534.13 (KHTML, like Gecko) Version/4.0 Safari/534.13"
                },
                {@"uc_iphone", @"IUC(U;iOS 5.1.1;Zh-cn;320*480;)/UCWEB8.9.1.271/42/800"},
                {
                    @"uc_android",
                    @"Mozilla/5.0 (Linux; U; Android 4.0.3; zh-cn; M032 Build/IML74K) UC AppleWebKit/534.31 (KHTML, like Gecko) Mobile Safari/534.31"
                },
                {
                    @"navigator",
                    @"Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.12) Gecko/20080219 Firefox/2.0.0.12 Navigator/9.0.0.6"
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

        #region 检测是否是正确的Url

        /// <summary>检测是否是正确的Url
        /// </summary>
        public static bool IsUrl(string strUrl)
        {
            if (!string.IsNullOrEmpty(strUrl))
            {
                strUrl = strUrl.ToLower();
                return Regex.IsMatch(strUrl,
                    @"^http(s)?\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
            }
            return false;
        }

        #endregion

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

        /// <summary>根据UserAgent获取手机的类型
        /// </summary>
        public static string GetMobileType(string userAgent)
        {
            var agentFlag = "";
            string[] weixinKeys = { "MicroMessenger" };
            string[] windowsKeys = { "Windows NT", "compatible;", "MSIE", ".NET CLR" };
            string[] androidKeys = { "Android" };
            string[] iphoneKeys = { "iPhone", "iPad", "iPod" };
            string[] macKeys = { "Macintosh" };
            if (weixinKeys.Any(item => userAgent != null && userAgent.Contains(item)))
            {
                return "weixin";
            }
            if (windowsKeys.Any(item => userAgent != null && userAgent.Contains(item)))
            {
                return "windows";
            }
            if (androidKeys.Any(item => userAgent != null && userAgent.Contains(item)))
            {
                return "android";
            }
            if (iphoneKeys.Any(item => userAgent != null && userAgent.Contains(item)))
            {
                return "iphone";
            }
            if (macKeys.Any(item => userAgent != null && userAgent.Contains(item)))
            {
                return "macbook";
            }
            return agentFlag;
        }

        #endregion

    }
}
