﻿using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotCommon.Test.Extensions
{
    public class TaskExtensionsTest
    {


        [Fact]
        public void WaitResult_Test()
        {
            Task<int> t1 = Task.FromResult(1);
            var v1 = t1.WaitResult(10);
            Assert.Equal(1, v1);

            Task<int> t2 = Task.Run(() =>
            {
                Thread.Sleep(100);
                return 2;
            });

            var v2 = t2.WaitResult(1);
            Assert.Equal(0, v2);
        }

        [Fact]
        public async Task TimeoutAfter_Test()
        {
            var t1 = Task.Run(() =>
            {
                return Task.FromResult(1);
            });

            Assert.Equal(1, await t1);


            Task t2 = Task.Run(() =>
            {
                Thread.Sleep(100);
            });

            var aggregateException = await Assert.ThrowsAsync<TimeoutException>(() =>
            {
                return t2.TimeoutAfter(2);
            });

            var t3 = Task.Run<int>(() =>
            {
                Thread.Sleep(100);
                return 1;
            });

            var aggregateException2 = await Assert.ThrowsAsync<TimeoutException>(() =>
            {
                return t3.TimeoutAfter<int>(1);
            });

        }

    }
}
