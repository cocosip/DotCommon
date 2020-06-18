using DotCommon.Scheduling;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using Xunit;

namespace DotCommon.Test.Serializing
{
    public class WorkerTest
    {
        private readonly Mock<ILogger<Worker>> _mockLogger;

        public WorkerTest()
        {
            _mockLogger = new Mock<ILogger<Worker>>();
        }


        [Fact]
        public void Work_Test()
        {
            var index = 0;
            var worker = new Worker(_mockLogger.Object, "a1", () =>
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
