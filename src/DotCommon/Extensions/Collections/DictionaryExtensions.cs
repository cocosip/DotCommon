using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace DotCommon.Extensions
{
    /// <summary>字典类型的扩展
    /// </summary>
    public static class DictionaryExtensions
    {
        internal static bool TryGetValue<T>(this IDictionary<string, object> dictionary, string key, out T value)
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



        public static void Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            TValue value;
            dict.TryRemove(key, out value);
        }

        /// <summary>将string,string类型字典转换成json
        /// </summary>
        public static string ToStringJson(this Dictionary<string, string> dictionary)
        {
            var sb = new StringBuilder();
            sb.Append($@"{{");
            foreach (var kv in dictionary.Where(kv => !string.IsNullOrWhiteSpace(kv.Key)))
            {
                sb.Append($@"""{kv.Key}""");
                sb.Append($@":");
                sb.Append($@"""{kv.Value}""");
                sb.Append($@",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append($"}}");
            return sb.ToString();
        }

        /// <summary>将string,object类型字典转换成json
        /// </summary>
        public static string ToJson(this Dictionary<string, object> dictionary)
        {
            var sb = new StringBuilder();
            sb.Append($@"{{");
            foreach (var kv in dictionary.Where(kv => !string.IsNullOrWhiteSpace(kv.Key)))
            {
                sb.Append($@"""{kv.Key}""");
                sb.Append($@":");
                sb.Append(kv.Value.GetType().GetTypeInfo().IsValueType ? $@"{kv.Value}" : $@"""{kv.Value}""");
                sb.Append($@",");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append($"}}");
            return sb.ToString();
        }

    }
}
