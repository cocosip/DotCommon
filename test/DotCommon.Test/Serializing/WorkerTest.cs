using DotCommon.Scheduling;
using Microsoft.Extensions.Logging;
using Moq;
using System;
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
            worker.Start(); // Should not start again
            Thread.Sleep(20);
            worker.Stop();

            Assert.Equal("a1", worker.ActionName);
            Assert.True(index > 0);
        }

        [Fact]
        public void Worker_Constructor_NullLogger_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new Worker(null, "test", () => { }));
        }

        [Fact]
        public void Worker_Constructor_NullActionName_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new Worker(_mockLogger.Object, null, () => { }));
        }

        [Fact]
        public void Worker_Constructor_NullAction_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new Worker(_mockLogger.Object, "test", null));
        }

        [Fact]
        public void Worker_Start_ReturnsSelf()
        {
            var worker = new Worker(_mockLogger.Object, "test", () => { });
            var result = worker.Start();
            Assert.Same(worker, result);
        }

        [Fact]
        public void Worker_Stop_ReturnsSelf()
        {
            var worker = new Worker(_mockLogger.Object, "test", () => { });
            var result = worker.Stop();
            Assert.Same(worker, result);
        }
    }
}