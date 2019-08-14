using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class RandomUtilTest
    {
        [Fact]
        public void GetRandomSeed_Test()
        {
            var seed1 = RandomUtil.GetRandomSeed(8);
            Assert.True(seed1 != 0);
        }

        [Fact]
        public void GetRandomInt_Test()
        {
            var r = RandomUtil.GetRandomInt(1, 9999);
            Assert.True(r >= 1);
            Assert.True(r <= 99999);
        }

        [Fact]
        public void GetRandomStr_Test()
        {
            var s1 = RandomUtil.GetRandomStr(5, RandomStringType.Fix);
            Assert.Equal(5, s1.Length);
            var matchs1 = Regex.IsMatch(s1, @"^[A-Za-z0-9]{5}$");
            Assert.True(matchs1);

            var s2 = RandomUtil.GetRandomStr(10, RandomStringType.NumberLower);
            var matchs2 = Regex.IsMatch(s2, @"^[a-z0-9]{10}$");
            Assert.True(matchs2);
        }

        [Fact]
        public void GetRandomArray_Test()
        {
            var constArray = new int[] { 1, 2, 3 };
            var array = new int[] { 1, 2, 3 };
            RandomUtil.GetRandomArray(array);
            Assert.Equal(3, array.Length);
            Assert.Equal(constArray, array.OrderBy(x => x).ToArray());
        }

    }
}
