using System;
using System.Linq;
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

        [Fact]
        public void GetAttributesOfMemberAndDeclaringType_Test()
        {
            var objects1 = ReflectionUtil.GetAttributesOfMemberAndDeclaringType(typeof(ReflectionUtilClass3).GetMember("Id").FirstOrDefault(), true);
            Assert.Single(objects1);

            var objects2 = ReflectionUtil.GetAttributesOfMemberAndDeclaringType(typeof(ReflectionUtilClass4).GetMember("Age").FirstOrDefault(), true);
            Assert.Equal(2, objects2.Count);

        }

        [Fact]
        public void GetAttributesOfMemberAndType_Test()
        {
            var objects1 = ReflectionUtil.GetAttributesOfMemberAndType(typeof(ReflectionUtilClass4).GetMember("Id").FirstOrDefault(), typeof(ReflectionUtilClass4), true);
            Assert.Equal(2, objects1.Count);

            var objects2 = ReflectionUtil.GetAttributesOfMemberAndType(typeof(ReflectionUtilClass4).GetMember("Name").FirstOrDefault(), typeof(ReflectionUtilClass4), true);
            Assert.Equal(2, objects2.Count);

            var objects3 = ReflectionUtil.GetAttributesOfMemberAndType(typeof(ReflectionUtilClass4).GetMember("Number").FirstOrDefault(), typeof(ReflectionUtilClass4), true);
            Assert.Single(objects3);

            var objects4 = ReflectionUtil.GetAttributesOfMemberAndType(typeof(ReflectionUtilClass4).GetMember("Age").FirstOrDefault(), typeof(ReflectionUtilClass4), true);
            Assert.Equal(2, objects4.Count);
        }

        [Fact]
        public void GetAttributesOfMemberAndDeclaringType_Strong_Test()
        {
            var attributes1 = ReflectionUtil.GetAttributesOfMemberAndDeclaringType<ReflectionUtil1Attribute>(typeof(ReflectionUtilClass3).GetMember("Id").FirstOrDefault(), true);
            Assert.Single(attributes1);

            var attributes2 = ReflectionUtil.GetAttributesOfMemberAndDeclaringType<ReflectionUtil1Attribute>(typeof(ReflectionUtilClass3).GetMember("Name").FirstOrDefault(), true);
            Assert.Empty(attributes2);

            var attributes3 = ReflectionUtil.GetAttributesOfMemberAndDeclaringType<ReflectionUtil2Attribute>(typeof(ReflectionUtilClass4).GetMember("Name").FirstOrDefault(), true);
            Assert.Single(attributes3);

            var attributes4 = ReflectionUtil.GetAttributesOfMemberAndDeclaringType<ReflectionUtil3Attribute>(typeof(ReflectionUtilClass4).GetMember("Age").FirstOrDefault(), true);
            Assert.Single(attributes4);

        }

        [Fact]
        public void GetAttributesOfMemberAndType_Strong_Test()
        {
            var attributes1 = ReflectionUtil.GetAttributesOfMemberAndType<ReflectionUtil1Attribute>(typeof(ReflectionUtilClass3).GetMember("Id").FirstOrDefault(), typeof(ReflectionUtilClass3), true);
            Assert.Single(attributes1);

            var attributes2 = ReflectionUtil.GetAttributesOfMemberAndType<ReflectionUtil1Attribute>(typeof(ReflectionUtilClass3).GetMember("Name").FirstOrDefault(), typeof(ReflectionUtilClass3), true);
            Assert.Empty(attributes2);

            var attributes3 = ReflectionUtil.GetAttributesOfMemberAndType<ReflectionUtil2Attribute>(typeof(ReflectionUtilClass3).GetMember("Name").FirstOrDefault(), typeof(ReflectionUtilClass3), true);
            Assert.Single(attributes3);

            var attributes4 = ReflectionUtil.GetAttributesOfMemberAndType<ReflectionUtil2Attribute>(typeof(ReflectionUtilClass3).GetMember("Number").FirstOrDefault(), typeof(ReflectionUtilClass3), true);
            Assert.Empty(attributes4);

            var attributes5 = ReflectionUtil.GetAttributesOfMemberAndType<ReflectionUtil2Attribute>(typeof(ReflectionUtilClass4).GetMember("Name").FirstOrDefault(), typeof(ReflectionUtilClass4), true);
            Assert.Single(attributes5);

            var attributes6 = ReflectionUtil.GetAttributesOfMemberAndType<ReflectionUtil3Attribute>(typeof(ReflectionUtilClass4).GetMember("Name").FirstOrDefault(), typeof(ReflectionUtilClass4), true);
            Assert.Single(attributes6);

            var attributes7 = ReflectionUtil.GetAttributesOfMemberAndType<ReflectionUtil3Attribute>(typeof(ReflectionUtilClass3).GetMember("Name").FirstOrDefault(), typeof(ReflectionUtilClass3), true);
            Assert.Empty(attributes7);
        }

        [Fact]
        public void GetSingleAttributeOfMemberOrDeclaringTypeOrDefault_Test()
        {
            var attribute1 = ReflectionUtil.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<ReflectionUtil4Attribute>(typeof(ReflectionUtilClass5).GetMember("Id").FirstOrDefault(), new ReflectionUtil4Attribute() { Order = 10 }, true);

            Assert.Equal(100, attribute1.Order);

            var attribute2 = ReflectionUtil.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<ReflectionUtil4Attribute>(typeof(ReflectionUtilClass5).GetMember("Name").FirstOrDefault(), new ReflectionUtil4Attribute() { Order = 10 }, true);

            Assert.Equal(10, attribute2.Order);

            var attribute3 = ReflectionUtil.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<ReflectionUtil4Attribute>(typeof(ReflectionUtilClass6).GetMember("Id").FirstOrDefault(), new ReflectionUtil4Attribute() { Order = 10 }, true);

            Assert.Equal(50, attribute3.Order);

            var attribute4 = ReflectionUtil.GetSingleAttributeOfMemberOrDeclaringTypeOrDefault<ReflectionUtil4Attribute>(typeof(ReflectionUtilClass6).GetMember("Name").FirstOrDefault(), new ReflectionUtil4Attribute() { Order = 10 }, true);

            Assert.Equal(300, attribute4.Order);
        }

        [Fact]
        public void GetSingleAttributeOrDefault_Test()
        {
            var attribute1 = ReflectionUtil.GetSingleAttributeOrDefault<ReflectionUtil4Attribute>(typeof(ReflectionUtilClass5).GetMember("Id").FirstOrDefault(), new ReflectionUtil4Attribute() { Order = 10 }, true);
            Assert.Equal(100, attribute1.Order);
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


    class ReflectionUtilClass3
    {
        [ReflectionUtil1]
        public int Id { get; set; }

        [ReflectionUtil2]
        public string Name { get; set; }


        public string Number { get; set; }

    }

    [ReflectionUtil3Attribute]

    class ReflectionUtilClass4 : ReflectionUtilClass3
    {
        public string NickName { get; set; }

        [ReflectionUtil2Attribute]
        public string Age { get; set; }
    }

    class ReflectionUtilClass5
    {
        [ReflectionUtil4(Order = 100)]
        public string Id { get; set; }

        public string Name { get; set; }
    }

    [ReflectionUtil4(Order = 300)]
    class ReflectionUtilClass6
    {
        [ReflectionUtil4(Order = 50)]
        public string Id { get; set; }

        public string Name { get; set; }
    }

    class ReflectionUtil1Attribute : Attribute
    {

    }

    class ReflectionUtil2Attribute : Attribute
    {

    }
    class ReflectionUtil3Attribute : Attribute
    {

    }

    class ReflectionUtil4Attribute : Attribute
    {
        public int Order { get; set; }

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
