using DotCommon.Scheduling;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
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
            using var scheduleService = new ScheduleService(_mockLogger.Object);
            var callCount = 0;

            scheduleService.StartTask("duplicate", () => { callCount++; }, 10, 10);
            scheduleService.StartTask("duplicate", () => { callCount += 10; }, 10, 10); // Should not be added

            Thread.Sleep(50);
            scheduleService.StopTask("duplicate");

            // Only the first task should have executed
            Assert.True(callCount > 0 && callCount % 10 != 0);
        }

        [Fact]
        public async Task ScheduleService_StartAndExecuteTask()
        {
            using var scheduleService = new ScheduleService(_mockLogger.Object);
            var callCount = 0;
            var taskExecuted = new TaskCompletionSource<bool>();

            scheduleService.StartTask("testTask", () => {
                Interlocked.Increment(ref callCount);
                taskExecuted.SetResult(true);
            }, 10, 0); // Execute once after 10ms

            // Wait for task to execute or timeout after 100ms
            using var cts = new CancellationTokenSource(100);
            try
            {
                await taskExecuted.Task.WaitAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                // Timeout - task didn't execute
            }

            scheduleService.StopTask("testTask");

            // Verify task executed
            Assert.True(callCount > 0);
        }

        [Fact]
        public async Task ScheduleService_StartAndStopTask()
        {
            using var scheduleService = new ScheduleService(_mockLogger.Object);
            var callCount = 0;

            scheduleService.StartTask("testTask", () => {
                Interlocked.Increment(ref callCount);
            }, 10, 50); // Execute every 50ms after 10ms delay

            // Wait a bit for the task to potentially execute
            await Task.Delay(30);

            // Stop the task
            scheduleService.StopTask("testTask");

            // Capture the count
            var countAfterStop = callCount;

            // Wait a bit more to ensure no more executions
            await Task.Delay(100);

            // Verify no more executions after stop
            Assert.Equal(countAfterStop, callCount);
        }

        [Fact]
        public void ScheduleService_StopNonExistentTask_LogsWarning()
        {
            using var scheduleService = new ScheduleService(_mockLogger.Object);
            
            // Setup logger verification
            _mockLogger.Setup(logger => logger.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()));

            scheduleService.StopTask("nonExistentTask");

            // Verify warning was logged
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("not found")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public async Task ScheduleService_TaskWithException_HandlesGracefully()
        {
            using var scheduleService = new ScheduleService(_mockLogger.Object);
            var taskExecuted = new TaskCompletionSource<bool>();

            // Setup logger verification for error log
            _mockLogger.Setup(logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Exception occurred")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()))
                .Callback(() => taskExecuted.SetResult(true));

            scheduleService.StartTask("errorTask", () => {
                throw new InvalidOperationException("Test exception");
            }, 10, 0); // Execute once

            // Wait for task to execute or timeout
            using var cts = new CancellationTokenSource(100);
            try
            {
                await taskExecuted.Task.WaitAsync(cts.Token);
            }
            catch (OperationCanceledException)
            {
                // Timeout - task didn't execute
            }

            scheduleService.StopTask("errorTask");

            // Verify error was logged
            _mockLogger.Verify(logger => logger.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Exception occurred")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
        }

        [Fact]
        public void ScheduleService_Dispose_StopsAllTasks()
        {
            var scheduleService = new ScheduleService(_mockLogger.Object);
            var callCount = 0;

            scheduleService.StartTask("testTask1", () => { Interlocked.Increment(ref callCount); }, 10, 50);
            scheduleService.StartTask("testTask2", () => { Interlocked.Increment(ref callCount); }, 20, 50);

            // Wait a bit for tasks to potentially execute
            Thread.Sleep(30);

            // Capture count before dispose
            var countBeforeDispose = callCount;

            // Dispose the service
            scheduleService.Dispose();

            // Wait a bit more to ensure no more executions
            Thread.Sleep(100);

            // Verify no more executions after dispose
            Assert.Equal(countBeforeDispose, callCount);
        }
    }
}