using DotCommon.Reflecting;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    public class TypeUtilTest
    {
        [Fact]
        public void IsFunc_Test()
        {
            Func<int> func1 = () =>
            {
                return 1;
            };
            Func<string> func2 = null;

            Func<DateTime> func3 = () =>
            {
                return DateTime.Now;
            };


            Action<int> action1 = a => { };

            Assert.True(TypeUtil.IsFunc(func1));
            Assert.False(TypeUtil.IsFunc(func2));
            Assert.True(TypeUtil.IsFunc(func3));
            Assert.True(TypeUtil.IsFunc<int>(func1));
            Assert.False(TypeUtil.IsFunc(action1));
        }

        [Fact]
        public void IsAsync_Test()
        {
            var method1 = this.GetType().GetMethod("AsyncMethod1", BindingFlags.NonPublic | BindingFlags.Instance);
            var method2 = this.GetType().GetMethod("SyncMethod1", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.True(TypeUtil.IsAsync(method1));
            Assert.False(TypeUtil.IsAsync(method2));
        }

        private void SyncMethod1()
        {

        }

        private Task AsyncMethod1()
        {
            return Task.FromResult(1);
        }


       

    }

    public class TypeUtilClass1
    {

    }

    public static class TypeUtilClass1Extension
    {
        public static void Method1(this TypeUtilClass1 typeUtilClass1)
        {

        }
    }
}
