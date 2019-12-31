using DotCommon.Scheduling;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Xunit;

namespace DotCommon.Test.Serializing
{
    public class ScheduleServiceTest
    {
        [Fact]
        public void ScheduleService_Test()
        {
            var index = 0;
            var logger = new MockLoggerFactory().CreateLogger("ScheduleService") as ILogger<ScheduleService>;
            IScheduleService scheduleService = new ScheduleService(logger);
            scheduleService.StartTask("t1", () => { Interlocked.Increment(ref index); }, 50, 50);
            Thread.Sleep(100);
            scheduleService.StopTask("t1");
            Assert.True(index > 0);
        }
    }
}
