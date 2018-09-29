using Nito.AsyncEx;
using System;
using System.Threading.Tasks;

namespace DotCommon.Threading
{
    public class AsyncOneTimeRunner
    {
        private volatile bool _runBefore;
        private readonly AsyncLock _asyncLock = new AsyncLock();

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
