using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class MoneyUtilTest
    {
        [Fact]
        public void GetRmb_Test()
        {
            var expected1 = "壹佰玖拾玖元捌角伍分";
            var actual1 = MoneyUtil.GetRmb(199.85M);
            Assert.Equal(expected1, actual1);

            var expected2 = "玖拾捌亿零伍仟元整";
            var actual2 = MoneyUtil.GetRmb(9800005000M);
            Assert.Equal(expected2, actual2);

            var expected3 = "玖拾万零捌仟捌佰捌拾捌元贰角整";
            var actual3 = MoneyUtil.GetRmb("908888.2");
            Assert.Equal(expected3, actual3);

            var expected4 = "玖万元整";
            var actual4 = MoneyUtil.GetRmb(90000);
            Assert.Equal(expected4, actual4);

            var expected5 = "贰拾万零伍拾元整";
            var actual5 = MoneyUtil.GetRmb(200050);
            Assert.Equal(expected5, actual5);

            Assert.Equal("零元整", MoneyUtil.GetRmb(0M));
            Assert.Equal("溢出", MoneyUtil.GetRmb(1234567890123456M));

        }
    }
}
