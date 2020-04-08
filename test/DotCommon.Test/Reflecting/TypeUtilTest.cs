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

            Action action2 = () => { };

            Assert.True(TypeUtil.IsFunc(func1));
            Assert.True(TypeUtil.IsFunc<int>(func1));
            Assert.False(TypeUtil.IsFunc<string>(func1));
            Assert.False(TypeUtil.IsFunc(func2));
            Assert.True(TypeUtil.IsFunc(func3));
            Assert.False(TypeUtil.IsFunc(action1));
            Assert.False(TypeUtil.IsFunc(action2));
            Assert.False(TypeUtil.IsFunc(null));
        }

        [Fact]
        public void IsAsync_Test()
        {
            var method1 = this.GetType().GetMethod("AsyncMethod1", BindingFlags.NonPublic | BindingFlags.Instance);
            var method2 = this.GetType().GetMethod("SyncMethod1", BindingFlags.NonPublic | BindingFlags.Instance);

            Assert.True(method1.IsAsync());
            Assert.False(method2.IsAsync());
        }

        [Fact]
        public void IsTaskOrTaskOfT_Test()
        {
            var o1 = new Action(() => { });
            Assert.False(o1.GetType().IsTaskOrTaskOfT());

            var o2 = Task.Run(() => { });
            Assert.True(o2.GetType().IsTaskOrTaskOfT());

            var o3 = Task.FromResult<int>(0);
            Assert.True(o3.GetType().IsTaskOrTaskOfT());

            var o4 = new Func<int>(() => { return 1; });
            Assert.False(o4.GetType().IsTaskOrTaskOfT());


        }

        [Fact]
        public void GetFirstGenericArgumentIfNullable_Test()
        {
            var t1 = TypeUtil.GetFirstGenericArgumentIfNullable(typeof(GenericTypeClass1));
            Assert.Equal(typeof(GenericTypeClass1), t1);

            var o2 = new GenericTypeClass2<int>();
            var t2 = TypeUtil.GetFirstGenericArgumentIfNullable(o2.GetType());
            Assert.Equal(typeof(GenericTypeClass2<int>), t2);

            var t3 = TypeUtil.GetFirstGenericArgumentIfNullable(typeof(int?));
            Assert.Equal(typeof(int), t3);

            var t4 = TypeUtil.GetFirstGenericArgumentIfNullable(typeof(DateTime?));
            Assert.Equal(typeof(DateTime), t4);

            var t5 = TypeUtil.GetFirstGenericArgumentIfNullable(null);
            Assert.Equal(default, t5);
        }

        [Fact]
        public void IsPrimitiveExtended_Test()
        {
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(int), includeEnums: true));
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(Enum1), includeEnums: true));
            Assert.False(TypeUtil.IsPrimitiveExtended(typeof(Enum1), includeEnums: false));
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(decimal), includeEnums: false));
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(double), includeEnums: false));
            Assert.True(TypeUtil.IsPrimitiveExtended(typeof(int?),true, includeEnums: false));
            Assert.False(TypeUtil.IsPrimitiveExtended(typeof(int?), false, includeEnums: false));
        }


        enum Enum1
        {
            Test1,
            Test2
        }

        class GenericTypeClass1
        {

        }
        class GenericTypeClass2<T1>
        {

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
