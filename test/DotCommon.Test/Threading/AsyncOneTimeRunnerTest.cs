using DotCommon.Threading;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotCommon.Test.Threading
{
    public class AsyncOneTimeRunnerTest
    {
        [Fact]
        public async Task RunAsync_Test()
        {
            int i = 1;
            AsyncOneTimeRunner oneTimeRunner = new AsyncOneTimeRunner();
            await oneTimeRunner.RunAsync(()=>
            {
                Interlocked.Increment(ref i);
                return Task.FromResult(1);
            });

            Assert.Equal(2, i);
        }

    }
}
