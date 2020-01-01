using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DotCommon.Extensions;
using System.Linq;

namespace DotCommon.Test.Extensions
{
    public class EnumerableExtensionsTest
    {
        [Fact]
        public void JoinAsString_Test()
        {
            var list1 = new List<string>();
            list1.Add("1");
            list1.Add("2");
            list1.Add("cc");
            var v1 = list1.JoinAsString(",");
            var v2 = list1.JoinAsString("|");
            Assert.Equal("1,2,cc", v1);
            Assert.Equal("1|2|cc", v2);

            var list2 = new List<int>();
            list2.Add(100);
            list2.Add(200);
            var v3 = list2.JoinAsString("@");
            Assert.Equal("100@200", v3);
        }

        [Fact]
        public void WhereIf_Test()
        {
            var list1 = new List<string>();
            list1.Add("1");
            list1.Add("100");
            list1.Add("200");

            var v1 = list1.WhereIf(true, x => x == "100" || x == "200");
            Assert.Equal(2, v1.Count());

            var v2 = list1.WhereIf(false, x => x == "1");
            Assert.Equal(3, v2.Count());

            var v3 = list1.WhereIf(true, (x, y) => x == "1" && y == 0);
            Assert.Single(v3);

            var v4 = list1.WhereIf(false, (x, y) => x == "100" || x == "101" && y > 0);
            Assert.Equal(3, v4.Count());
        }

        [Fact]
        public void ForEach_Test()
        {
            var list1 = new List<int>();
            list1.Add(101);
            list1.Add(103);
            list1.Add(105);
            list1.Add(107);

            var list2 = new List<int>();

            list1.ForEach(x =>
            {
                list2.Add(x);
            });

            Assert.Equal(4, list2.Count);
            Assert.Contains(101, list2);
            Assert.Contains(103, list2);
            Assert.Contains(105, list2);
            Assert.Contains(107, list2);

            var list3 = list1.Safe();
            Assert.Equal(4, list3.Count());

            list1 = null;
            var list4 = list1.Safe();
            Assert.Empty(list4);


        }


    }
}
