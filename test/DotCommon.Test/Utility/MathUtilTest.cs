using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class MathUtilTest
    {

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
