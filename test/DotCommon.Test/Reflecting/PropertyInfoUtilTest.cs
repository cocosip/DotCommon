using DotCommon.Reflecting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    public class PropertyInfoUtilTest
    {
        [Fact]
        public void GetProperties_Test()
        {
            var test1 = new PropertyInfoUtilTestClass1();
            var properties1 = PropertyInfoUtil.GetProperties(test1).ToList();
            Assert.Single(properties1);

            var properties2 = PropertyInfoUtil.GetProperties(test1, System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static).ToList();
            Assert.Single(properties2);
        }


    }
    public class PropertyInfoUtilTestClass1
    {
        public int Id { get; set; }

        public static string Name { get; set; }

     

        private string Number { get; set; }
    }
}
