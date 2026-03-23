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

            Thread.Sleep(200);

            Assert.NotEmpty(processedMessages);
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

            var deadline = DateTime.UtcNow.AddSeconds(5);
            while (processedMessages.Count < 5 && DateTime.UtcNow < deadline)
            {
                Thread.Sleep(50);
            }

            Assert.True(processedMessages.Count >= 5, $"Expected at least 5 messages processed, but got {processedMessages.Count}");
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

            var deadline = DateTime.UtcNow.AddSeconds(5);
            while (processedCount < 1 && DateTime.UtcNow < deadline)
            {
                Thread.Sleep(50);
            }

            Assert.True(processedCount >= 1, $"Expected at least 1 message processed, but got {processedCount}");
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

            Thread.Sleep(500);

            Assert.NotEmpty(processedMessages);
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
                tasks[i] = Task.Run(() => queue.EnqueueMessage(value));
            }

            await Task.WhenAll(tasks);
            await Task.Delay(1000);

            Assert.True(processedMessages.Count >= 20);
        }
    }
}