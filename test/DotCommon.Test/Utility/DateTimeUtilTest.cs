using DotCommon.Utility;
using System;
using System.Collections.Generic;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class DateTimeUtilTest
    {
        [Fact]
        public void ToInt32_FromDateTime_Test()
        {
            var dateTime = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixTimestamp = DateTimeUtil.ToInt32(dateTime);
            Assert.Equal(1672531200, unixTimestamp); // Unix timestamp for 2023-01-01 00:00:00 UTC
        }

        [Fact]
        public void ToInt64_FromDateTime_Test()
        {
            var dateTime = new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var unixTimestampMs = DateTimeUtil.ToInt64(dateTime);
            Assert.Equal(1672531200000, unixTimestampMs); // Unix timestamp in milliseconds for 2023-01-01 00:00:00 UTC
        }

        [Fact]
        public void ToInt32_FromString_Valid_Test()
        {
            string dateTimeString = "2023-01-01 00:00:00";
            // Convert the string to DateTime, then to UTC, then to Unix timestamp
            DateTime parsedDateTime = DateTime.Parse(dateTimeString);
            int expectedUnixTimestamp = DateTimeUtil.ToInt32(parsedDateTime);

            // Call the method under test
            int actualUnixTimestamp = DateTimeUtil.ToInt32(dateTimeString, 0);

            // Compare the Unix timestamps directly
            Assert.Equal(expectedUnixTimestamp, actualUnixTimestamp);
        }

        [Fact]
        public void ToInt32_FromString_Invalid_Test()
        {
            var unixTimestamp = DateTimeUtil.ToInt32("invalid-date", 99);
            Assert.Equal(99, unixTimestamp);
        }

        [Fact]
        public void ToDateTime_FromInt32_Test()
        {
            var unixTimestamp = 1672531200; // Unix timestamp for 2023-01-01 00:00:00 UTC
            var dateTime = DateTimeUtil.ToDateTime(unixTimestamp);
            // The result is converted to local time, so we compare year, month, day, hour, minute, second
            Assert.Equal(2023, dateTime.Year);
            Assert.Equal(1, dateTime.Month);
            Assert.Equal(1, dateTime.Day);
            // Hour will depend on local timezone, so we don't assert it directly
        }

        [Fact]
        public void ToDateTime_FromInt64_Test()
        {
            var unixTimestampMs = 1672531200000; // Unix timestamp in milliseconds for 2023-01-01 00:00:00 UTC
            var dateTime = DateTimeUtil.ToDateTime(unixTimestampMs);
            // The result is converted to local time, so we compare year, month, day, hour, minute, second
            Assert.Equal(2023, dateTime.Year);
            Assert.Equal(1, dateTime.Month);
            Assert.Equal(1, dateTime.Day);
            // Hour will depend on local timezone, so we don't assert it directly
        }

        [Fact]
        public void TruncateToMilliseconds_Test()
        {
            var originalDateTime = new DateTime(2023, 1, 1, 10, 30, 45, 123, 456);
            var truncatedDateTime = originalDateTime.TruncateToMilliseconds();
            Assert.Equal(new DateTime(2023, 1, 1, 10, 30, 45, 123), truncatedDateTime);
        }

        [Fact]
        public void TruncateToSeconds_Test()
        {
            var originalDateTime = new DateTime(2023, 1, 1, 10, 30, 45, 123, 456);
            var truncatedDateTime = originalDateTime.TruncateToSeconds();
            Assert.Equal(new DateTime(2023, 1, 1, 10, 30, 45), truncatedDateTime);
        }

        [Fact]
        public void GetPadDay_Test()
        {
            var dateTime = new DateTime(2023, 1, 15);
            Assert.Equal("20230115", DateTimeUtil.GetPadDay(dateTime));
        }

        [Fact]
        public void GetPadSecond_Test()
        {
            var dateTime = new DateTime(2023, 1, 15, 10, 30, 45);
            Assert.Equal("20230115103045", DateTimeUtil.GetPadSecond(dateTime));
        }

        [Fact]
        public void GetPadSecondWithoutPrefix_Test()
        {
            var dateTime = new DateTime(2023, 1, 15, 10, 30, 45);
            Assert.Equal("230115103045", DateTimeUtil.GetPadSecondWithoutPrefix(dateTime));
        }

        [Fact]
        public void GetPadMillSecond_Test()
        {
            var dateTime = new DateTime(2023, 1, 15, 10, 30, 45, 123);
            Assert.Equal("20230115103045123", DateTimeUtil.GetPadMillSecond(dateTime));
        }

        [Fact]
        public void GetPadMillSecondWithoutPrefix_Test()
        {
            var dateTime = new DateTime(2023, 1, 15, 10, 30, 45, 123);
            Assert.Equal("230115103045123", DateTimeUtil.GetPadMillSecondWithoutPrefix(dateTime));
        }

        [Fact]
        public void GetWeekCross_NormalRange_Test()
        {
            var begin = new DateTime(2023, 1, 1); // Sunday
            var end = new DateTime(2023, 1, 3);   // Tuesday
            Assert.Equal("0,1,2", DateTimeUtil.GetWeekCross(begin, end));
        }

        [Fact]
        public void GetWeekCross_FullWeek_Test()
        {
            var begin = new DateTime(2023, 1, 1); // Sunday
            var end = new DateTime(2023, 1, 7);   // Saturday
            Assert.Equal("0,1,2,3,4,5,6", DateTimeUtil.GetWeekCross(begin, end));
        }

        [Fact]
        public void GetWeekCross_BeginAfterEnd_Test()
        {
            var begin = new DateTime(2023, 1, 3);
            var end = new DateTime(2023, 1, 1);
            Assert.Empty(DateTimeUtil.GetWeekCross(begin, end));
        }

        [Fact]
        public void GetChineseWeekOfDay_Test()
        {
            Assert.Equal("星期日", DateTimeUtil.GetChineseWeekOfDay(new DateTime(2023, 1, 1))); // Sunday
            Assert.Equal("星期一", DateTimeUtil.GetChineseWeekOfDay(new DateTime(2023, 1, 2))); // Monday
        }

        [Fact]
        public void GetWeekDays_Test()
        {
            var weekDays = DateTimeUtil.GetWeekDays();
            Assert.Equal(7, weekDays.Count);
            Assert.Equal("星期日", weekDays[0]);
            Assert.Equal("星期六", weekDays[6]);
        }

        [Fact]
        public void GetFirstDayOfMonth_Test()
        {
            var dateTime = new DateTime(2023, 5, 15);
            Assert.Equal(new DateTime(2023, 5, 1), DateTimeUtil.GetFirstDayOfMonth(dateTime));
        }

        [Fact]
        public void GetLastDayOfMonth_Test()
        {
            var dateTime = new DateTime(2023, 2, 15); // February
            Assert.Equal(new DateTime(2023, 2, 28), DateTimeUtil.GetLastDayOfMonth(dateTime));
            dateTime = new DateTime(2024, 2, 15); // Leap year February
            Assert.Equal(new DateTime(2024, 2, 29), DateTimeUtil.GetLastDayOfMonth(dateTime));
        }

        [Fact]
        public void GetFirstDayOfYear_Test()
        {
            var dateTime = new DateTime(2023, 5, 15);
            Assert.Equal(new DateTime(2023, 1, 1), DateTimeUtil.GetFirstDayOfYear(dateTime));
        }

        [Fact]
        public void GetLastDayOfYear_Test()
        {
            var dateTime = new DateTime(2023, 5, 15);
            Assert.Equal(new DateTime(2023, 12, 31), DateTimeUtil.GetLastDayOfYear(dateTime));
        }

        [Fact]
        public void GetLastSecondOfDay_Test()
        {
            var dateTime = new DateTime(2023, 1, 1, 10, 30, 0);
            Assert.Equal(new DateTime(2023, 1, 1, 23, 59, 59), DateTimeUtil.GetLastSecondOfDay(dateTime));
        }

        [Fact]
        public void IsFirstDayOfMonth_Test()
        {
            Assert.True(DateTimeUtil.IsFirstDayOfMonth(new DateTime(2023, 1, 1)));
            Assert.False(DateTimeUtil.IsFirstDayOfMonth(new DateTime(2023, 1, 2)));
        }

        [Fact]
        public void IsLastDayOfMonth_Test()
        {
            Assert.True(DateTimeUtil.IsLastDayOfMonth(new DateTime(2023, 1, 31)));
            Assert.False(DateTimeUtil.IsLastDayOfMonth(new DateTime(2023, 1, 30)));
            Assert.True(DateTimeUtil.IsLastDayOfMonth(new DateTime(2024, 2, 29))); // Leap year
        }

        [Fact]
        public void IsFirstDayOfYear_Test()
        {
            Assert.True(DateTimeUtil.IsFirstDayOfYear(new DateTime(2023, 1, 1)));
            Assert.False(DateTimeUtil.IsFirstDayOfYear(new DateTime(2023, 1, 2)));
        }

        [Fact]
        public void IsLastDayOfYear_Test()
        {
            Assert.True(DateTimeUtil.IsLastDayOfYear(new DateTime(2023, 12, 31)));
            Assert.False(DateTimeUtil.IsLastDayOfYear(new DateTime(2023, 12, 30)));
        }

        [Fact]
        public void ReplaceDay_Test()
        {
            var originalDateTime = new DateTime(2023, 1, 15, 10, 0, 0);
            var newDateTime = DateTimeUtil.ReplaceDay("05", originalDateTime);
            Assert.Equal(new DateTime(2023, 1, 5), newDateTime);
        }

        [Fact]
        public void ReplaceTime_Test()
        {
            var originalDateTime = new DateTime(2023, 1, 15, 10, 0, 0);
            var newDateTime = DateTimeUtil.ReplaceTime("14:30:00", originalDateTime);
            Assert.Equal(new DateTime(2023, 1, 15, 14, 30, 0), newDateTime);
        }
    }
}