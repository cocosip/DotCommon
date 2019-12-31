using DotCommon.Scheduling;
using Microsoft.Extensions.Logging;
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
            var logger = new MockLoggerFactory().CreateLogger("Worker") as ILogger<Worker>;
            var worker = new Worker(logger, "a1", () =>
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
