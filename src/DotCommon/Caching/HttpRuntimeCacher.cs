#if NET45
using System;
using System.Web;

namespace DotCommon.Caching
{
    /// <summary>系统自带缓存读写
    /// </summary>
    public class HttpRuntimeCacher : ICacher
    {
        /// <summary> 读取缓存
        /// </summary>
        public object Get(string key)
        {
            return HttpRuntime.Cache.Get(key);
        }

        /// <summary> 读取缓存
        /// </summary>
        public T Get<T>(string key)
        {
            object obj = HttpRuntime.Cache.Get(key);
            return (T)obj;
        }

        /// <summary>移除某个缓存
        /// </summary>
        public void Remove(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }

        /// <summary> 写入缓存
        /// </summary>
        public void Set(string key, object value)
        {
            HttpRuntime.Cache.Insert(key, value, null, DateTime.MaxValue, TimeSpan.Zero);
        }

        /// <summary>写入缓存
        /// </summary>
        public void Set(string key, object value, TimeSpan sliding)
        {
            HttpRuntime.Cache.Insert(key, value, null, DateTime.MaxValue, sliding);
        }

        /// <summary> 写入缓存
        /// </summary>
        public void Set(string key, object value, DateTime exp)
        {
            HttpRuntime.Cache.Insert(key, value, null, exp, TimeSpan.Zero);
        }
    }
}
#endif