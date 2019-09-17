﻿using System;
using System.Threading;

namespace DotCommon
{
    /// <summary>可释放的Action操作
    /// </summary>
    public class DisposeAction : IDisposable
    {
        /// <summary>Empty
        /// </summary>
        public static readonly DisposeAction Empty = new DisposeAction(null);

        private Action _action;

        /// <summary>Ctor
        /// </summary>
        public DisposeAction(Action action)
        {
            _action = action;
        }

        /// <summary>释放操作
        /// </summary>
        public void Dispose()
        {
            // Interlocked prevents multiple execution of the _action.
            var action = Interlocked.Exchange(ref _action, null);
            action?.Invoke();
        }
    }
}
