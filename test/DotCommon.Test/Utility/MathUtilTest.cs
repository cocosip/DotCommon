using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class MathUtilTest
    {
        /// <summary>差值最小
        /// </summary>
        [Fact]
        public void GetMinMinus_Test()
        {
            var intArray = new int[] { 6, 1, 4, 555, 18, 33 };
            var intValue = MathUtil.GetMinMinus(intArray.ToList(), x => x);
            Assert.Equal((4, 6), intValue);

            var intTwo = MathUtil.GetMinMinus(new List<int>() { 1, 2 }, x => x);
            Assert.Equal((1, 2), intTwo);
            Assert.Equal(default, MathUtil.GetMinMinus(new List<int>() { }, x => x));

            //decimal
            var decimalArray = new decimal[] { 3.5M, 10.8M, 1.3M, 1.8M, 22M, 8.5M };
            var decimalValue = MathUtil.GetMinMinus(decimalArray.ToList(), x => x);
            Assert.Equal((1.3M, 1.8M), decimalValue);

            var decimalTwo = MathUtil.GetMinMinus(new List<decimal>() { 1.3M, 2.8M }, x => x);
            Assert.Equal((1.3M, 2.8M), decimalTwo);
            Assert.Equal(default, MathUtil.GetMinMinus(new List<decimal>() { }, x => x));

            //double
            var doubleArray = new double[] { 1.444, 2.533, 33.2, 15.4, 15.41 };
            var doubleValue = MathUtil.GetMinMinus(doubleArray.ToList(), x => x);
            Assert.Equal((15.4, 15.41), doubleValue);

            var doubleTwo = MathUtil.GetMinMinus(new List<double>() { 1.24, 254.2 }, x => x);
            Assert.Equal((1.24, 254.2), doubleTwo);
            Assert.Equal(default, MathUtil.GetMinMinus(new List<double>() { }, x => x));

            //float
            var floatArray = new float[] { 22f, 18.2f, 19.1f, 5f, 153f };
            var floatValue = MathUtil.GetMinMinus(floatArray.ToList(), x => x);
            Assert.Equal((18.2f, 19.1f), floatValue);

            var floatTwo = MathUtil.GetMinMinus(new List<float>() { 1.4f, 4.2f }, x => x);
            Assert.Equal((1.4f, 4.2f), floatTwo);
            Assert.Equal(default, MathUtil.GetMinMinus(new List<float>() { }, x => x));

            //long
            var longArray = new long[] { 888L, 777L, 5L, 8L, 233L, 15L, 95L, 11L };
            var longValue = MathUtil.GetMinMinus(longArray.ToList(), x => x);
            Assert.Equal((5L, 8L), longValue);

            var longTwo = MathUtil.GetMinMinus(new List<long>() { 9L, 22L }, x => x);
            Assert.Equal((9L, 22L), longTwo);
            Assert.Equal(default, MathUtil.GetMinMinus(new List<long>() { }, x => x));
        }

        /// <summary>笛卡尔积
        /// </summary>
        [Fact]
        public void DescartesTest()
        {
            var list1 = new List<string>()
            {
               "A","B" ,"C"
            };

            var list2 = new List<string>()
            {
                "1","2"
            };

            var list3 = new List<string>()
            {
                "Y"
            };

            var r = MathUtil.Descartes(list1, list2, list3);
            Assert.Equal(6, r.Count);

        }
    }
}
