using System.Threading.Tasks;
using DotCommon.Threading;
using Xunit;

namespace DotCommon.Test.Threading
{
    public class OneTimeRunnerTest
    {
        [Fact]
        public void Run_ShouldExecuteActionOnce()
        {
            var runner = new OneTimeRunner();
            var count = 0;

            runner.Run(() => count++);
            runner.Run(() => count++);
            runner.Run(() => count++);

            Assert.Equal(1, count);
        }

        [Fact]
        public void Run_WithMultipleRunners_ShouldExecuteEachOnce()
        {
            var runner1 = new OneTimeRunner();
            var runner2 = new OneTimeRunner();
            var count1 = 0;
            var count2 = 0;

            runner1.Run(() => count1++);
            runner1.Run(() => count1++);
            runner2.Run(() => count2++);
            runner2.Run(() => count2++);

            Assert.Equal(1, count1);
            Assert.Equal(1, count2);
        }

        [Fact]
        public void Run_WithException_ShouldAllowRetry()
        {
            var runner = new OneTimeRunner();
            var count = 0;

            Assert.Throws<System.InvalidOperationException>(() =>
            {
                runner.Run(() =>
                {
                    count++;
                    throw new System.InvalidOperationException();
                });
            });

            runner.Run(() => count++);
            Assert.Equal(2, count);
        }
    }

    public class AsyncOneTimeRunnerTest
    {
        [Fact]
        public async Task RunAsync_ShouldExecuteActionOnce()
        {
            var runner = new AsyncOneTimeRunner();
            var count = 0;

            await runner.RunAsync(async () =>
            {
                await Task.Delay(1);
                count++;
            });

            await runner.RunAsync(async () =>
            {
                await Task.Delay(1);
                count++;
            });

            await runner.RunAsync(async () =>
            {
                await Task.Delay(1);
                count++;
            });

            Assert.Equal(1, count);
        }

        [Fact]
        public async Task RunAsync_WithMultipleRunners_ShouldExecuteEachOnce()
        {
            var runner1 = new AsyncOneTimeRunner();
            var runner2 = new AsyncOneTimeRunner();
            var count1 = 0;
            var count2 = 0;

            await runner1.RunAsync(async () =>
            {
                await Task.Delay(1);
                count1++;
            });
            await runner1.RunAsync(async () =>
            {
                await Task.Delay(1);
                count1++;
            });

            await runner2.RunAsync(async () =>
            {
                await Task.Delay(1);
                count2++;
            });
            await runner2.RunAsync(async () =>
            {
                await Task.Delay(1);
                count2++;
            });

            Assert.Equal(1, count1);
            Assert.Equal(1, count2);
        }

        [Fact]
        public async Task RunAsync_WithException_ShouldAllowRetry()
        {
            var runner = new AsyncOneTimeRunner();
            var count = 0;

            await Assert.ThrowsAsync<System.InvalidOperationException>(async () =>
            {
                await runner.RunAsync(async () =>
                {
                    await Task.Delay(1);
                    count++;
                    throw new System.InvalidOperationException();
                });
            });

            await runner.RunAsync(async () =>
            {
                await Task.Delay(1);
                count++;
            });

            Assert.Equal(2, count);
        }

        [Fact]
        public async Task RunAsync_ConcurrentCalls_ShouldOnlyExecuteOnce()
        {
            var runner = new AsyncOneTimeRunner();
            var count = 0;

            var tasks = new Task[10];
            for (var i = 0; i < 10; i++)
            {
                tasks[i] = runner.RunAsync(async () =>
                {
                    await Task.Delay(10);
                    count++;
                });
            }

            await Task.WhenAll(tasks);

            Assert.Equal(1, count);
        }
    }
}