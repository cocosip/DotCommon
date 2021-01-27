using System.Collections.Concurrent;

namespace DotCommon.Extensions
{
    public static class ConcurrentDictionaryExtensions
    {
        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.TryRemove(key, out _);
        }
    }
}
