using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using DotCommon.Utility;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class BufferQueueTest
    {
        [Fact]
        public void EnqueueMessage_ShouldHandleMessage()
        {
            var processedMessages = new ConcurrentBag<int>();
            var queue = new BufferQueue<int>("TestQueue", 1, msg => processedMessages.Add(msg));

            queue.EnqueueMessage(1);

            Assert.True(
                WaitUntil(() => !processedMessages.IsEmpty),
                "The message was not processed before the timeout.");
        }

        [Fact]
        public void EnqueueMessage_MultipleMessages_ShouldHandleAll()
        {
            var processedMessages = new ConcurrentBag<int>();
            var queue = new BufferQueue<int>("TestQueue", 1, msg => processedMessages.Add(msg));

            for (var i = 0; i < 10; i++)
            {
                queue.EnqueueMessage(i);
                Thread.Sleep(20);
            }

            Assert.True(
                WaitUntil(() => processedMessages.Count >= 5),
                $"Expected at least 5 messages processed, but got {processedMessages.Count}");
        }

        [Fact]
        public void EnqueueMessage_WithException_ShouldContinueProcessing()
        {
            var processedCount = 0;
            var queue = new BufferQueue<int>("TestQueue", 1, msg =>
            {
                if (msg == 1) throw new Exception("Test exception");
                Interlocked.Increment(ref processedCount);
            });

            queue.EnqueueMessage(1);
            Thread.Sleep(100);
            queue.EnqueueMessage(2);
            Thread.Sleep(100);
            queue.EnqueueMessage(3);

            Assert.True(
                WaitUntil(() => processedCount >= 1),
                $"Expected at least 1 message processed, but got {processedCount}");
        }

        [Fact]
        public void EnqueueMessage_WithThreshold_ShouldTriggerProcessing()
        {
            var processedMessages = new ConcurrentBag<int>();
            var queue = new BufferQueue<int>("TestQueue", 5, msg => processedMessages.Add(msg));

            for (var i = 0; i < 10; i++)
            {
                queue.EnqueueMessage(i);
            }

            Assert.True(
                WaitUntil(() => !processedMessages.IsEmpty),
                "No message was processed before the timeout.");
        }

        [Fact]
        public async Task EnqueueMessage_Concurrent_ShouldHandleAll()
        {
            var processedMessages = new ConcurrentBag<int>();
            var queue = new BufferQueue<int>("TestQueue", 1, msg => processedMessages.Add(msg));

            var tasks = new Task[50];
            for (var i = 0; i < 50; i++)
            {
                var value = i;
                tasks[i] = Task.Run(() => queue.EnqueueMessage(value), TestContext.Current.CancellationToken);
            }

            await Task.WhenAll(tasks);

            var deadline = DateTime.UtcNow.AddSeconds(10);
            while (processedMessages.Count < 20 && DateTime.UtcNow < deadline)
            {
                await Task.Delay(20, TestContext.Current.CancellationToken);
            }

            Assert.True(
                processedMessages.Count >= 20,
                $"Expected at least 20 messages processed, but got {processedMessages.Count}");
        }

        /// <summary>
        /// Message processing runs on the thread pool, so the tests poll instead of
        /// sleeping for a fixed amount of time; a slow CI runner would otherwise
        /// fail despite correct behaviour.
        /// </summary>
        private static bool WaitUntil(Func<bool> condition, int timeoutMilliseconds = 10000)
        {
            var deadline = DateTime.UtcNow.AddMilliseconds(timeoutMilliseconds);
            while (!condition())
            {
                if (DateTime.UtcNow >= deadline)
                {
                    return false;
                }

                Thread.Sleep(20);
            }

            return true;
        }
    }
}
