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
            var dict1 = EnumUtil.GetEnumItems<TestEnum>(true);
            Assert.Equal("TE1", dict1["1"]);
            Assert.Equal("这是E2", dict1["2"]);
            Assert.Equal("E3Test", dict1["3"]);
            Assert.Equal("E4", dict1["6"]);
            Assert.Equal("TE5", dict1["7"]);

            var dict2 = EnumUtil.GetEnumItems<TestEnum>(false);
            Assert.Equal("TE1", dict2["E1"]);
            Assert.Equal("这是E2", dict2["E2"]);
            Assert.Equal("E3Test", dict2["E3"]);
            Assert.Equal("E4", dict2["E4"]);
            Assert.Equal("TE5", dict2["E5"]);
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

            var e21 = EnumUtil.GetEnumDescription(TestEnum2.E21);
            var e22 = EnumUtil.GetEnumDescription(TestEnum2.E22);

            Assert.Equal("E21", e21);
            Assert.Equal("E22", e22);
        }

        [Fact]
        public void ToStr_FromStr_Test()
        {
            Assert.Equal("E1", EnumUtil.ToStr(TestEnum.E1));
            Assert.Equal("E2", EnumUtil.ToStr(TestEnum.E2));

            Assert.Equal(TestEnum.E3, EnumUtil.FromStr<TestEnum>("E3"));
            Assert.Equal(TestEnum.E4, EnumUtil.FromStr<TestEnum>("E4"));

            Assert.Equal(5, EnumUtil.GetEnumCount<TestEnum>());

            var array = EnumUtil.GetValues<TestEnum>();
            Assert.Equal(5, array.Length);

            Assert.True(EnumUtil.InEnum<TestEnum>(1));

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

    public enum TestEnum2
    {
        E21,
        E22
    }
}
