#if NET45
using System;

namespace DotCommon.Caching
{
    /// <summary>缓存操作接口
    /// </summary>
    public interface ICacher
    {
        /// <summary> 写入缓存
        /// </summary>
        void Set(string key, object value);

        /// <summary> 写入缓存
        /// </summary>
        void Set(string key, object value, DateTime exp);

        /// <summary> 写入缓存
        /// </summary>
        void Set(string key, object value, TimeSpan sliding);

        /// <summary>读取缓存
        /// </summary>
        object Get(string key);

        /// <summary> 读取缓存
        /// </summary>
        T Get<T>(string key);

        /// <summary>移除某个缓存
        /// </summary>
        void Remove(string key);

    }
}
#endif