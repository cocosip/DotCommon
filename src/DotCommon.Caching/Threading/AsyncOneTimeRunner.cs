using Nito.AsyncEx;
using System;
using System.Threading.Tasks;

namespace DotCommon.Threading
{
    /// <summary>异步一次运行方法
    /// </summary>
    public class AsyncOneTimeRunner
    {
        private volatile bool _runBefore;
        private readonly AsyncLock _asyncLock = new AsyncLock();

        /// <summary>Ctor
        /// </summary>
        public async Task RunAsync(Func<Task> action)
        {
            if (_runBefore)
            {
                return;
            }

            using (await _asyncLock.LockAsync())
            {
                if (_runBefore)
                {
                    return;
                }

                await action();

                _runBefore = true;
            }
        }
    }
}
