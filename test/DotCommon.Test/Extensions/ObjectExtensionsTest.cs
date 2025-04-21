using System;
using System.Collections.Generic;
using Xunit;

namespace DotCommon.Test.Extensions
{
    public class ObjectExtensionsTest
    {
        [Fact]
        public void As_To_Test()
        {
            object o = new TestAsClass()
            {
                Id = 1,
                Name = "zhangsan"
            };
            var v1 = o.As<TestAsClass>();
            Assert.Equal(typeof(TestAsClass), v1.GetType());

            object o2 = new TestAsStruct()
            {
                Id = 1
            };

            var v2 = o2.To<TestAsStruct>();
            Assert.Equal(typeof(TestAsStruct), v2.GetType());
        }

        [Fact]
        public void IsIn_Test()
        {
            var list1 = new List<int>();
            list1.Add(1);
            list1.Add(3);
            list1.Add(5);

            var a1 = 1;
            var v1 = a1.IsIn(list1.ToArray());
            Assert.True(v1);

            var a2 = 15;
            var v2 = a2.IsIn(list1.ToArray());
            Assert.False(v2);

        }



    }

    struct TestAsStruct
    {
        public int Id { get; set; }
    }

    class TestAsClass
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
