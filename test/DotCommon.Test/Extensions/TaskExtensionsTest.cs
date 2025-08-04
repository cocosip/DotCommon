using System;
using System.Threading.Tasks;
using Xunit;
using Shouldly;

namespace DotCommon.Test.Extensions
{
    public class TaskExtensionsTest
    {
        [Fact]
        public void WaitResult_Should_Return_Result_When_Task_Completes_In_Time()
        {
            var task = Task.FromResult(42);
            var result = task.WaitResult(1000);
            result.ShouldBe(42);
        }

        [Fact]
        public void WaitResult_Should_Return_Default_When_Task_Times_Out()
        {
            var task = Task.Delay(2000).ContinueWith(_ => 42);
            var result = task.WaitResult(100);
            result.ShouldBe(default(int));
        }

        [Fact]
        public async Task TimeoutAfter_Should_Complete_When_Task_Finishes_In_Time()
        {
            var task = Task.Delay(100);
            await task.TimeoutAfter(1000);
        }

        [Fact]
        public async Task TimeoutAfter_Should_Throw_TimeoutException_When_Task_Times_Out()
        {
            var task = Task.Delay(2000);
            await Assert.ThrowsAsync<TimeoutException>(() => task.TimeoutAfter(100));
        }

        [Fact]
        public async Task TimeoutAfter_With_Result_Should_Return_Result_When_Task_Finishes_In_Time()
        {
            var task = Task.Delay(100).ContinueWith(_ => 42);
            var result = await task.TimeoutAfter(1000);
            result.ShouldBe(42);
        }

        [Fact]
        public async Task TimeoutAfter_With_Result_Should_Throw_TimeoutException_When_Task_Times_Out()
        {
            var task = Task.Delay(2000).ContinueWith(_ => 42);
            await Assert.ThrowsAsync<TimeoutException>(() => task.TimeoutAfter(100));
        }
    }
}