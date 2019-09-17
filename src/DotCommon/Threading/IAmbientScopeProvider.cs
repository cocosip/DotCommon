using System;

namespace DotCommon.Threading
{
    /// <summary>数据槽上下文
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IAmbientScopeProvider<T>
    {
        /// <summary>根据上下文Key取值
        /// </summary>
        /// <param name="contextKey">Key</param>
        /// <returns></returns>
        T GetValue(string contextKey);

        /// <summary>开始上下文生命周期
        /// </summary>
        /// <param name="contextKey">上下文Key</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        IDisposable BeginScope(string contextKey, T value);
    }
}
