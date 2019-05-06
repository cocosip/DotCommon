using DotCommon.Reflecting;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    public class ReflectionUtilTest
    {
        [Fact]
        public void IsAssignableToGenericType_Test()
        {
            var t3Type = typeof(GenericClassTest3);
            Assert.True(ReflectionUtil.IsAssignableToGenericType(t3Type, typeof(ITestGenericInterface<>)));

            var t4Type = typeof(GenericClassTest4);
            Assert.True(ReflectionUtil.IsAssignableToGenericType(t4Type, typeof(TestGenericClass<>)));
        }

    }

    interface ITestGenericInterface<T>
    {

    }

    class TestGenericClass<T>
    {

    }
    class GenericClassT
    {

    }
    class GenericClassTest1<T> : ITestGenericInterface<T>
    {

    }

    class GenericClassTest2<T> : TestGenericClass<T>
    {

    }

    class GenericClassTest3 : GenericClassTest1<GenericClassT>
    {

    }

    class GenericClassTest4 : GenericClassTest2<GenericClassT>
    {

    }

}
