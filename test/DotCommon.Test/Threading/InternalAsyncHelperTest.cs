using System;
using System.Threading.Tasks;
using DotCommon.Threading;
using Xunit;

namespace DotCommon.Test.Threading
{
    public class InternalAsyncHelperTest
    {
        [Fact]
        public async Task AwaitTaskWithFinally_ShouldExecuteFinally()
        {
            var finallyExecuted = false;
            await InternalAsyncHelper.AwaitTaskWithFinally(
                Task.CompletedTask,
                ex => finallyExecuted = true
            );
            Assert.True(finallyExecuted);
        }

        [Fact]
        public async Task AwaitTaskWithFinally_WithException_ShouldExecuteFinally()
        {
            var finallyExecuted = false;
            Exception capturedException = null;

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await InternalAsyncHelper.AwaitTaskWithFinally(
                    Task.FromException(new InvalidOperationException("Test")),
                    ex =>
                    {
                        finallyExecuted = true;
                        capturedException = ex;
                    }
                );
            });

            Assert.True(finallyExecuted);
            Assert.NotNull(capturedException);
            Assert.IsType<InvalidOperationException>(capturedException);
        }

        [Fact]
        public async Task AwaitTaskWithPostActionAndFinally_ShouldExecuteInOrder()
        {
            var executionOrder = new System.Collections.Generic.List<string>();

            await InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                Task.Run(() => executionOrder.Add("main")),
                () => { executionOrder.Add("post"); return Task.CompletedTask; },
                ex => executionOrder.Add("finally")
            );

            Assert.Equal(new[] { "main", "post", "finally" }, executionOrder);
        }

        [Fact]
        public async Task AwaitTaskWithPostActionAndFinally_WithException_ShouldExecuteFinally()
        {
            var finallyExecuted = false;

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await InternalAsyncHelper.AwaitTaskWithPostActionAndFinally(
                    Task.FromException(new InvalidOperationException("Test")),
                    () => Task.CompletedTask,
                    ex => finallyExecuted = true
                );
            });

            Assert.True(finallyExecuted);
        }

        [Fact]
        public async Task AwaitTaskWithPreActionAndPostActionAndFinally_ShouldExecuteInOrder()
        {
            var executionOrder = new System.Collections.Generic.List<string>();

            await InternalAsyncHelper.AwaitTaskWithPreActionAndPostActionAndFinally(
                () => { executionOrder.Add("main"); return Task.CompletedTask; },
                () => { executionOrder.Add("pre"); return Task.CompletedTask; },
                () => { executionOrder.Add("post"); return Task.CompletedTask; },
                ex => executionOrder.Add("finally")
            );

            Assert.Equal(new[] { "pre", "main", "post", "finally" }, executionOrder);
        }

        [Fact]
        public async Task AwaitTaskWithPreActionAndPostActionAndFinally_WithoutOptionalActions_ShouldExecute()
        {
            var executed = false;
            await InternalAsyncHelper.AwaitTaskWithPreActionAndPostActionAndFinally(
                () => { executed = true; return Task.CompletedTask; }
            );
            Assert.True(executed);
        }

        [Fact]
        public async Task AwaitTaskWithFinallyAndGetResult_ShouldReturnResult()
        {
            var finallyExecuted = false;
            var result = await InternalAsyncHelper.AwaitTaskWithFinallyAndGetResult(
                Task.FromResult(42),
                ex => finallyExecuted = true
            );

            Assert.Equal(42, result);
            Assert.True(finallyExecuted);
        }

        [Fact]
        public async Task AwaitTaskWithFinallyAndGetResult_WithException_ShouldThrow()
        {
            var finallyExecuted = false;

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await InternalAsyncHelper.AwaitTaskWithFinallyAndGetResult<int>(
                    Task.FromException<int>(new InvalidOperationException("Test")),
                    ex => finallyExecuted = true
                );
            });

            Assert.True(finallyExecuted);
        }

        [Fact]
        public async Task AwaitTaskWithPostActionAndFinallyAndGetResult_ShouldReturnResult()
        {
            var result = await InternalAsyncHelper.AwaitTaskWithPostActionAndFinallyAndGetResult(
                Task.FromResult(42),
                () => Task.CompletedTask,
                ex => { }
            );

            Assert.Equal(42, result);
        }

        [Fact]
        public async Task AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult_ShouldReturnResult()
        {
            var result = await InternalAsyncHelper.AwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult(
                () => Task.FromResult(42),
                () => Task.CompletedTask,
                () => Task.CompletedTask,
                ex => { }
            );

            Assert.Equal(42, result);
        }

        [Fact]
        public void CallAwaitTaskWithFinallyAndGetResult_ShouldReturnTask()
        {
            var task = InternalAsyncHelper.CallAwaitTaskWithFinallyAndGetResult(
                typeof(int),
                Task.FromResult(42),
                ex => { }
            );

            Assert.NotNull(task);
        }

        [Fact]
        public void CallAwaitTaskWithPostActionAndFinallyAndGetResult_ShouldReturnTask()
        {
            var task = InternalAsyncHelper.CallAwaitTaskWithPostActionAndFinallyAndGetResult(
                typeof(int),
                Task.FromResult(42),
                () => Task.CompletedTask,
                ex => { }
            );

            Assert.NotNull(task);
        }

        [Fact]
        public void CallAwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult_ShouldReturnTask()
        {
            var task = InternalAsyncHelper.CallAwaitTaskWithPreActionAndPostActionAndFinallyAndGetResult(
                typeof(int),
                () => Task.FromResult(42),
                () => Task.CompletedTask,
                () => Task.CompletedTask,
                ex => { }
            );

            Assert.NotNull(task);
        }
    }
}