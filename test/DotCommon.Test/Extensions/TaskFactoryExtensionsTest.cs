using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotCommon.Test.Extensions
{
    public class TaskFactoryExtensionsTest
    {
        [Fact]
        public async Task StartDelayedTask_Should_Throw_ArgumentNullException_For_Null_Factory()
        {
            // Assert that calling the method with a null factory throws ArgumentNullException.
            await Assert.ThrowsAsync<ArgumentNullException>(() => TaskFactoryExtensions.StartDelayedTask(null, 100, () => { }));
        }

        [Fact]
        public async Task StartDelayedTask_Should_Throw_ArgumentNullException_For_Null_Action()
        {
            // Assert that calling the method with a null action throws ArgumentNullException.
            await Assert.ThrowsAsync<ArgumentNullException>(() => Task.Factory.StartDelayedTask(100, null));
        }

        [Fact]
        public async Task StartDelayedTask_Should_Throw_ArgumentOutOfRangeException_For_Negative_Delay()
        {
            // Assert that a negative delay throws ArgumentOutOfRangeException.
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => Task.Factory.StartDelayedTask(-1, () => { }));
        }

        [Fact]
        public async Task StartDelayedTask_Should_Execute_Action_After_Delay()
        {
            const int delay = 200;
            var executed = false;
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            var task = Task.Factory.StartDelayedTask(delay, () =>
            {
                executed = true;
                stopwatch.Stop();
            });

            await task;

            Assert.True(executed);
            // Ensure the delay was respected by checking the elapsed time.
            Assert.True(stopwatch.ElapsedMilliseconds >= delay, $"Elapsed time should be >= {delay}ms, but was {stopwatch.ElapsedMilliseconds}ms");
        }

        [Fact]
        public async Task StartDelayedTask_Should_Not_Execute_Action_If_Canceled_Before_Delay()
        {
            const int delay = 500;
            var executed = false;
            var cts = new CancellationTokenSource();
            var factory = new TaskFactory(cts.Token);

            var task = factory.StartDelayedTask(delay, () => executed = true);

            // Cancel the task before the delay has passed.
            cts.Cancel();

            // Expect a TaskCanceledException when awaiting the task.
            await Assert.ThrowsAsync<TaskCanceledException>(() => task);

            // Ensure the action was not executed and the task is in a canceled state.
            Assert.False(executed);
            Assert.True(task.IsCanceled);
        }

        [Fact]
        public void StartDelayedTask_With_PreCanceled_Token_Should_Return_Canceled_Task()
        {
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var factory = new TaskFactory(cts.Token);

            // The method should immediately return a canceled task.
            var task = factory.StartDelayedTask(100, () => { });

            Assert.True(task.IsCanceled);
        }
    }
}
