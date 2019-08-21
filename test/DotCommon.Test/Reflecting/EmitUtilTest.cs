using DotCommon.Reflecting;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    public class EmitUtilTest
    {
        [Fact]
        public void IsNullable_Test()
        {
            Assert.True(EmitUtil.IsNullable(typeof(int?)));
            Assert.False(EmitUtil.IsNullable(typeof(string)));
        }

        [Fact]
        public void GetNullableArg0_Test()
        {
            Assert.Equal(typeof(int), EmitUtil.GetNullableArg0(typeof(int?)));
            Assert.Equal(typeof(float), EmitUtil.GetNullableArg0(typeof(float?)));
            Assert.Equal(typeof(string), EmitUtil.GetNullableArg0(typeof(string)));
        }
    }
}
