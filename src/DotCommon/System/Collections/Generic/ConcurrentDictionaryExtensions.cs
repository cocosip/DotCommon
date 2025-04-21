using System.Collections.Concurrent;

namespace System.Collections.Generic
{
    /// <summary>
    /// Concurrent dictionary extensions
    /// </summary>
    public static class ConcurrentDictionaryExtensions
    {
        /// <summary>
        /// Remove by key
        /// </summary>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="dict"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Remove<TKey, TValue>(this ConcurrentDictionary<TKey, TValue> dict, TKey key)
        {
            return dict.TryRemove(key, out _);
        }
    }
}
