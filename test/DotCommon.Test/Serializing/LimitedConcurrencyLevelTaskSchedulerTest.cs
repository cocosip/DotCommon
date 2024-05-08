using DotCommon.Scheduling;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotCommon.Test.Serializing
{
    public class LimitedConcurrencyLevelTaskSchedulerTest
    {
        [Fact]
        public void LimitedConcurrencyLevelTaskScheduler_Test()
        {
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(2);
            var index = 1;
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            CancellationToken token = tokenSource.Token;
            var t = Task.Factory.StartNew(() =>
            {
                Interlocked.Increment(ref index);
            }, token, TaskCreationOptions.None, scheduler);
            Thread.Sleep(20);
            Assert.True(index > 1);
            tokenSource.Cancel();
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var scheduler2 = new LimitedConcurrencyLevelTaskScheduler(0);
            });

        }
    }

}
