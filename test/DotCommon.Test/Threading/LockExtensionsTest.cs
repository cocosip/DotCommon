using System;
using System.Threading;
using System.Threading.Tasks;
using DotCommon.Threading;
using Xunit;

namespace DotCommon.Test.Threading
{
    public class LockExtensionsTest
    {
        [Fact]
        public void Locking_WithAction_ShouldExecuteUnderLock()
        {
            var obj = new object();
            var executed = false;

            obj.Locking(() => executed = true);

            Assert.True(executed);
        }

        [Fact]
        public void Locking_WithGenericAction_ShouldExecuteUnderLock()
        {
            var obj = new TestLockClass { Value = 0 };

            obj.Locking(x => x.Value = 42);

            Assert.Equal(42, obj.Value);
        }

        [Fact]
        public void Locking_WithFunc_ShouldReturnResult()
        {
            var obj = new object();
            var result = obj.Locking(() => 42);

            Assert.Equal(42, result);
        }

        [Fact]
        public void Locking_WithGenericFunc_ShouldReturnResult()
        {
            var obj = new TestLockClass { Value = 100 };

            var result = obj.Locking(x => x.Value * 2);

            Assert.Equal(200, result);
        }

        [Fact]
        public async Task Locking_WithMultipleThreads_ShouldBeThreadSafe()
        {
            var obj = new TestLockClass { Value = 0 };
            var tasks = new Task[100];

            for (var i = 0; i < 100; i++)
            {
                tasks[i] = Task.Run(() => obj.Locking(x => x.Value++));
            }

            await Task.WhenAll(tasks);

            Assert.Equal(100, obj.Value);
        }

        [Fact]
        public void Locking_WithException_ShouldPropagate()
        {
            var obj = new object();

            Assert.Throws<InvalidOperationException>(() =>
            {
                obj.Locking(() => throw new InvalidOperationException("Test"));
            });
        }

        [Fact]
        public void Locking_WithGenericActionAndException_ShouldPropagate()
        {
            var obj = new TestLockClass { Value = 0 };

            Assert.Throws<InvalidOperationException>(() =>
            {
                obj.Locking(x => throw new InvalidOperationException("Test"));
            });
        }

        [Fact]
        public void Locking_WithGenericFuncAndException_ShouldPropagate()
        {
            var obj = new TestLockClass { Value = 0 };

            Assert.Throws<InvalidOperationException>(() =>
            {
                obj.Locking(x =>
                {
                    throw new InvalidOperationException("Test");
                    return 0;
                });
            });
        }

        private class TestLockClass
        {
            public int Value { get; set; }
        }
    }

    public class SemaphoreSlimExtensionsTest
    {
        [Fact]
        public async Task LockAsync_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);

            using (await semaphore.LockAsync())
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }

        [Fact]
        public async Task LockAsync_WithCancellationToken_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);
            var cts = new CancellationTokenSource();

            using (await semaphore.LockAsync(cts.Token))
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }

        [Fact]
        public async Task LockAsync_WithTimeout_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);

            using (await semaphore.LockAsync(1000))
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }

        [Fact]
        public async Task LockAsync_WithTimeoutAndCancellationToken_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);
            var cts = new CancellationTokenSource();

            using (await semaphore.LockAsync(1000, cts.Token))
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }

        [Fact]
        public async Task LockAsync_WithTimeSpan_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);

            using (await semaphore.LockAsync(TimeSpan.FromSeconds(1)))
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }

        [Fact]
        public async Task LockAsync_WithTimeSpanAndCancellationToken_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);
            var cts = new CancellationTokenSource();

            using (await semaphore.LockAsync(TimeSpan.FromSeconds(1), cts.Token))
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }

        [Fact]
        public async Task LockAsync_WithTimeoutExpired_ShouldThrowTimeoutException()
        {
            var semaphore = new SemaphoreSlim(0, 1);

            await Assert.ThrowsAsync<TimeoutException>(async () =>
            {
                using (await semaphore.LockAsync(1)) { }
            });
        }

        [Fact]
        public void Lock_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);

            using (semaphore.Lock())
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }

        [Fact]
        public void Lock_WithCancellationToken_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);
            var cts = new CancellationTokenSource();

            using (semaphore.Lock(cts.Token))
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }

        [Fact]
        public void Lock_WithTimeout_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);

            using (semaphore.Lock(1000))
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }

        [Fact]
        public void Lock_WithTimeSpan_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);

            using (semaphore.Lock(TimeSpan.FromSeconds(1)))
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }

        [Fact]
        public void Lock_WithTimeoutExpired_ShouldThrowTimeoutException()
        {
            var semaphore = new SemaphoreSlim(0, 1);

            Assert.Throws<TimeoutException>(() =>
            {
                using (semaphore.Lock(1)) { }
            });
        }

        [Fact]
        public void Lock_WithTimeoutAndCancellationToken_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);
            var cts = new CancellationTokenSource();

            using (semaphore.Lock(1000, cts.Token))
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }

        [Fact]
        public void Lock_WithTimeSpanAndCancellationToken_ShouldAcquireAndRelease()
        {
            var semaphore = new SemaphoreSlim(1, 1);
            var cts = new CancellationTokenSource();

            using (semaphore.Lock(TimeSpan.FromSeconds(1), cts.Token))
            {
                Assert.Equal(0, semaphore.CurrentCount);
            }

            Assert.Equal(1, semaphore.CurrentCount);
        }
    }
}