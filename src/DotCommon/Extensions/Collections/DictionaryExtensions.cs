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

        public static TValue GetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, bool ignoreCase = false)
        {
            TValue obj;
            if (ignoreCase && key.GetType() == typeof(string))
            {
                var kv = dictionary.FirstOrDefault(x => x.Key.ToString().ToUpper() == key.ToString());
                if (!kv.Key.ToString().IsNullOrEmpty())
                {
                    key = kv.Key;
                }
            }
            return dictionary.TryGetValue(key, out obj) ? obj : default(TValue);
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> factory)
        {
            TValue obj;
            if (dictionary.TryGetValue(key, out obj))
            {
                return obj;
            }

            return dictionary[key] = factory(key);
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> factory)
        {
            return dictionary.GetOrAdd(key, k => factory());
        }



        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            TValue value;
            return dict.TryRemove(key, out value);
        }
    }
}
