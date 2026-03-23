using System;
using DotCommon.Timing;
using Microsoft.Extensions.Options;
using Xunit;

namespace DotCommon.Test.Timing
{
    public class ClockTest
    {
        private static Clock CreateClock(DateTimeKind kind, string timeZone = null)
        {
            var options = new DotCommonClockOptions { Kind = kind };
            var currentTimezoneProvider = new CurrentTimezoneProvider { TimeZone = timeZone };
            var timezoneProvider = new TZConvertTimezoneProvider();
            return new Clock(Options.Create(options), currentTimezoneProvider, timezoneProvider);
        }

        [Fact]
        public void Now_WithUtcKind_ShouldReturnUtcNow()
        {
            var clock = CreateClock(DateTimeKind.Utc);
            var now = clock.Now;

            Assert.Equal(DateTimeKind.Utc, now.Kind);
            Assert.True((DateTime.UtcNow - now).TotalSeconds < 1);
        }

        [Fact]
        public void Now_WithLocalKind_ShouldReturnLocalNow()
        {
            var clock = CreateClock(DateTimeKind.Local);
            var now = clock.Now;

            Assert.Equal(DateTimeKind.Local, now.Kind);
        }

        [Fact]
        public void Now_WithUnspecifiedKind_ShouldReturnLocalKind()
        {
            var clock = CreateClock(DateTimeKind.Unspecified);
            var now = clock.Now;

            Assert.Equal(DateTimeKind.Local, now.Kind);
        }

        [Fact]
        public void Kind_ShouldReturnConfiguredKind()
        {
            var clock = CreateClock(DateTimeKind.Utc);
            Assert.Equal(DateTimeKind.Utc, clock.Kind);
        }

        [Fact]
        public void SupportsMultipleTimezone_WithUtcKind_ShouldReturnTrue()
        {
            var clock = CreateClock(DateTimeKind.Utc);
            Assert.True(clock.SupportsMultipleTimezone);
        }

        [Fact]
        public void SupportsMultipleTimezone_WithNonUtcKind_ShouldReturnFalse()
        {
            var clock = CreateClock(DateTimeKind.Local);
            Assert.False(clock.SupportsMultipleTimezone);
        }

        [Fact]
        public void Normalize_WithSameKind_ShouldReturnSameDateTime()
        {
            var clock = CreateClock(DateTimeKind.Utc);
            var dateTime = DateTime.UtcNow;
            var normalized = clock.Normalize(dateTime);

            Assert.Equal(dateTime, normalized);
        }

        [Fact]
        public void Normalize_WithLocalToUtc_ShouldConvertToUtc()
        {
            var clock = CreateClock(DateTimeKind.Utc);
            var localTime = DateTime.Now;
            var normalized = clock.Normalize(localTime);

            Assert.Equal(DateTimeKind.Utc, normalized.Kind);
        }

        [Fact]
        public void Normalize_WithUtcToLocal_ShouldConvertToLocal()
        {
            var clock = CreateClock(DateTimeKind.Local);
            var utcTime = DateTime.UtcNow;
            var normalized = clock.Normalize(utcTime);

            Assert.Equal(DateTimeKind.Local, normalized.Kind);
        }

        [Fact]
        public void Normalize_WithUnspecifiedKind_ShouldReturnSame()
        {
            var clock = CreateClock(DateTimeKind.Unspecified);
            var dateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Unspecified);
            var normalized = clock.Normalize(dateTime);

            Assert.Equal(dateTime, normalized);
        }

        [Fact]
        public void ConvertToUserTime_WithUtcKindAndTimezone_ShouldConvert()
        {
            var clock = CreateClock(DateTimeKind.Utc, "China Standard Time");
            var utcTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var userTime = clock.ConvertToUserTime(utcTime);

            Assert.Equal(20, userTime.Hour);
        }

        [Fact]
        public void ConvertToUserTime_WithNonUtcKind_ShouldReturnSame()
        {
            var clock = CreateClock(DateTimeKind.Local, "China Standard Time");
            var dateTime = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var result = clock.ConvertToUserTime(dateTime);

            Assert.Equal(dateTime, result);
        }

        [Fact]
        public void ConvertToUserTime_DateTimeOffset_WithTimezone_ShouldConvert()
        {
            var clock = CreateClock(DateTimeKind.Utc, "China Standard Time");
            var utcOffset = new DateTimeOffset(2024, 1, 1, 12, 0, 0, TimeSpan.Zero);
            var userTime = clock.ConvertToUserTime(utcOffset);

            Assert.Equal(20, userTime.Hour);
        }

        [Fact]
        public void ConvertToUtc_WithLocalTimeAndTimezone_ShouldConvert()
        {
            var clock = CreateClock(DateTimeKind.Utc, "China Standard Time");
            var localTime = new DateTime(2024, 1, 1, 20, 0, 0, DateTimeKind.Local);
            var utcTime = clock.ConvertToUtc(localTime);

            Assert.Equal(DateTimeKind.Utc, utcTime.Kind);
        }

        [Fact]
        public void ConvertToUtc_WithNonUtcKind_ShouldReturnSame()
        {
            var clock = CreateClock(DateTimeKind.Local, "China Standard Time");
            var dateTime = new DateTime(2024, 1, 1, 12, 0, 0);
            var result = clock.ConvertToUtc(dateTime);

            Assert.Equal(dateTime, result);
        }
    }

    public class CurrentTimezoneProviderTest
    {
        [Fact]
        public void TimeZone_GetSet_ShouldWork()
        {
            var provider = new CurrentTimezoneProvider();
            Assert.Null(provider.TimeZone);

            provider.TimeZone = "China Standard Time";
            Assert.Equal("China Standard Time", provider.TimeZone);

            provider.TimeZone = null;
            Assert.Null(provider.TimeZone);
        }
    }

    public class TZConvertTimezoneProviderTest
    {
        private readonly TZConvertTimezoneProvider _provider;

        public TZConvertTimezoneProviderTest()
        {
            _provider = new TZConvertTimezoneProvider();
        }

        [Fact]
        public void GetWindowsTimezones_ShouldReturnList()
        {
            var timezones = _provider.GetWindowsTimezones();
            Assert.NotEmpty(timezones);
            Assert.Contains(timezones, t => t.Name == "China Standard Time");
        }

        [Fact]
        public void GetIanaTimezones_ShouldReturnList()
        {
            var timezones = _provider.GetIanaTimezones();
            Assert.NotEmpty(timezones);
            Assert.Contains(timezones, t => t.Name == "Asia/Shanghai");
        }

        [Fact]
        public void WindowsToIana_ShouldConvertCorrectly()
        {
            var iana = _provider.WindowsToIana("China Standard Time");
            Assert.Equal("Asia/Shanghai", iana);
        }

        [Fact]
        public void IanaToWindows_ShouldConvertCorrectly()
        {
            var windows = _provider.IanaToWindows("Asia/Shanghai");
            Assert.Equal("China Standard Time", windows);
        }

        [Fact]
        public void GetTimeZoneInfo_WithWindowsId_ShouldReturnTimeZoneInfo()
        {
            var tzInfo = _provider.GetTimeZoneInfo("China Standard Time");
            Assert.NotNull(tzInfo);
            // On Linux, TimeZoneInfo.Id returns IANA timezone ID, not Windows timezone ID
            // So we just verify the timezone is correctly resolved by checking the base UTC offset
            Assert.Equal(TimeSpan.FromHours(8), tzInfo.BaseUtcOffset);
        }

        [Fact]
        public void GetTimeZoneInfo_WithIanaId_ShouldReturnTimeZoneInfo()
        {
            var tzInfo = _provider.GetTimeZoneInfo("Asia/Shanghai");
            Assert.NotNull(tzInfo);
        }

        [Fact]
        public void GetTimeZoneInfo_WithUTC_ShouldReturnTimeZoneInfo()
        {
            var tzInfo = _provider.GetTimeZoneInfo("UTC");
            Assert.NotNull(tzInfo);
            Assert.Equal(TimeSpan.Zero, tzInfo.BaseUtcOffset);
        }
    }
}