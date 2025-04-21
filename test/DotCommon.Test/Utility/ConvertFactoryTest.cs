using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class ConvertFactoryTest
    {
        [Fact]
        public void ToInt32_Test()
        {
            var r1 = ConvertUtil.ToInt32("15", 3);
            var r2 = ConvertUtil.ToInt32("13w", 11);

            Assert.Equal(15, r1);
            Assert.Equal(11, r2);
        }

        [Fact]
        public void ToInt64_Test()
        {
            var r1 = ConvertUtil.ToInt64("999", 1);
            var r2 = ConvertUtil.ToInt64("qweqwex", 2);

            Assert.Equal(999, r1);
            Assert.Equal(2, r2);
        }

        [Fact]
        public void ToDouble_Test()
        {
            var r1 = ConvertUtil.ToDouble("252.11", 9.9d);
            var r2 = ConvertUtil.ToDouble("22 2232 ", 11.8d);

            Assert.Equal(252.11d, r1);
            Assert.Equal(11.8d, r2);

            var r3 = ConvertUtil.ToDouble("995.5", 12d, 3);
            Assert.Equal(995.500, r3);

            var r4 = ConvertUtil.ToDouble("23.s", 15.65, 1);
            Assert.Equal(15.6, r4);

        }

        [Fact]
        public void ToDateTime_Test()
        {
            var d1 = ConvertUtil.ToDateTime("2019-01-02", new DateTime(2019, 5, 3));
            Assert.Equal(new DateTime(2019, 1, 2), d1);

            var d2 = ConvertUtil.ToDateTime("2018-08-92s", new DateTime(2010, 1, 1));
            Assert.Equal(new DateTime(2010, 1, 1), d2);

        }

        [Fact]
        public void ToBool_Test()
        {
            var r1 = ConvertUtil.ToBool("false", true);
            Assert.False(r1);
            var r2 = ConvertUtil.ToBool("s", false);
            Assert.False(r2);
        }

        [Fact]
        public void ToGuid_Test()
        {
            var source = "6F9619FF-8B86-D011-B42D-00C04FC964FF";
            var g1 = ConvertUtil.ToGuid(source);
            Assert.Equal(source, g1.ToString().ToUpper());
        }

    }
}
