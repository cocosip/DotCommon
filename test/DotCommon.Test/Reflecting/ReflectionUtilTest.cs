using DotCommon.Reflecting;
using System;
using System.Reflection;
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

        [Fact]
        public void GetValueByPath_Test()
        {
            var o1 = new ReflectionUtilClass2()
            {
                Id = "1000",
                UserName = "zhangsan",
                Age = 18
            };

            var v1 = ReflectionUtil.GetValueByPath(o1, typeof(ReflectionUtilClass2), "Age");
            Assert.Equal(18, Convert.ToInt32(v1));
            var v2 = ReflectionUtil.GetValueByPath(o1, typeof(ReflectionUtilClass2), "DotCommon.Test.Reflecting.ReflectionUtilClass2.Age");
            Assert.Equal(18, Convert.ToInt32(v2));
            var v3 = ReflectionUtil.GetValueByPath(o1, typeof(ReflectionUtilClass2), "Id");
            Assert.Equal("1000", v3.ToString());

        }

        [Fact]
        public void SetValueByPath_Test()
        {
            var o1 = new ReflectionUtilClass2()
            {
                Id = "1000",
                UserName = "zhangsan",
                Age = 18,
                Class1 = new ReflectionUtilClass1()
                {
                    UserName = "Ku"
                }
            };
            Assert.Equal("1000", o1.Id);
            Assert.Equal("zhangsan", o1.UserName);
            Assert.Equal(18, o1.Age);
            ReflectionUtil.SetValueByPath(o1, typeof(ReflectionUtilClass2), "Id", "3");
            Assert.Equal("3", o1.Id);
            ReflectionUtil.SetValueByPath(o1, typeof(ReflectionUtilClass2), "DotCommon.Test.Reflecting.ReflectionUtilClass2.UserName", "lisi");
            Assert.Equal("lisi", o1.UserName);


            ReflectionUtil.SetValueByPath(o1, typeof(ReflectionUtilClass2), "Class1.UserName", "123");
            Assert.Equal("123", o1.Class1.UserName);
        }

        [Fact]
        public void GetPropertyByPath_Test()
        {
            var o1 = new ReflectionUtilClass2()
            {
                Id = "1000",
                UserName = "zhangsan",
                Age = 18
            };

            var p1 = ReflectionUtil.GetPropertyByPath(o1, typeof(ReflectionUtilClass2), "Id");
            Assert.Equal(typeof(ReflectionUtilClass2).GetProperty("Id"), p1);
            var p2 = ReflectionUtil.GetPropertyByPath(o1, typeof(ReflectionUtilClass2), "DotCommon.Test.Reflecting.ReflectionUtilClass2.Id");
            Assert.Equal(typeof(ReflectionUtilClass2).GetProperty("Id"), p2);
            var p3 = ReflectionUtil.GetPropertyByPath(o1, typeof(ReflectionUtilClass2), "UserName");
            Assert.Equal(typeof(ReflectionUtilClass2).GetProperty("UserName"), p3);
            var p4 = ReflectionUtil.GetPropertyByPath(o1, typeof(ReflectionUtilClass2), "Age");
            Assert.Equal(typeof(ReflectionUtilClass2).GetProperty("Age"), p4);
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

    class ReflectionUtilClass2
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public int Age { get; set; }

        public ReflectionUtilClass1 Class1 { get; set; }
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
