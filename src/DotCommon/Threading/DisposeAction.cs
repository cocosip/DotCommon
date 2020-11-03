using System;
using System.Threading;

namespace DotCommon
{
    /// <summary>
    /// 可释放的Action操作
    /// </summary>
    public class DisposeAction : IDisposable
    {
        public static readonly DisposeAction Empty = new DisposeAction(null);

        private Action _action;

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
