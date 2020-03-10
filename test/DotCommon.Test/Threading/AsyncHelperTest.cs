using DotCommon.Threading;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace DotCommon.Test.Threading
{
    public class AsyncHelperTest
    {
        [Fact]
        public void IsAsync_Test()
        {
            var method1 = this.GetType().GetMethod("AsyncMethod1", BindingFlags.NonPublic | BindingFlags.Instance);
            var method2 = this.GetType().GetMethod("SyncMethod1", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.True(AsyncHelper.IsAsync(method1));
            Assert.False(AsyncHelper.IsAsync(method2));
        }

        [Fact]
        public void IsTaskOrTaskOfT_Test()
        {
            var o1 = new Action(() => { });
            Assert.False(AsyncHelper.IsTaskOrTaskOfT(o1.GetType()));

            var o2 = Task.Run(() => { });
            Assert.True(AsyncHelper.IsTaskOrTaskOfT(o2.GetType()));

            var o3 = Task.FromResult<int>(0);
            Assert.True(AsyncHelper.IsTaskOrTaskOfT(o3.GetType()));

            var o4 = new Func<int>(() => { return 1; });
            Assert.False(AsyncHelper.IsTaskOrTaskOfT(o4.GetType()));


        }

        [Fact]
        public void RunSync_Test()
        {

            var r1 = AsyncHelper.RunSync(() =>
            {
                return Task.FromResult(1);
            });

            Assert.Equal(1, r1);
            var i = 1;
            AsyncHelper.RunSync(() =>
            {
                return Task.Run(() =>
                {
                    Interlocked.Increment(ref i);
                });
            });
            Assert.Equal(2, i);

        }

        [Fact]
        public void UnwrapTask_Test()
        {
            var type1 = typeof(Task);
            Assert.Equal(typeof(void), AsyncHelper.UnwrapTask(type1));
            var type2 = typeof(Task<int>);
            Assert.Equal(typeof(int), AsyncHelper.UnwrapTask(type2));

            var type3 = typeof(List<int>);
            Assert.Equal(typeof(List<int>), AsyncHelper.UnwrapTask(type3));

            var type4 = typeof(string);
            Assert.Equal(typeof(string), AsyncHelper.UnwrapTask(type4));


        }


        private void SyncMethod1()
        {

        }

        private Task AsyncMethod1()
        {
            return Task.FromResult(1);
        }


    }
}
