using System.Collections.Concurrent;
using System.Threading;

namespace DotCommon.Threading
{
    /// <summary>
    /// 环境数据上下文
    /// </summary>
    public class AsyncLocalAmbientDataContext : IAmbientDataContext
    {
        private static readonly ConcurrentDictionary<string, AsyncLocal<object>> AsyncLocalDictionary = new ConcurrentDictionary<string, AsyncLocal<object>>();

        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="key">Key值</param>
        /// <param name="value">值</param>
        public void SetData(string key, object value)
        {
            var asyncLocal = AsyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object>());
            asyncLocal.Value = value;
        }

        /// <summary>
        /// 取值
        /// </summary>
        /// <param name="key">Key值</param>
        /// <returns></returns>
        public object GetData(string key)
        {
            var asyncLocal = AsyncLocalDictionary.GetOrAdd(key, (k) => new AsyncLocal<object>());
            return asyncLocal.Value;
        }
    }
}
