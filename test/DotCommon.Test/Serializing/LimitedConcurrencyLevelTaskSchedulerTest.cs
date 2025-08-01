using DotCommon.Scheduling;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotCommon.Test.Serializing
{
    public class LimitedConcurrencyLevelTaskSchedulerTest
    {
        [Fact]
        public void LimitedConcurrencyLevelTaskScheduler_ValidMaxDegreeOfParallelism_CreatesScheduler()
        {
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(2);
            Assert.Equal(2, scheduler.MaximumConcurrencyLevel);
        }

        [Fact]
        public void LimitedConcurrencyLevelTaskScheduler_InvalidMaxDegreeOfParallelism_ThrowsException()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LimitedConcurrencyLevelTaskScheduler(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new LimitedConcurrencyLevelTaskScheduler(-1));
        }

        [Fact]
        public async Task LimitedConcurrencyLevelTaskScheduler_ScheduleAndExecuteTask_TaskExecutes()
        {
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(2);
            var executed = false;

            var task = new Task(() => { executed = true; });
            task.Start(scheduler);

            await task; // Wait for task completion

            Assert.True(executed);
        }

        [Fact]
        public async Task LimitedConcurrencyLevelTaskScheduler_ConcurrentTasks_RespectsLimit()
        {
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(2);
            var concurrentTasks = 0;
            var maxConcurrentTasks = 0;
            var tasks = new Task[10];

            // Create tasks that will track concurrent execution
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i] = new Task(() =>
                {
                    var currentConcurrent = Interlocked.Increment(ref concurrentTasks);

                    // Atomically update maxConcurrentTasks
                    int initialValue, computedValue;
                    do
                    {
                        initialValue = maxConcurrentTasks;
                        computedValue = Math.Max(initialValue, currentConcurrent);
                    } while (initialValue != Interlocked.CompareExchange(ref maxConcurrentTasks, computedValue, initialValue));

                    // Simulate some work
                    Thread.Sleep(50);

                    Interlocked.Decrement(ref concurrentTasks);
                });
            }

            // Start all tasks
            foreach (var task in tasks)
            {
                task.Start(scheduler);
            }

            // Wait for all tasks to complete
            await Task.WhenAll(tasks); // Use await Task.WhenAll instead of Task.WaitAll

            // Verify that concurrency never exceeded the limit
            Assert.True(maxConcurrentTasks <= 2);
        }
    }
}