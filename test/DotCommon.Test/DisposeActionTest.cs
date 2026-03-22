using System;
using Xunit;

namespace DotCommon.Test
{
    public class DisposeActionTest
    {
        [Fact]
        public void Dispose_ShouldExecuteAction()
        {
            var executed = false;
            using (new DisposeAction(() => executed = true))
            {
                Assert.False(executed);
            }
            Assert.True(executed);
        }

        [Fact]
        public void Dispose_WithNullAction_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new DisposeAction(null!));
        }

        [Fact]
        public void Dispose_ShouldBeCalledWhenLeavingScope()
        {
            var count = 0;
            {
                using var disposeAction = new DisposeAction(() => count++);
                Assert.Equal(0, count);
            }
            Assert.Equal(1, count);
        }

        [Fact]
        public void Dispose_CalledMultipleTimes_ShouldExecuteMultipleTimes()
        {
            var count = 0;
            var disposeAction = new DisposeAction(() => count++);
            disposeAction.Dispose();
            Assert.Equal(1, count);
            disposeAction.Dispose();
            Assert.Equal(2, count);
        }
    }

    public class DisposeActionTTest
    {
        [Fact]
        public void Dispose_ShouldExecuteActionWithParameter()
        {
            var result = 0;
            using (new DisposeAction<int>(x => result = x, 42))
            {
                Assert.Equal(0, result);
            }
            Assert.Equal(42, result);
        }

        [Fact]
        public void Dispose_WithNullAction_ShouldThrow()
        {
            Assert.Throws<ArgumentNullException>(() => new DisposeAction<int>(null!, 0));
        }

        [Fact]
        public void Dispose_WithReferenceType_ShouldPassParameter()
        {
            var capturedValue = "";
            using (new DisposeAction<string>(s => capturedValue = s, "test"))
            {
                Assert.Equal("", capturedValue);
            }
            Assert.Equal("test", capturedValue);
        }

        [Fact]
        public void Dispose_WithNullParameter_ShouldNotExecute()
        {
            var executed = false;
            using (new DisposeAction<string>(s => executed = s != null, null))
            {
            }
            Assert.False(executed);
        }
    }
}