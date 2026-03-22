using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotCommon.Scheduling;
using Xunit;

namespace DotCommon.Test.Scheduling
{
    public class LimitedConcurrencyLevelTaskSchedulerTest
    {
        [Fact]
        public void Constructor_WithValidConcurrency_ShouldCreate()
        {
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(2);
            Assert.Equal(2, scheduler.MaximumConcurrencyLevel);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-10)]
        public void Constructor_WithInvalidConcurrency_ShouldThrow(int maxConcurrency)
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new LimitedConcurrencyLevelTaskScheduler(maxConcurrency));
        }

        [Fact]
        public void MaximumConcurrencyLevel_ShouldReturnConfiguredValue()
        {
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(4);
            Assert.Equal(4, scheduler.MaximumConcurrencyLevel);
        }

        [Fact]
        public async Task ExecuteTask_ShouldRunTask()
        {
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(1);
            var executed = false;

            var task = Task.Factory.StartNew(() => executed = true, CancellationToken.None, TaskCreationOptions.None, scheduler);
            await task;

            Assert.True(executed);
        }

        [Fact]
        public async Task ExecuteMultipleTasks_ShouldRunAll()
        {
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(2);
            var counter = 0;

            var tasks = new Task[10];
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => Interlocked.Increment(ref counter), CancellationToken.None, TaskCreationOptions.None, scheduler);
            }

            await Task.WhenAll(tasks);

            Assert.Equal(10, counter);
        }

        [Fact]
        public async Task WithLimitedConcurrency_ShouldLimitConcurrentTasks()
        {
            var maxConcurrency = 2;
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(maxConcurrency);
            var currentRunning = 0;
            var maxObserved = 0;
            var lockObj = new object();

            var tasks = new Task[10];
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = Task.Factory.StartNew(() =>
                {
                    lock (lockObj)
                    {
                        currentRunning++;
                        if (currentRunning > maxObserved)
                        {
                            maxObserved = currentRunning;
                        }
                    }

                    Task.Delay(50).Wait();

                    lock (lockObj)
                    {
                        currentRunning--;
                    }
                }, CancellationToken.None, TaskCreationOptions.None, scheduler);
            }

            await Task.WhenAll(tasks);

            Assert.True(maxObserved <= maxConcurrency);
        }

        [Fact]
        public async Task SingleConcurrency_ShouldExecuteSequentially()
        {
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(1);
            var executionOrder = new System.Collections.Generic.List<int>();

            var task1 = Task.Factory.StartNew(() => executionOrder.Add(1), CancellationToken.None, TaskCreationOptions.None, scheduler);
            var task2 = Task.Factory.StartNew(() => executionOrder.Add(2), CancellationToken.None, TaskCreationOptions.None, scheduler);
            var task3 = Task.Factory.StartNew(() => executionOrder.Add(3), CancellationToken.None, TaskCreationOptions.None, scheduler);

            await Task.WhenAll(task1, task2, task3);

            Assert.Equal(new[] { 1, 2, 3 }, executionOrder);
        }

        [Fact]
        public async Task HighConcurrency_ShouldAllowManyParallelTasks()
        {
            var scheduler = new LimitedConcurrencyLevelTaskScheduler(10);
            var counter = 0;

            var tasks = new Task[100];
            for (var i = 0; i < 100; i++)
            {
                tasks[i] = Task.Factory.StartNew(() => Interlocked.Increment(ref counter), CancellationToken.None, TaskCreationOptions.None, scheduler);
            }

            await Task.WhenAll(tasks);

            Assert.Equal(100, counter);
        }
    }
}