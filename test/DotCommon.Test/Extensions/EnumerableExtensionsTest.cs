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
        public void Contains_Test()
        {
            var list1 = new List<int>();
            list1.Add(101);
            list1.Add(103);
            list1.Add(105);
            list1.Add(107);

            var v1 = list1.Contains(x => x == 3);
            Assert.False(v1);

            var v2 = list1.Contains(x => x == 101);
            Assert.True(v2);

        }

        [Fact]
        public void Safe_Test()
        {
            List<int> list1 = null;
            var v1 = list1.Safe();
            Assert.NotNull(v1);
        }

        [Fact]
        public void IsEmpty_Test()
        {
            var list1 = new List<string>();
            var v1 = list1.IsEmpty();
            Assert.True(v1);

            list1.Add("1");
            list1.Add("100");
            list1.Add("300");
            var v2 = list1.IsEmpty();
            Assert.False(v2);
            var v3 = list1.IsNotEmpty();
            Assert.True(v3);


            list1 = null;
            var v4 = list1.IsEmpty();
            Assert.True(v4);

        }


    }
}
