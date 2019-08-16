using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class BufferQueueTest
    {
        [Fact]
        public void BufferQueue_Test()
        {
            var r = 0;
            var queue = new BufferQueue<int>("q1", 2, v =>
            {
                r = v;
            });
            queue.EnqueueMessage(1);
            queue.EnqueueMessage(2);
            queue.EnqueueMessage(3);
            queue.EnqueueMessage(4);
            queue.EnqueueMessage(5);
       
            Assert.True(r > 0);

        }
    }
}
