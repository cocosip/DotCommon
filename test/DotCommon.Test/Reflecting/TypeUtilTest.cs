using DotCommon.Reflecting;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Reflecting
{
    public class TypeUtilTest
    {
        [Fact]
        public void IsFunc_Test()
        {
            Func<int> func1 = () =>
            {
                return 1;
            };
            Func<string> func2 = null;

            Func<DateTime> func3 = () =>
            {
                return DateTime.Now;
            };

            Assert.True(TypeUtil.IsFunc(func1));
            Assert.False(TypeUtil.IsFunc(func2));
            Assert.True(TypeUtil.IsFunc(func3));
            Assert.True(TypeUtil.IsFunc<int>(func1));

        }
    }
}
