using System;
using System.Threading;

namespace DotCommon.Threading
{
    /// <summary>
    /// 可释放的Action操作
    /// </summary>
    public class DisposeAction : IDisposable
    {
        /// <summary>
        /// 空的DisposeAction
        /// </summary>
        public static readonly DisposeAction Empty = new DisposeAction(null);

        private Action _action;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="action"></param>
        public DisposeAction(Action action)
        {
            _action = action;
        }

        /// <summary>
        /// 释放操作
        /// </summary>
        public void Dispose()
        {
            var action = Interlocked.Exchange(ref _action, null);
            action?.Invoke();
        }
    }
}
