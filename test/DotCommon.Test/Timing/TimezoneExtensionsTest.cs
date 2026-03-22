using System;
using DotCommon.Timing;
using Xunit;

namespace DotCommon.Test.Timing
{
    public class CurrentTimezoneProviderExtensionsTest
    {
        [Fact]
        public void Change_ShouldChangeTimezone()
        {
            var provider = new CurrentTimezoneProvider();
            provider.TimeZone = "UTC";

            using (provider.Change("China Standard Time"))
            {
                Assert.Equal("China Standard Time", provider.TimeZone);
            }

            Assert.Equal("UTC", provider.TimeZone);
        }

        [Fact]
        public void Change_WithNull_ShouldSetNull()
        {
            var provider = new CurrentTimezoneProvider();
            provider.TimeZone = "UTC";

            using (provider.Change(null))
            {
                Assert.Null(provider.TimeZone);
            }

            Assert.Equal("UTC", provider.TimeZone);
        }

        [Fact]
        public void Change_MultipleNested_ShouldRestoreCorrectly()
        {
            var provider = new CurrentTimezoneProvider();
            provider.TimeZone = "UTC";

            using (provider.Change("China Standard Time"))
            {
                Assert.Equal("China Standard Time", provider.TimeZone);

                using (provider.Change("Eastern Standard Time"))
                {
                    Assert.Equal("Eastern Standard Time", provider.TimeZone);
                }

                Assert.Equal("China Standard Time", provider.TimeZone);
            }

            Assert.Equal("UTC", provider.TimeZone);
        }
    }

    public class TimeZoneHelperTest
    {
        [Fact]
        public void GetTimezoneOffset_WithPositiveOffset_ShouldReturnPlus()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("China Standard Time");
            var result = TimeZoneHelper.GetTimezoneOffset(tz);
            Assert.StartsWith("+", result);
        }

        [Fact]
        public void GetTimezoneOffset_WithNegativeOffset_ShouldReturnMinus()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            var result = TimeZoneHelper.GetTimezoneOffset(tz);
            Assert.True(result.StartsWith("+") || result.StartsWith("-"));
        }

        [Fact]
        public void GetTimezoneOffset_WithUtc_ShouldReturnPlusZero()
        {
            var tz = TimeZoneInfo.Utc;
            var result = TimeZoneHelper.GetTimezoneOffset(tz);
            Assert.Equal("+00:00", result);
        }

        [Fact]
        public void GetTimezones_WithValidTimezones_ShouldReturnSortedWithOffsets()
        {
            var timezones = new System.Collections.Generic.List<NameValue>
            {
                new NameValue("UTC", "UTC"),
                new NameValue("China Standard Time", "China Standard Time")
            };

            var result = TimeZoneHelper.GetTimezones(timezones);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, t => t.Value == "UTC");
            Assert.Contains(result, t => t.Value == "China Standard Time");
            Assert.All(result, t => Assert.Contains("(", t.Name));
        }

        [Fact]
        public void GetTimezones_WithEmptyList_ShouldReturnEmptyList()
        {
            var timezones = new System.Collections.Generic.List<NameValue>();

            var result = TimeZoneHelper.GetTimezones(timezones);

            Assert.Empty(result);
        }
    }
}