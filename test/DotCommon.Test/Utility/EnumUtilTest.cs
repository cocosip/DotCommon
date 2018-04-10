using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class EnumUtilTest
    {
        [Fact]
        public void GetEnumItemsTest()
        {
            var list = EnumUtil.GetEnumItems<TestEnum>();
            Assert.Equal("TE1", list["1"]);
            Assert.Equal("这是E2", list["2"]);
            Assert.Equal("E3Test", list["3"]);
            Assert.Equal("E4", list["6"]);
            Assert.Equal("TE5", list["7"]);
        }


        [Fact]
        public void GetEnumDescription()
        {
            var e1 = EnumUtil.GetEnumDescription(TestEnum.E1);
            var e2 = EnumUtil.GetEnumDescription(TestEnum.E2);
            var e3 = EnumUtil.GetEnumDescription(TestEnum.E3);
            var e4 = EnumUtil.GetEnumDescription(TestEnum.E4);
            var e5 = EnumUtil.GetEnumDescription(TestEnum.E5);
            Assert.Equal("TE1", e1);
            Assert.Equal("这是E2", e2);
            Assert.Equal("E3Test", e3);
            Assert.Equal("E4", e4);
            Assert.Equal("TE5", e5);
        }

    }

    public enum TestEnum
    {
        [Description("TE1")]
        E1 = 1,
        [Description("这是E2")]
        E2,
        [Description("E3Test")]
        E3,
        E4 = 6,
        [Description("TE5")]
        E5

    }
}
