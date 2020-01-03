using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using DotCommon.Extensions;

namespace DotCommon.Test.Extensions
{
    public class ListExtensionsTest
    {
        [Fact]
        public void Remove_Test()
        {
            var list1 = new List<int>();
            list1.Add(1);
            list1.Add(2);
            list1.Add(3);
            list1.Add(4);
            list1.Add(100);
            list1.Add(200);

            Assert.True(list1.Remove(x => x == 1));

            Assert.Equal(5, list1.Count);
            Assert.DoesNotContain(1, list1);

        }

    }
}
