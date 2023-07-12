using DotCommon.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DotCommon.Test.Utility
{
    public class DateTimeUtilTest
    {
        [Fact]
        public void ToInt32_FromInt32_Test()
        {
            var dateTime = DateTime.Now.ToLocalTime();
            var int32DateTime = DateTimeUtil.ToInt32(dateTime);
            Assert.True(int32DateTime > 0);
            var dateTime2 = DateTimeUtil.ToDateTime(int32DateTime);

            var dvalue = dateTime2 - dateTime;
            Assert.True(dvalue.TotalSeconds < 1);

            var int32DateTime2 = DateTimeUtil.ToInt32(dateTime.ToString("yyyy-MM-dd hh:mm:ss"), 0);
            var dateTime3 = DateTimeUtil.ToDateTime(int32DateTime2);

            //var dvalue2 = dateTime3 - dateTime;
            //Assert.True(dvalue2.TotalSeconds < 2);

            var int32DateTime4 = DateTimeUtil.ToInt32("2019-08-12 111111", 33);
            Assert.Equal(33, int32DateTime4);
        }


        [Fact]
        public void ToInt64_FromInt64_Test()
        {
            var dateTime = DateTime.Now;
            var int64DateTime = DateTimeUtil.ToInt64(dateTime);
            Assert.True(int64DateTime > 0);
            var dateTime2 = DateTimeUtil.ToDateTime(int64DateTime);

            var dvalue = dateTime2 - dateTime;
            Assert.True(dvalue.TotalSeconds < 1);

            var d2 = DateTime.UtcNow;
            var i1 = DateTimeUtil.ToInt64(dateTime);
            var d2_2 = DateTimeUtil.ToDateTime(i1);
            Assert.Equal(d2.Hour, d2_2.Hour);
        }

        [Fact]
        public void GetPad_Test()
        {
            var dateTime = new DateTime(2019, 3, 5, 23, 0, 0);
            var padDayExpected = "20190305";
            Assert.Equal(padDayExpected, DateTimeUtil.GetPadDay(dateTime));

            var padSecondExpected = "20190305230000";
            Assert.Equal(padSecondExpected, DateTimeUtil.GetPadSecond(dateTime));

            var padSecondWithoutPrefixExpected = "190305230000";
            Assert.Equal(padSecondWithoutPrefixExpected, DateTimeUtil.GetPadSecondWithoutPrefix(dateTime));

            var padMillSecondExpected = "20190305230000000";
            Assert.Equal(padMillSecondExpected, DateTimeUtil.GetPadMillSecond(dateTime));

            var padMillSecondWithoutPrefixExpected = "190305230000000";
            Assert.Equal(padMillSecondWithoutPrefixExpected, DateTimeUtil.GetPadMillSecondWithoutPrefix(dateTime));
        }

        [Fact]
        public void GetWeek_Test()
        {
            var dt1 = new DateTime(2019, 8, 3);
            var dt2 = new DateTime(2019, 8, 6);
            var weekCrossExpected = "6,0,1,2";
            Assert.Equal(weekCrossExpected, DateTimeUtil.GetWeekCross(dt1, dt2));
            Assert.Empty(DateTimeUtil.GetWeekCross(dt2, dt1));

            var dt3= new DateTime(2019, 8, 18);
            Assert.Equal("0,1,2,3,4,5,6", DateTimeUtil.GetWeekCross(dt1, dt3));


            var chineseWeekOfDayExpected = "星期日";
            Assert.Equal(chineseWeekOfDayExpected, DateTimeUtil.GetChineseWeekOfDay(new DateTime(2019, 8, 4)));

            var weekDays = DateTimeUtil.GetWeekDays();
            Assert.Equal(7, weekDays.Count);
            Assert.Equal("星期日", weekDays[0]);

        }

        [Fact]
        public void GetDay_Test()
        {
            var date1 = new DateTime(2019, 8, 13);
            var firstDayOfMonthExpected = new DateTime(2019, 8, 1);
            Assert.Equal(firstDayOfMonthExpected, DateTimeUtil.GetFirstDayOfMonth(date1));

            var lastDayOfMonthExpected = new DateTime(2019, 8, 31);
            Assert.Equal(lastDayOfMonthExpected, DateTimeUtil.GetLastDayOfMonth(date1));

            var firstDayOfYearExpected = new DateTime(2019, 1, 1);
            Assert.Equal(firstDayOfYearExpected, DateTimeUtil.GetFirstDayOfYear(date1));

            var lastDayOfYearExpected = new DateTime(2019, 12, 31);
            Assert.Equal(lastDayOfYearExpected, DateTimeUtil.GetLastDayOfYear(date1));

            Assert.False(DateTimeUtil.IsFirstDayOfYear(new DateTime(2019, 1, 2)));
            Assert.False(DateTimeUtil.IsLastDayOfYear(new DateTime(2019, 12, 30)));
            Assert.True(DateTimeUtil.IsFirstDayOfMonth(new DateTime(2019, 8, 1)));
            Assert.True(DateTimeUtil.IsLastDayOfMonth(new DateTime(2019, 8, 31)));

        }

        [Fact]
        public void DayReplace_Test()
        {
            var replaceDay = DateTimeUtil.ReplaceDay("02", new DateTime(2019, 8, 10));
            var replaceDayExpected = new DateTime(2019, 8, 2);
            Assert.Equal(replaceDayExpected, replaceDay);


            var replaceTime = DateTimeUtil.ReplaceTime("19:30:20", new DateTime(2019, 8, 9, 10, 11, 12));
            var replaceTimeExpected = new DateTime(2019, 8, 9, 19, 30, 20);
            Assert.Equal(replaceTimeExpected, replaceTime);
        }



        [Fact]
        public void Time_Convert_Test()
        {
            var dateTime = DateTime.Now;
            var timeSpan = dateTime - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var begtime = timeSpan.TotalSeconds * 10000000; //100毫微秒为单位
            var dt1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var tricks1970 = dt1970.Ticks; //1970年1月1日刻度
            var timeTricks = tricks1970 + begtime; //日志日期刻度
            var dt2 = new DateTime(Convert.ToInt64(timeTricks)); //转化为DateTime

            Assert.Equal(dateTime.Second, dt2.Second);

        }
    }
}
