using DotCommon.Scheduling;
using Microsoft.Extensions.Logging;
using Moq;
using System;
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

        [Fact]
        public void ScheduleService_Constructor_NullLogger_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new ScheduleService(null));
        }

        [Fact]
        public void StartTask_NullName_ThrowsException()
        {
            var scheduleService = new ScheduleService(_mockLogger.Object);
            Assert.Throws<ArgumentNullException>(() => scheduleService.StartTask(null, () => { }, 10, 10));
        }

        [Fact]
        public void StartTask_EmptyName_ThrowsException()
        {
            var scheduleService = new ScheduleService(_mockLogger.Object);
            Assert.Throws<ArgumentException>(() => scheduleService.StartTask("", () => { }, 10, 10));
        }

        [Fact]
        public void StartTask_NullAction_ThrowsException()
        {
            var scheduleService = new ScheduleService(_mockLogger.Object);
            Assert.Throws<ArgumentNullException>(() => scheduleService.StartTask("test", null, 10, 10));
        }

        [Fact]
        public void StartTask_NegativeDueTime_ThrowsException()
        {
            var scheduleService = new ScheduleService(_mockLogger.Object);
            Assert.Throws<ArgumentOutOfRangeException>(() => scheduleService.StartTask("test", () => { }, -1, 10));
        }

        [Fact]
        public void StartTask_NegativePeriod_ThrowsException()
        {
            var scheduleService = new ScheduleService(_mockLogger.Object);
            Assert.Throws<ArgumentOutOfRangeException>(() => scheduleService.StartTask("test", () => { }, 10, -1));
        }

        [Fact]
        public void StopTask_NullName_ThrowsException()
        {
            var scheduleService = new ScheduleService(_mockLogger.Object);
            Assert.Throws<ArgumentNullException>(() => scheduleService.StopTask(null));
        }

        [Fact]
        public void StartTask_DuplicateName_DoesNotAdd()
        {
            var scheduleService = new ScheduleService(_mockLogger.Object);
            var callCount = 0;

            scheduleService.StartTask("duplicate", () => { callCount++; }, 10, 10);
            scheduleService.StartTask("duplicate", () => { callCount += 10; }, 10, 10); // Should not be added

            Thread.Sleep(50);
            scheduleService.StopTask("duplicate");

            // Only the first task should have executed
            Assert.True(callCount > 0 && callCount % 10 != 0);
        }
    }
}