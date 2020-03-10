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

        [Fact]
        public void IsPropertyGetterSetterMethod_Test()
        {
            var method1 = typeof(ReflectionUtilClass1).GetMethod("GetAge");
            Assert.False(ReflectionUtil.IsPropertyGetterSetterMethod(method1, typeof(ReflectionUtilClass1)));

            var method2 = typeof(ReflectionUtilClass1).GetMethod("Do");
            Assert.False(ReflectionUtil.IsPropertyGetterSetterMethod(method2, typeof(ReflectionUtilClass1)));

            var method3 = typeof(ReflectionUtilClass1).GetMethod("get_UserName");
            Assert.True(ReflectionUtil.IsPropertyGetterSetterMethod(method3, typeof(ReflectionUtilClass1)));
 
        }

    }


    class ReflectionUtilClass1
    {
        public string UserName { get; set; }
        public string GetAge()
        {
            return "Age";
        }

        public void Do()
        {

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
