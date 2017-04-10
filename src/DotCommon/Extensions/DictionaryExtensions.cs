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
