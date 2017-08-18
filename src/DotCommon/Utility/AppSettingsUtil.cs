#if !NETSTANDARD2_0
using System;
using System.Collections.Specialized;


namespace DotCommon.Utility
{
    /// <summary>AppSetting节点操作工具类
    /// </summary>
    public class AppSettingsUtil
    {
        private static readonly Lazy<NameValueCollection> AppSettings = new Lazy<NameValueCollection>(InitAppSettings);

        private static NameValueCollection InitAppSettings()
        {
            return System.Configuration.ConfigurationManager.AppSettings;
        }

        #region GetString
        /// <summary>获取配置文件中appSettings节点下指定索引键的字符串类型的的值
        /// </summary>
        public static string GetString(string key, string defaultValue = "")
        {
            return GetValue(key, false, defaultValue);
        }

        #endregion

        #region GetStringArray

        /// <summary>获取配置文件中appSettings节点下指定索引键的string[]类型的的值
        /// </summary>
        /// <param name="key">索引键</param>
        /// <param name="separator">分隔符</param>
        /// <param name="valueRequired">指定配置文件中是否必须需要配置有该名称的元素，传入False则方法返回默认值，反之抛出异常</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字符串数组</returns>
        public static string[] GetStringArray(string key, string separator, bool valueRequired = false, string[] defaultValue = default(string[]))
        {
            var value = GetValue(key, valueRequired);

            if (!string.IsNullOrEmpty(value))
            {
                return value.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            }
            if (!valueRequired)
            {
                return defaultValue;
            }
            throw new ApplicationException($"在配置文件的appSettings节点集合中找不到key为{key}的子节点，且没有指定默认值");
        }

        #endregion

        #region GetInt32

        /// <summary>获取配置文件中appSettings节点下指定索引键的Int类型的的值
        /// </summary>
        public static int GetInt32(string key, int defaultValue = 0)
        {
            return GetValue<int>(key, (v, pv) =>
            {
                if (pv <= 0) throw new ArgumentOutOfRangeException(nameof(pv));
                return int.TryParse(v, out pv);
            }, defaultValue);
        }

        #endregion

        #region GetBoolean

        /// <summary>获取配置文件中appSettings节点下指定索引键的布尔类型的的值
        /// </summary>
        public static bool GetBoolean(string key, bool defaultValue = false)
        {
            return GetValue<bool>(key, (v, pv) => bool.TryParse(v, out pv), defaultValue);
        }

        #endregion

        #region GetTimeSpan

        /// <summary> 获取配置文件中appSettings节点下指定索引键的时间间隔类型的的值
        /// </summary>

        public static TimeSpan GetTimeSpan(string key, TimeSpan defaultValue = default(TimeSpan))
        {
            var val = GetValue(key, false, null);
            return val == null ? defaultValue : TimeSpan.Parse(val);
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// 获取配置文件appSettings集合中指定索引的值
        /// </summary>
        /// <typeparam name="T">返回值类型参数</typeparam>
        /// <param name="key">索引键</param>
        /// <param name="parseValue">将指定索引键的值转化为返回类型的值的委托方法</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        private static T GetValue<T>(string key, Func<string, T, bool> parseValue, T? defaultValue) where T : struct
        {
            string value = AppSettings.Value[key];

            if (value != null)
            {
                T parsedValue = default(T);

                if (parseValue(value, parsedValue))
                {
                    return parsedValue;
                }
                throw new ApplicationException($"Setting '{key}' was not a valid {typeof(T).FullName}");
            }

            if (!defaultValue.HasValue)
            {
                throw new ApplicationException("在配置文件的appSettings节点集合中找不到key为" + key + "的子节点，且没有指定默认值");
            }
            return defaultValue.Value;
        }

        /// <summary>
        /// 获取配置文件appSettings集合中指定索引的值
        /// </summary>
        /// <param name="key">索引</param>
        /// <param name="valueRequired">指定配置文件中是否必须需要配置有该名称的元素，传入False则方法返回默认值，反之抛出异常</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>字符串</returns>
        private static string GetValue(string key, bool valueRequired, string defaultValue = "")
        {
            var value = AppSettings.Value[key];

            if (value != null)
            {
                return value;
            }
            if (!valueRequired)
            {
                return defaultValue;
            }
            throw new ApplicationException($"在配置文件的appSettings节点集合中找不到key为{key}的子节点");
        }

        #endregion
    }
}
#endif