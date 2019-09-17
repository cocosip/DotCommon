using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace DotCommon.Extensions
{
    /// <summary>字典类型的扩展
    /// </summary>
    public static class DictionaryExtensions
    {
        /// <summary>取值
        /// </summary>
        /// <typeparam name="T">值的泛型类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="key">Key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
        {
            object valueObj;
            if (dictionary.TryGetValue(key, out valueObj) && valueObj is T)
            {
                value = (T)valueObj;
                return true;
            }
            value = default(T);
            return false;
        }

        /// <summary>取值
        /// </summary>
        /// <typeparam name="TKey">Key的泛型类型</typeparam>
        /// <typeparam name="TValue">值的泛型类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="key">Key</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <returns></returns>
        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, bool ignoreCase = false)
        {
            if (ignoreCase && key.GetType() == typeof(string))
            {
                var kv = dictionary.FirstOrDefault(x => x.Key.ToString().ToUpper() == key.ToString());
                if (!kv.Key.ToString().IsNullOrEmpty())
                {
                    key = kv.Key;
                }
            }
            return dictionary.TryGetValue(key, out TValue o) ? o : default;
        }

        /// <summary>取值或者添加到字典
        /// </summary>
        /// <typeparam name="TKey">Key的泛型类型</typeparam>
        /// <typeparam name="TValue">值的泛型类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="key">Key</param>
        /// <param name="factory"><see cref="Func{TKey, TValue}" /></param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
        {
            if (dictionary.TryGetValue(key, out TValue obj))
            {
                return obj;
            }

            return dictionary[key] = factory(key);
        }

        /// <summary>取值或者添加到字典
        /// </summary>
        /// <typeparam name="TKey">Key的泛型类型</typeparam>
        /// <typeparam name="TValue">值的泛型类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="key">Key</param>
        /// <param name="factory"><see cref="Func{TValue}"/></param>
        /// <returns></returns>
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory)
        {
            return dictionary.GetOrAdd(key, k => factory());
        }

        /// <summary>取值或者添加到字典
        /// </summary>
        /// <typeparam name="TKey">Key的泛型类型</typeparam>
        /// <typeparam name="TValue">值的泛型类型</typeparam>
        /// <param name="dictionary">字典</param>
        /// <param name="key">Key</param>
        /// <returns></returns>
        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dictionary, TKey key)
        {
            return dictionary.TryRemove(key, out TValue value);
        }
    }
}
