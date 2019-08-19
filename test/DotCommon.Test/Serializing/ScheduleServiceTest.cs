using DotCommon.Scheduling;
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
            IScheduleService scheduleService = new ScheduleService(new MockLoggerFactory());
            scheduleService.StartTask("t1", () => { Interlocked.Increment(ref index); }, 50, 50);
            Thread.Sleep(100);
            scheduleService.StopTask("t1");
            Assert.True(index > 0);
        }
    }
}
