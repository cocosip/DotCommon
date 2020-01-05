using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DotCommon.Extensions;
using System.Threading;
using System.Linq;

namespace DotCommon.Test.Extensions
{
    public class TaskExtensionsTest
    {


        [Fact]
        public void WaitResult_Test()
        {
            Task<int> t1 = Task.FromResult(1);
            var v1 = t1.WaitResult(10);
            Assert.Equal(1, v1);

            Task<int> t2 = Task.Run(() =>
            {
                Thread.Sleep(100);
                return 2;
            });

            var v2 = t2.WaitResult(1);
            Assert.Equal(0, v2);
        }

        [Fact]
        public void TimeoutAfter_Test()
        {
            Task t1 = Task.FromResult(0);
            t1.TimeoutAfter(10).Wait();
            Assert.Equal(TaskStatus.RanToCompletion, t1.Status);


            Task t2 = Task.Run(() =>
            {
                Thread.Sleep(100);
            });

            var aggregateException = Assert.Throws<AggregateException>(() =>
            {
                t2.TimeoutAfter(1).Wait();
            });

            var timeoutException = aggregateException.InnerExceptions.FirstOrDefault(x => x.GetType() == typeof(TimeoutException));
            
            Assert.NotNull(timeoutException);

        }

    }
}
