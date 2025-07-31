using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotCommon.Utility
{
    /// <summary>
    /// A utility class for handling HTTP request-related information, such as User-Agent parsing.
    /// </summary>
    public static class RequestUtil
    {
        private static readonly Lazy<Dictionary<string, string>> UserAgentsLazy = new Lazy<Dictionary<string, string>>(GetUserAgentDictionary);

        private static readonly Regex MobileRegex = new Regex(
            @"(iemobile|iphone|ipod|android|nokia|sonyericsson|blackberry|samsung|sec\-|windows ce|motorola|mot\-|up.b|midp\-)",
            RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// Gets a dictionary of common User-Agent strings, keyed by a friendly name.
        /// </summary>
        public static IReadOnlyDictionary<string, string> UserAgents => UserAgentsLazy.Value;

        /// <summary>
        /// Retrieves a collection of common User-Agent strings.
        /// </summary>
        private static Dictionary<string, string> GetUserAgentDictionary()
        {
            return new Dictionary<string, string>
            {
                { "safari_mac", "Mozilla/5.0 (Macintosh; U; Intel Mac OS X 10_6_8; en-us) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50" },
                { "safari_windows", "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US) AppleWebKit/534.50 (KHTML, like Gecko) Version/5.1 Safari/534.50" },
                { "ie9", "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)" },
                { "ie8", "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0; Trident/4.0)" },
                { "ie7", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.0)" },
                { "ie6", "Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.1)" },
                { "firefox_mac", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.6; rv:2.0.1) Gecko/20100101 Firefox/4.0.1" },
                { "firefox_windows", "Mozilla/5.0 (Windows NT 6.1; rv:2.0.1) Gecko/20100101 Firefox/4.0.1" },
                { "opera_mac", "Opera/9.80 (Macintosh; Intel Mac OS X 10.6.8; U; en) Presto/2.8.131 Version/11.11" },
                { "opera_windows", "Opera/9.80 (Windows NT 6.1; U; en) Presto/2.8.131 Version/11.11" },
                { "chrome_mac", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_7_0) AppleWebKit/535.11 (KHTML, like Gecko) Chrome/17.0.963.56 Safari/535.11" },
                { "chrome_windows", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/46.0.2490.80 Safari/537.36" },
                { "maxthon", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Maxthon 2.0)" },
                { "tencenttt", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; TencentTraveler 4.0)" },
                { "theworld", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1;The World)" },
                { "sougou", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; Trident/4.0; SE 2.X MetaSr 1.0; SE 2.X MetaSr 1.0; .NET CLR 2.0.50727; SE 2.X MetaSr 1.0)" },
                { "360", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; 360SE)" },
                { "safari_iphone", "Mozilla/5.0 (iPhone; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5" },
                { "safari_ipod", "Mozilla/5.0 (iPod; U; CPU iPhone OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5" },
                { "safari_ipad", "Mozilla/5.0 (iPad; U; CPU OS 4_3_3 like Mac OS X; en-us) AppleWebKit/533.17.9 (KHTML, like Gecko) Version/5.0.2 Mobile/8J2 Safari/6533.18.5" },
                { "android", "Mozilla/5.0 (Linux; U; Android 2.3.7; en-us; Nexus One Build/FRF91) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1" },
                { "androidqq", "MQQBrowser/26 Mozilla/5.0 (Linux; U; Android 2.3.7; zh-cn; MB200 Build/GRJ22; CyanogenMod-7) AppleWebKit/533.1 (KHTML, like Gecko) Version/4.0 Mobile Safari/533.1" },
                { "opera_android", "Opera/9.80 (Android 2.3.4; Linux; Opera Mobi/build-1107180945; U; en-GB) Presto/2.8.149 Version/11.10" },
                { "android_pad", "Mozilla/5.0 (Linux; U; Android 3.0; en-us; Xoom Build/HRI39) AppleWebKit/534.13 (KHTML, like Gecko) Version/4.0 Safari/534.13" },
                { "uc_iphone", "IUC(U;iOS 5.1.1;Zh-cn;320*480;)/UCWEB8.9.1.271/42/800" },
                { "uc_android", "Mozilla/5.0 (Linux; U; Android 4.0.3; zh-cn; M032 Build/IML74K) UC AppleWebKit/534.31 (KHTML, like Gecko) Mobile Safari/534.31" },
                { "navigator", "Mozilla/5.0 (Windows; U; Windows NT 5.1; en-US; rv:1.8.1.12) Gecko/20080219 Firefox/2.0.0.12 Navigator/9.0.0.6" }
            };
        }

        /// <summary>
        /// Gets a specific User-Agent string by its key.
        /// </summary>
        /// <param name="agentKey">The key for the User-Agent string (e.g., "chrome_windows").</param>
        /// <returns>The User-Agent string if found; otherwise, null.</returns>
        public static string? GetUserAgent(string agentKey)
        {
            if (string.IsNullOrEmpty(agentKey))
                return null;

            return UserAgents.TryGetValue(agentKey.ToLower(), out var agent) ? agent : null;
        }

        /// <summary>
        /// Determines if a User-Agent string belongs to a mobile device.
        /// </summary>
        /// <param name="userAgent">The User-Agent string to check.</param>
        /// <returns>True if the User-Agent is identified as a mobile device; otherwise, false.</returns>
        public static bool IsMobile(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return false;

            return MobileRegex.IsMatch(userAgent);
        }

        /// <summary>
        /// Gets the platform name (e.g., Windows, Android) from a User-Agent string.
        /// </summary>
        /// <param name="userAgent">The User-Agent string to analyze.</param>
        /// <returns>The identified platform name or an empty string if not determined.</returns>
        public static string GetPlatform(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return string.Empty;

            if (userAgent.Contains("Windows NT") || userAgent.Contains("MSIE") || userAgent.Contains(".NET CLR"))
                return MobilePlatform.Windows;
            if (userAgent.Contains("Android"))
                return MobilePlatform.Android;
            if (userAgent.Contains("iPhone") || userAgent.Contains("iPad") || userAgent.Contains("iPod"))
                return MobilePlatform.IPhone;
            if (userAgent.Contains("Macintosh"))
                return MobilePlatform.MacBook;

            return string.Empty;
        }

        /// <summary>
        /// Determines if a User-Agent string is from the WeChat built-in browser.
        /// </summary>
        /// <param name="userAgent">The User-Agent string to check.</param>
        /// <returns>True if the User-Agent is from WeChat; otherwise, false.</returns>
        public static bool IsWechatPlatform(string userAgent)
        {
            if (string.IsNullOrEmpty(userAgent))
                return false;

            return userAgent.IndexOf("MicroMessenger", StringComparison.OrdinalIgnoreCase) >= 0;
        }
    }

    /// <summary>
    /// Defines constants for common mobile platform names.
    /// </summary>
    public static class MobilePlatform
    {
        /// <summary>
        /// Android platform.
        /// </summary>
        public const string Android = "Android";

        /// <summary>
        /// iPhone platform (includes iPad and iPod).
        /// </summary>
        public const string IPhone = "IPhone";

        /// <summary>
        /// MacBook platform.
        /// </summary>
        public const string MacBook = "MacBook";

        /// <summary>
        /// Windows platform.
        /// </summary>
        public const string Windows = "Windows";
    }
}
