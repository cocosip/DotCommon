using System;
using System.Reflection;
using System.Threading.Tasks;
using DotCommon.Threading;
using Xunit;

namespace DotCommon.Test.Threading
{
    public class AsyncHelperTest
    {
        [Fact]
        public void IsAsync_WithAsyncMethod_ShouldReturnTrue()
        {
            MethodInfo method = GetType().GetMethod(nameof(AsyncMethod))!;
            Assert.True(method.IsAsync());
        }

        [Fact]
        public void IsAsync_WithSyncMethod_ShouldReturnFalse()
        {
            MethodInfo method = GetType().GetMethod(nameof(SyncMethod))!;
            Assert.False(method.IsAsync());
        }

        [Fact]
        public void IsAsync_WithAsyncMethodWithResult_ShouldReturnTrue()
        {
            MethodInfo method = GetType().GetMethod(nameof(AsyncMethodWithResult))!;
            Assert.True(method.IsAsync());
        }

        [Fact]
        public void IsTaskOrTaskOfT_WithTask_ShouldReturnTrue()
        {
            Assert.True(typeof(Task).IsTaskOrTaskOfT());
        }

        [Fact]
        public void IsTaskOrTaskOfT_WithTaskOfT_ShouldReturnTrue()
        {
            Assert.True(typeof(Task<int>).IsTaskOrTaskOfT());
        }

        [Fact]
        public void IsTaskOrTaskOfT_WithOtherType_ShouldReturnFalse()
        {
            Assert.False(typeof(int).IsTaskOrTaskOfT());
            Assert.False(typeof(string).IsTaskOrTaskOfT());
        }

        [Fact]
        public void IsTaskOfT_WithTaskOfT_ShouldReturnTrue()
        {
            Assert.True(typeof(Task<int>).IsTaskOfT());
            Assert.True(typeof(Task<string>).IsTaskOfT());
        }

        [Fact]
        public void IsTaskOfT_WithTask_ShouldReturnFalse()
        {
            Assert.False(typeof(Task).IsTaskOfT());
        }

        [Fact]
        public void IsTaskOfT_WithOtherType_ShouldReturnFalse()
        {
            Assert.False(typeof(int).IsTaskOfT());
        }

        [Fact]
        public void UnwrapTask_WithTask_ShouldReturnVoid()
        {
            Assert.Equal(typeof(void), AsyncHelper.UnwrapTask(typeof(Task)));
        }

        [Fact]
        public void UnwrapTask_WithTaskOfT_ShouldReturnT()
        {
            Assert.Equal(typeof(int), AsyncHelper.UnwrapTask(typeof(Task<int>)));
            Assert.Equal(typeof(string), AsyncHelper.UnwrapTask(typeof(Task<string>)));
        }

        [Fact]
        public void UnwrapTask_WithOtherType_ShouldReturnSameType()
        {
            Assert.Equal(typeof(int), AsyncHelper.UnwrapTask(typeof(int)));
            Assert.Equal(typeof(string), AsyncHelper.UnwrapTask(typeof(string)));
        }

        [Fact]
        public void RunSync_WithTask_ShouldExecute()
        {
            var executed = false;
            AsyncHelper.RunSync(async () =>
            {
                await Task.Delay(10);
                executed = true;
            });
            Assert.True(executed);
        }

        [Fact]
        public void RunSync_WithTaskOfResult_ShouldReturnResult()
        {
            var result = AsyncHelper.RunSync(async () =>
            {
                await Task.Delay(10);
                return 42;
            });
            Assert.Equal(42, result);
        }

        [Fact]
        public void RunSync_WithException_ShouldPropagate()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                AsyncHelper.RunSync(async () =>
                {
                    await Task.Delay(10);
                    throw new InvalidOperationException("Test exception");
                });
            });
        }

        [Fact]
        public void RunSync_WithResultException_ShouldPropagate()
        {
            Assert.Throws<ArgumentException>(() =>
            {
                AsyncHelper.RunSync(async () =>
                {
                    await Task.Delay(10);
                    throw new ArgumentException("Test exception");
                });
            });
        }

        #region Test Methods

#pragma warning disable xUnit1013 // Public method should be marked as Fact
        public async Task AsyncMethod()
        {
            await Task.Delay(1);
        }

        public void SyncMethod()
        {
        }

        public async Task<int> AsyncMethodWithResult()
        {
            await Task.Delay(1);
            return 1;
        }
#pragma warning restore xUnit1013
        #endregion
    }
}