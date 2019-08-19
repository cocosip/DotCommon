using DotCommon.Scheduling;
using System.Threading;
using Xunit;

namespace DotCommon.Test.Serializing
{
    public class WorkerTest
    {
        [Fact]
        public void Work_Test()
        {
            var index = 0;
            var worker = new Worker(new MockLoggerFactory(), "a1", () =>
            {
                Interlocked.Increment(ref index);

            });
            worker.Start();
            worker.Start();
            Thread.Sleep(20);
            worker.Stop();

            Assert.Equal("a1", worker.ActionName);
            Assert.True(index > 0);

        }
    }
}
