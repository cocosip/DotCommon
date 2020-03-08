using DotCommon.Scheduling;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading;
using Xunit;

namespace DotCommon.Test.Serializing
{
    public class ScheduleServiceTest
    {
        private readonly Mock<ILogger<ScheduleService>> _mockLogger;

        public ScheduleServiceTest()
        {
            _mockLogger = new Mock<ILogger<ScheduleService>>();
        }

        [Fact]
        public void ScheduleService_Test()
        {
            var index = 0;
            IScheduleService scheduleService = new ScheduleService(_mockLogger.Object);
            scheduleService.StartTask("t1", () => { Interlocked.Increment(ref index); }, 10, 10);
            Thread.Sleep(100);
            scheduleService.StopTask("t1");
            Assert.True(index > 0);
        }
    }
}
