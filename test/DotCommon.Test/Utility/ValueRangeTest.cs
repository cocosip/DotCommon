using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class ValueRangeTest
    {
        [Fact]
        public void ValueRange_Test()
        {
            var range = new ValueRange<int>(5, 10);
            Assert.Equal(5, range.MinValue);
            Assert.Equal(10, range.MaxValue);
            Assert.True(range.Contains(10));
            Assert.True(range.Contains(7));
            range.Join(3);
            Assert.Equal(3, range.MinValue);
            Assert.Equal(10, range.MaxValue);
            range.Join(15);
            Assert.Equal(15, range.MaxValue);

        }

        [Fact]
        public void DateRange_Test()
        {
            var minDate = DateTime.Parse("2019-01-01");
            var maxDate = DateTime.Parse("2019-12-31");
            var inDate = DateTime.Parse("2019-05-30");
            var outDate = DateTime.Parse("2018-03-05");

            DateRange range = new DateRange(minDate, maxDate);
            Assert.Equal(minDate, range.MinValue);
            Assert.Equal(maxDate, range.MaxValue);
            Assert.True(range.Contains(inDate));
            Assert.False(range.Contains(outDate));
            Assert.Equal("2019-01-01 00:00:00-2019-12-31 00:00:00", range.ToString("yyyy-MM-dd HH:mm:ss"));

            var range2 = new DateRange();
            Assert.Equal("", range2.ToString());


        }

    }
}
